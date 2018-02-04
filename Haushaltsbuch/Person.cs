using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haushaltsbuch
{
    public class Person
    {
        int _id;
        string _vorname;
        string _nachname;

        public Person(object nr, object vor, object nach)
        {
            _id = Convert.ToInt32(nr);
            _vorname = vor.ToString();
            _nachname = nach.ToString();
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

        public string vorname
        {
            get
            {
                return _vorname;
            }

            set
            {
                _vorname = value;
            }
        }

        public string nachname
        {
            get
            {
                return _nachname;
            }

            set
            {
                _nachname = value;
            }
        }

        public override string ToString()
        {
            return _vorname;
        }
    }
}
