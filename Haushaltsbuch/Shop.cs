using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haushaltsbuch
{
    public class Shop
    {
        int _id;
        string _name;
        bool _online;

        public Shop(object nr, object str, object nln)
        {
            _id = Convert.ToInt32(nr);
            _name = str.ToString();
            _online = (Convert.ToInt32(nln) == 0) ? false : true;
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

        public string name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        public bool online
        {
            get
            {
                return _online;
            }

            set
            {
                _online = value;
            }
        }

        public override string ToString()
        {
            return _name;
        }
    }
}
