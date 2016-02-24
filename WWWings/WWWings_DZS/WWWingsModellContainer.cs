using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WWWings_DZS
{
    /// <summary>
    /// Erweiterung der Entity Framework-Kontextklasse
    /// </summary>
    public partial class WWWingsModellContainer
    {
        public static string Protokolldatei;
        static WWWingsModellContainer()
        {
            Protokolldatei = Path.Combine(@"c:\temp", "EFLog.csv");
        }


        /// <summary>
        /// Überschreiben von SaveChanges: Zusätzliches Protokollieren in Datei
        /// </summary>
        public override int SaveChanges()
        {
            List<ObjectStateEntry> neue = new List<ObjectStateEntry>();

            // Alle Änderngen aus den Objekten sammeln
            this.ChangeTracker.DetectChanges();

            // Hole geänderte
            foreach (var ose in ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager.GetObjectStateEntries(System.Data.Entity.EntityState.Modified))
            {
                foreach (var mprop in ose.GetModifiedProperties())
                {
                    WriteProtokoll(ose.EntitySet.Name,
                           (int)ose.EntityKey.EntityKeyValues[0].Value,
                               "Modified", mprop, ose.OriginalValues[mprop].ToString(),
                                   ose.CurrentValues[mprop].ToString(), "");
                }
            }


            // Hole neue
            foreach (var ose in ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager
                        .GetObjectStateEntries(System.Data.Entity.EntityState.Added))
            {
                // Erst mal nur merken, denn die Autowerte sind noch nicht gesetzt!
                neue.Add(ose);
            }


            // Hole gelöschte
            foreach (var ose in ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager
                      .GetObjectStateEntries(System.Data.Entity.EntityState.Deleted))
            {
                WriteProtokoll(ose.EntitySet.Name, (int)ose.EntityKey
                           .EntityKeyValues[0].Value, "Deleted", "", "", "", "");
            }

            // Nun Standardimplementierung aufrufen
            int Anzahl = base.SaveChanges();

            // Jetzt noch die neuen behandeln
            foreach (var ose in neue)
            {
                if (ose.EntityKey != null && ose.EntityKey.EntityKeyValues != null)
                    WriteProtokoll(ose.EntitySet.Name, (int)ose.EntityKey
                          .EntityKeyValues[0].Value, "Added", "", "", "", "");
            }


            return Anzahl;
        }

        /// <summary>
        /// Erzeuge Protokolleintrag
        /// </summary>
        public void WriteProtokoll(string Entity,
                   int EntityID, string Aktion, string Attribut,
                           string AlterWert, string NeuerWert, string Text)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter
                           (WWWingsModellContainer.Protokolldatei, true);

            sw.WriteLine(System.Environment.UserDomainName + "\\"
                 + System.Environment.UserName + ";" + DateTime.Now +
                       ";" + Entity + ";" + EntityID + ";" + Aktion + ";"
                           + Attribut + ";" + AlterWert +
                                 ";" + NeuerWert + ";" + Text);
            sw.Close();
        }

    }
}
