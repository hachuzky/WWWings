using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WWWings
{
    /// <summary>
    /// Erweiterungen der Klasse Person, wird auch Passagier und Pilot vererbt
    /// </summary>
    public partial class Person
    {
        public string GanzerName { get { return this.Vorname + " " + this.Name; } }

        public override string ToString()
        {
            return "Person #" + this.Id + ": " + this.GanzerName;
        }
    }

    /// <summary>
    /// Erweiterungen der Klasse Flug, wird auch Passagier und Pilot vererbt
    /// </summary>
    public partial class Flug
    {
        public string Route { get { return this.Abflugort + " -> " + this.Zielort; } }

        public override string ToString()
        {
            return "Flug #" + this.Id + ": " + this.Route + ": " +
                               this.FreiePlaetze + " von " + this.Plaetze + " frei.";
        }
    }
}
