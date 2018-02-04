using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haushaltsbuch
{
    public class Einkommen
    {
        int _id;
        string _bezeichnung;
        double _betrag;
        Person _person;
        DateTime _datum;

        public int Id
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

        public string Bezeichnung
        {
            get
            {
                return _bezeichnung;
            }

            set
            {
                _bezeichnung = value;
            }
        }

        public double Betrag
        {
            get
            {
                return _betrag;
            }

            set
            {
                _betrag = value;
            }
        }

        public Person Person
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

        public DateTime Datum
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

        public Einkommen(object nr, object day, object str, Person p, object b)
        {
            _id = Convert.ToInt32(nr);
            _datum = Convert.ToDateTime(day);
            _bezeichnung = str.ToString();
            _person = p;
            _betrag = Convert.ToDouble(b);
        }

    }
}
