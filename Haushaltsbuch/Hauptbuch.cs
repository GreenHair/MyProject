using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows;
using System.Collections;

namespace Haushaltsbuch
{
    public class Hauptbuch
    {
        public static string dbconnectstring =
              "SERVER=192.168.2.113;" +
                              "DATABASE=haushaltsbuch;" +
                              "UID=auke;" +
                              "PASSWORD=ich;";
        MySqlConnection connection = new MySqlConnection(dbconnectstring);
        MySqlCommand command;// = connection.CreateCommand();
        MySqlDataReader Reader;

        List<Rechnung> _ausgaben = new List<Rechnung>();
        List<Rechnung> _dieseWoche = new List<Rechnung>();
        List<Rechnung> _letzteWoche = new List<Rechnung>();
        List<Einkommen> _einnahmen = new List<Einkommen>();
        List<Person> _familienmitglied = new List<Person>();
        List<Shop> alleLaeden = new List<Shop>();
        List<Produktgruppe> kategorien = new List<Produktgruppe>();
        double _bilanz;
        public bool IstVerbunden { get; private set; }

        public Hauptbuch()
        {
            try
            {
                connection = new MySqlConnection(dbconnectstring);
                command = connection.CreateCommand();
                connection.Open();                

                command.CommandText = "SELECT * FROM familienmitglied";
                Reader = command.ExecuteReader(); 
           
                while (Reader.Read())
                {
                    Person p = new Person(Reader["ID"], Reader["vorname"], Reader["nachname"]);
                    _familienmitglied.Add(p);
                }
                Reader.Close();

                command.CommandText = "SELECT * FROM laden";
                //connection.Open();
                Reader = command.ExecuteReader();

                while (Reader.Read())
                {
                    Shop l = new Shop(Reader[0], Reader[1], Reader[2]);
                    alleLaeden.Add(l);
                }
                Reader.Close();

                command.CommandText = "SELECT * FROM produktgruppe";
               // connection.Open();
                Reader = command.ExecuteReader();

                while (Reader.Read())
                {
                    Produktgruppe p = new Produktgruppe(Reader[0], Reader[1], Reader[2]);
                    kategorien.Add(p);
                }
                Reader.Close();

                //_ausgaben = listeBauen("SELECT * FROM rechnung");
            
                //_dieseWoche = listeBauen("SELECT * FROM rechnung WHERE WEEK(rechnung.datum,1)=WEEK(CURRENT_DATE(),1)");

                //_letzteWoche = listeBauen("SELECT * FROM rechnung WHERE WEEK(rechnung.datum,1)=WEEK(CURRENT_DATE(),1)-1");

                command.CommandText = "SELECT * FROM einkommen";
               // connection.Open();
                Reader = command.ExecuteReader();
                while (Reader.Read())
                {
                    Einkommen e = new Einkommen(Reader[0], Reader[1], Reader[2], _familienmitglied[Convert.ToInt32(Reader[3])-1], Reader[4]);
                    _einnahmen.Add(e);
                }
                Reader.Close();
               // connection.Close();
            }
            catch (MySqlException e)
            {                
                ,string str = e.Message;
            }
            IstVerbunden = connection.State == System.Data.ConnectionState.Open;
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
            set
            {
                alleLaeden = value;
            }
        }

        public List<Produktgruppe> Kategorien
        {
            get
            {
                return kategorien;
            }
            set
            {
                kategorien = value;
            }
        }

        private List<Rechnung> listeBauen(string queryString)
        {
            List<Rechnung> temp = new List<Rechnung>();
            command.CommandText = queryString;
            //connection.Open();
            if (connection.State == System.Data.ConnectionState.Open)
            {
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
                //connection.Close();
            }
            return temp;
        }

        public List<Rechnung> GetRechnung_M()
        {
            string query = "SELECT * FROM `rechnung` WHERE MONTH(rechnung.datum) = MONTH(CURRENT_DATE())";
            return listeBauen(query);
        }

        public List<Rechnung> GetRechnung_M(int i)
        {
            if (DateTime.Now.Month - i < 1)
            {
                i = 13 - i;
            }
            else
            {
                i = DateTime.Now.Month - i;
            }
            string query = "SELECT * FROM `rechnung` WHERE MONTH(rechnung.datum) = " + i;
            return listeBauen(query);
        }

        public List<Rechnung> GetRechnung_W()
        {
            return listeBauen("SELECT * FROM rechnung WHERE WEEK(rechnung.datum,1)=WEEK(CURRENT_DATE(),1)");
        }

        public List<Rechnung> GetRechnung_W(int i)
        {
            return listeBauen("SELECT * FROM rechnung WHERE WEEK(rechnung.datum,1)=WEEK(CURRENT_DATE(),1)-" + i);
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

        public List<Person> GetFamilie()
        {
            List<Person> temp = new List<Person>();
            command.CommandText = "SELECT * FROM familienmitglied";
            Reader = command.ExecuteReader();
            while (Reader.Read())
            {
                Person p = new Person(Reader["ID"], Reader["vorname"], Reader["nachname"]);
                temp.Add(p);
            }
            Reader.Close();
            return temp;
        }

        public List<Shop> GetLaeden()
        {
            List<Shop> temp = new List<Shop>();
            command.CommandText = "SELECT * FROM laden";
            Reader = command.ExecuteReader();
            while (Reader.Read())
            {
                Shop l = new Shop(Reader[0], Reader[1], Reader[2]);
                temp.Add(l);
            }
            Reader.Close();
            return temp;
        }

        public List<Produktgruppe> GetKategorien()
        {
            List<Produktgruppe> temp = new List<Produktgruppe>();
            command.CommandText = "SELECT * FROM produktgruppe";
            Reader = command.ExecuteReader();
            while (Reader.Read())
            {
                Produktgruppe p = new Produktgruppe(Reader[0], Reader[1], Reader[2]);
                temp.Add(p);
            }
            Reader.Close();
            return temp;
        }

        public List<Einkommen> GetEinkommen()
        {
            List<Einkommen> temp = new List<Einkommen>();
            command.CommandText = "SELECT * FROM einkommen";
            Reader = command.ExecuteReader();
            while (Reader.Read())
            {
                Einkommen e = new Einkommen(Reader[0], Reader[1], Reader[2], _familienmitglied[Convert.ToInt32(Reader[3]) - 1], Reader[4]);
                temp.Add(e);
            }
            Reader.Close();
            return temp;
        }
        
        public int Eintragen(MySqlCommand comm)
        {
            comm.Connection = connection;            
            //connection.Open();
            comm.Prepare();
            int result = comm.ExecuteNonQuery();
            //connection.Close();
            return result;
        }

        internal string Verbinden(string connectstring)
        {
            string result;
            try
            {
                connection = new MySqlConnection(connectstring);
                command = connection.CreateCommand();
                connection.Open();
                connection.ChangeDatabase("haushaltsbuch");

                _familienmitglied = GetFamilie();
                alleLaeden = GetLaeden();
                kategorien = GetKategorien();
                _einnahmen = GetEinkommen();

                result = "Verbunden";
            }
            catch(MySqlException error)
            {
                result = error.Message;
            }
            IstVerbunden = connection.State == System.Data.ConnectionState.Open;
            return result;
        }

        internal List<Suchergebnis> Suchen(MySqlCommand command)
        {
            List<Suchergebnis> liste = new List<Suchergebnis>();
            command.Connection = connection;
            command.Prepare();
            Reader = command.ExecuteReader();
            while (Reader.Read())
            {
                Suchergebnis s = new Suchergebnis();
                Rechnung r = new Rechnung();
                Posten p = new Posten();

                r.id = (int)Reader["r_id"];
                r.laden = alleLaeden[(int)Reader["laden"]-1];
                r.datum = Convert.ToDateTime(Reader["datum"]);
                r.einmalig = Convert.ToBoolean(Reader["einmalig"]);
                r.person = _familienmitglied[(int)Reader["person"]-1];
                s.Kassenzettel = r;
                p.id = (int)Reader["a_id"];
                p.bezeichnung = Reader["bezeichnung"].ToString();
                p.betrag = Convert.ToDouble(Reader["betrag"]);
                p.kategorie = kategorien[(int)Reader["prod_gr"]-1];
                s.Artikel = p;

                liste.Add(s);
            }
            Reader.Close();
            return liste;
        }

        ~Hauptbuch()
        {
            connection.Close();
        }
    }
}
