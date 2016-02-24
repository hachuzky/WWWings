using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWWings;

namespace WWWings_DZS
{
    /// <summary>
    /// Datenmanager für Flüge
    /// </summary>
    public class FlugDataManager : IDisposable
    {

        // Eine Instanz des Entity-Framework-Kontextes pro Manager-Instanz
        WWWingsModellContainer modell = new WWWingsModellContainer();

        /// <summary>
        /// Konstruktor
        /// </summary>
        public FlugDataManager(bool LazyLoading = false)
        {
            modell.Configuration.LazyLoadingEnabled = LazyLoading;
        }


        /// <summary>
        /// Objekt vernichten
        /// </summary>
        public void Dispose()
        {
            modell.Dispose();
        }

        /// <summary>
        /// Laden eines Flugs
        /// </summary>
        public Flug GetFlug(int FlugID)
        {
            var abfrage = from flug in modell.FlugSet
                          where flug.Id ==
                            FlugID
                          select flug;
            return abfrage.SingleOrDefault();
        }

        /// <summary>
        /// Laden einer Liste von Flügen
        /// </summary>
        public List<Flug> GetFluege(string Abflugort, string Zielort)
        {
            // Grundabfrage
            var abfrage = from flug in modell.FlugSet select flug;
            // Abfrage ggf. erweitern
            if (!String.IsNullOrEmpty(Abflugort)) abfrage
                             = from flug in abfrage
                               where flug.Abflugort ==
                          Abflugort
                               select flug;
            if (!String.IsNullOrEmpty(Zielort)) abfrage
                             = from flug in abfrage
                               where flug.Zielort ==
                          Zielort
                               select flug;

            return abfrage.ToList();
        }


        /// <summary>
        /// Reduzieren der Platzanzahl
        /// </summary>
        public bool ReducePlatzAnzahl(int FlugID, short Platzanzahl)
        {
            var einzelnerFlug = GetFlug(FlugID);

            if (einzelnerFlug != null)
            {
                if (einzelnerFlug.FreiePlaetze >= Platzanzahl &&
                                  einzelnerFlug.FreiePlaetze - Platzanzahl
                                                       <= einzelnerFlug.Plaetze)
                {
                    // Änderung durchführen
                    einzelnerFlug.FreiePlaetze -= Platzanzahl;

                    // Speichern
                    modell.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Liefert eine Liste aller Abflug- und Zielflughäfen 
        /// als Zeichenkettenliste
        /// </summary>
        /// <returns></returns>
        public List<string> GetFlughaefen()
        {
            var l1 = modell.FlugSet.Select(f => f.Abflugort).Distinct();
            var l2 = modell.FlugSet.Select(f => f.Zielort).Distinct();
            var l3 = l1.Union(l2).Distinct();
            return l3.OrderBy(z => z).ToList();
        }
    }
}