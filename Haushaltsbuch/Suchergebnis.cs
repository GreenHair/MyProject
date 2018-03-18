using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haushaltsbuch
{
    class Suchergebnis
    {
        public Rechnung Kassenzettel { get; set; }
        public Posten Artikel { get; set; }

        public Suchergebnis() { }

        public Suchergebnis(Rechnung r, Posten p)
        {
            Kassenzettel = r;
            Artikel = p;
        }
    }
}
