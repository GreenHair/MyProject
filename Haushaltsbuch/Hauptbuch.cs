using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows;

namespace Haushaltsbuch
{
    public class Hauptbuch
    {
        public static string dbconnectstring =
              "SERVER=192.168.2.113;" +
                              "DATABASE=haushaltsbuch;" +
                              "UID=auke;" +
                              "PASSWORD=ich;";
        static MySqlConnection connection = new MySqlConnection(dbconnectstring);
        MySqlCommand command = connection.CreateCommand();
        MySqlDataReader Reader;

        List<Rechnung> _ausgaben = new List<Rechnung>();
        List<Rechnung> _dieseWoche = new List<Rechnung>();
        List<Rechnung> _letzteWoche = new List<Rechnung>();
        List<Einkommen> _einnahmen = new List<Einkommen>();
        List<Person> _familienmitglied = new List<Person>();
        List<Shop> alleLaeden = new List<Shop>();
        List<Produktgruppe> kategorien = new List<Produktgruppe>();
        double _bilanz;

        public Hauptbuch()
        {
            
            command.CommandText = "SELECT * FROM familienmitglied";
            
            connection.Open();
            Reader = command.ExecuteReader(); 
           
            while (Reader.Read())
            {
                Person p = new Person(Reader[0], Reader[1], Reader[2]);
                _familienmitglied.Add(p);
            }
            connection.Close();

            command.CommandText = "SELECT * FROM laden";
            connection.Open();
            Reader = command.ExecuteReader();

            while (Reader.Read())
            {
                Shop l = new Shop(Reader[0], Reader[1], Reader[2]);
                alleLaeden.Add(l);
            }
            connection.Close();

            command.CommandText = "SELECT * FROM produktgruppe";
            connection.Open();
            Reader = command.ExecuteReader();

            while (Reader.Read())
            {
                Produktgruppe p = new Produktgruppe(Reader[0], Reader[1], Reader[2]);
                kategorien.Add(p);
            }
            connection.Close();

            _ausgaben = listeBauen("SELECT * FROM rechnung");
            
            _dieseWoche = listeBauen("SELECT * FROM rechnung WHERE WEEK(rechnung.datum,1)=WEEK(CURRENT_DATE(),1)");

            _letzteWoche = listeBauen("SELECT * FROM rechnung WHERE WEEK(rechnung.datum,1)=WEEK(CURRENT_DATE(),1)-1");

            command.CommandText = "SELECT * FROM einkommen";
            connection.Open();
            Reader = command.ExecuteReader();
            while (Reader.Read())
            {
                Einkommen e = new Einkommen(Reader[0], Reader[1], Reader[2], _familienmitglied[Convert.ToInt32(Reader[3])-1], Reader[4]);
                _einnahmen.Add(e);
            }
        }

        public List<Rechnung> ausgaben
        {
            get
            {
                return _ausgaben;
            }

            set
            {
                _ausgaben = value;
            }
        }

        public List<Einkommen> einnahmen
        {
            get
            {
                return _einnahmen;
            }

            set
            {
                _einnahmen = value;
            }
        }

        public double bilanz
        {
            get
            {
                return _bilanz;
            }

            set
            {
                _bilanz = value;
            }
        }

        public List<Person> familienmitglied
        {
            get
            {
                return _familienmitglied;
            }

            set
            {
                _familienmitglied = value;
            }
        }

        public List<Rechnung> dieseWoche
        {
            get
            {
                return _dieseWoche;
            }

            set
            {
                _dieseWoche = value;
            }
        }

        public List<Rechnung> LetzteWoche
        {
            get
            {
                return _letzteWoche;
            }

            set
            {
                _letzteWoche = value;
            }
        }

        public List<Shop> AlleLaeden
        {
            get
            {
                return alleLaeden;
            }
        }

        public List<Produktgruppe> Kategorien
        {
            get
            {
                return kategorien;
            }
        }

        private List<Rechnung> listeBauen(string queryString)
        {
            List<Rechnung> temp = new List<Rechnung>();
            command.CommandText = queryString;
            connection.Open();
            Reader = command.ExecuteReader();
            while (Reader.Read())
            {
                Rechnung r = new Rechnung(Reader[0], alleLaeden[Convert.ToInt32(Reader[1]) - 1], Reader[2], Reader[3], _familienmitglied[Convert.ToInt32(Reader[4]) - 1]);
                temp.Add(r);
            }
            Reader.Close();

            foreach (Rechnung kassenzettel in temp)
            {
                command.CommandText = "SELECT * FROM ausgaben WHERE rechnungsnr=" + kassenzettel.id;
                //connection.Open();
                Reader = command.ExecuteReader();
                while (Reader.Read())
                {
                    Posten p = new Posten(Reader[0], Reader[1], Reader[2], kategorien[Convert.ToInt32(Reader[3]) - 1]);
                    kassenzettel.items.Add(p);
                }
                Reader.Close();
            }
            connection.Close();
            return temp;
        }

        public List<Rechnung> monatsliste(int i)
        {
            List<Rechnung> temp = new List<Rechnung>();

            if(DateTime.Now.Month - i < 1)
            {
                i = 13 - i;
            }
            else
            {
                i = DateTime.Now.Month - i;
            }

            var monat = from rechnung in _ausgaben where rechnung.datum.Month == i select rechnung;
            foreach(var item in monat)
            {
                temp.Add(item);
            }

            return temp;
        }
        
    }
}
