using System;
using System.IO;

namespace BrowserFiles {
    internal class Program {
        //public const string BROWSER_DATA_DIR = @"\\vFileserver01\company$\EDV\Projekte\InEK-Browsers\Begleitforschung";
        public const string BROWSER_DATA_DIR = @"D:\tmp\BrowserFiles";

        public static string FilePrefix { get; set; }
        public static string FileDir { get; set; }
        public static int DataYear { get; set; }

        private static void Main(string[] args) {
            if (args.Length != 2) {
                Console.WriteLine("Es werden 2 Startparameter benötigt! (<Dateien-Pfad> <Datenjahr>)");
                Console.Read();
                return;
            }
            FileDir = args[0];
            try {
                DataYear = int.Parse(args[1]);
            } catch (Exception) {
                Console.WriteLine("Fehler: Datenjahr ist keine gültige Zahl");
                Console.Read();
                return;
            }
            FilePrefix = "BegleitfBr_Drg_" + DataYear + "_";
            try {
                FileChecker.CheckDirectoy(FilePrefix, FileDir, DataYear);
                Console.WriteLine("Alle benötigten Dateien sind vorhanden.");
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                Console.WriteLine("Eine Datei zur Erstellung der Browser-Daten fehlt.");
                Console.Read();
                return;
            }
            try {
                if (Directory.Exists(Path.Combine(BROWSER_DATA_DIR, DataYear + "")))
                    Directory.Delete(Path.Combine(BROWSER_DATA_DIR, DataYear + ""), true);
                Directory.CreateDirectory(Path.Combine(BROWSER_DATA_DIR, DataYear + ""));
                Console.WriteLine("Daten-Verzeichnis " + Path.Combine(BROWSER_DATA_DIR, DataYear + "") + " erstellt");
            } catch (Exception) {
                Console.WriteLine("Fehler: Konnte Browser-Datenverzeichnis nicht erstellen!");
                Console.Read();
                return;
            }
            try {
                Console.WriteLine("Erstelle Dateien..." + Environment.NewLine);
                FileBuilder.BuildFiles();
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                Console.WriteLine("Fehler: Dateierstellung fehlgeschlagen. Verzeichnis wird gelöscht.");
                if (Directory.Exists(Path.Combine(BROWSER_DATA_DIR, DataYear + "")))
                    Directory.Delete(Path.Combine(BROWSER_DATA_DIR, DataYear + ""), true);
                Console.WriteLine("Verzeichnis gelöscht.");
                Console.Read();
                return;
            }
            Console.WriteLine(Environment.NewLine+"Alle Dateien wurden erfolgreich erstellt! (" +
                              Directory.GetFiles(Path.Combine(BROWSER_DATA_DIR, DataYear + "")).Length + " Dateien)");
            Console.Read();
        }
    }
}