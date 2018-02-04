using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Haushaltsbuch
{
    public class Rechnung
    {
        int _id;
        DateTime _datum;
        bool _einmalig;
        Person _person;
        List<Posten> _items = new List<Posten>();
        Shop _laden;

        public Rechnung(object nr,Shop l, object day, object ein, Person p)
        {
            _id = Convert.ToInt32(nr);
            _laden = l;
            _datum = Convert.ToDateTime(day);
            _einmalig = (Convert.ToInt32(ein) == 0) ? false : true;
            _person = p;
        }

        public int id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        public DateTime datum
        {
            get
            {
                return _datum;
            }

            set
            {
                _datum = value;
            }
        }

        public bool einmalig
        {
            get
            {
                return _einmalig;
            }

            set
            {
                _einmalig = value;
            }
        }

        public Person person
        {
            get
            {
                return _person;
            }

            set
            {
                _person = value;
            }
        }

        public List<Posten> items
        {
            get
            {
                return _items;
            }

            set
            {
                _items = value;
            }
        }

        public Shop laden
        {
            get
            {
                return _laden;
            }

            set
            {
                _laden = value;
            }
        }
    }
}
