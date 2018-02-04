using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haushaltsbuch
{
    public class Produktgruppe
    {
        int _id;
        string _bezeichnung;
        bool _essbar;

        public Produktgruppe(object nr, object str, object essen)
        {
            _id = Convert.ToInt32(nr);
            _bezeichnung = str.ToString();
            _essbar = (Convert.ToInt32(essen) == 0) ? false : true;
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

        public bool essbar
        {
            get
            {
                return _essbar;
            }

            set
            {
                _essbar = value;
            }
        }

        public override string ToString()
        {
            return _bezeichnung;
        }
    }
}
