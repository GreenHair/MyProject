using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haushaltsbuch
{
    public class Posten
    {
        int _id;
        string _bezeichnung;
        double _betrag;
        Produktgruppe _kategorie;

        public Posten(object nr, object str, object amount, Produktgruppe pr)
        {
            _id = Convert.ToInt32(nr);
            _bezeichnung = str.ToString();
            _betrag = Convert.ToDouble(amount);
            _kategorie = pr;
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

        public string bezeichnung
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

        public double betrag
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

        public Produktgruppe kategorie
        {
            get
            {
                return _kategorie;
            }

            set
            {
                _kategorie = value;
            }
        }
    }
}
