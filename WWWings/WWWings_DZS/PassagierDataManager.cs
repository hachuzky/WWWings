using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWWings;

namespace WWWings_DZS
{

    /// <summary>

    /// Datenmanager für Passagiere
    /// </summary>
    public class PassagierDataManager : IDisposable
    {

        // Eine Instanz des Datenkontextes pro Manager-Instanz

        WWWingsModellContainer modell = new WWWingsModellContainer();

        /// <summary>

        /// Konstruktor
        /// </summary>
        public PassagierDataManager(bool LazyLoading = false)
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

        /// Holt einen Passagier
        /// </summary>
        public Passagier GetPassagier(int PassagierID)
        {
            // .OfType<Passagier>() notwendig wegen Vererbung
            var abfrage = from p in modell.PersonSet.OfType<Passagier>()
                          where p.Id == PassagierID
                          select p;
            return abfrage.SingleOrDefault();
        }

        /// <summary>

        /// Holt alle Passagiere mit einem Namensbestandteil
        /// </summary>
        public List<Passagier> GetPassagiere(string Namensbestandteil)
        {
            // .OfType<Passagier>() notwendig wegen Vererbung
            var abfrage = from p in modell.PersonSet.OfType<Passagier>()
                          where p.Name.Contains(Namensbestandteil)
                            || p.Vorname.Contains(Namensbestandteil)
                          select p;
            return abfrage.ToList();
        }

        /// <summary>

        /// Füge einen Passagier zu einem Flug hinzu
        /// </summary>
        public bool AddPassagierZuFlug(int PassagierID, int FlugID)
        {
            try
            {
                Flug flug = modell.FlugSet.Where(f => f.Id ==
                                                  FlugID).SingleOrDefault();
                Passagier passagier = modell.PersonSet.OfType<Passagier>().Where(p => p.Id == PassagierID).SingleOrDefault();
                flug.Passagier.Add(passagier);

                modell.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>

        /// Änderungen an einer Liste von Passagieren speichern
        /// Die neu hinzugefügten Passagiere muss die Routine wieder 
        /// zurückgeben, da die IDs für die neuen Passagiere erst beim 
        /// Speichern von der Datenbank vergeben werden
        /// </summary>
        public List<Passagier> SavePassagierSet(List<Passagier>
                                  PassagierSet, out string Statistik)
        {

            // Änderungen für jeden einzelnen Passagier übernehmen

            foreach (Passagier p in PassagierSet)
            {
                modell.PersonSet.Add(p);
            }

            // Statistik der Änderungen zusammenstellen
            var manager = ((IObjectContextAdapter)modell).ObjectContext.ObjectStateManager;
            Statistik = "";
            Statistik += "Geändert: " + manager.GetObjectStateEntries(System.Data.Entity.EntityState.Modified).Count();
            Statistik += " Neu: " + manager.GetObjectStateEntries(System.Data.Entity.EntityState.Added).Count();
            Statistik += " Gelöscht: " + manager.GetObjectStateEntries(System.Data.Entity.EntityState.Deleted).Count();

            // Neue Datensätze merken, da diese nach Speichern

            // zurückgegeben werden müssen (haben dann erst ihre IDs!)

            List<Passagier> NeuePassagiere = PassagierSet.Where(f
                => modell.Entry(f).State == System.Data.Entity.EntityState.Added).ToList();

            // Änderungen speichern

            modell.SaveChanges();

            // Statistik der Änderungen zurückgeben

            return NeuePassagiere;
        }

    }
}
