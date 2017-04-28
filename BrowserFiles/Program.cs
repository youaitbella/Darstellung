using System;
using System.IO;
using Ionic.Zip;

namespace BrowserFiles {
    internal class Program {

        public const string BROWSER_EXPORT_DATA_DIR_TEST = @"\\vFileserver01\company$\EDV\Projekte\InEK-Browsers\BegleitforschungTest";
        public const string BROWSER_EXPORT_DATA_DIR_LIVE = @"\\vFileserver01\company$\EDV\Projekte\InEK-Browsers\Begleitforschung";

        public static string BROWSER_EXPORT_DATA_DIR = BROWSER_EXPORT_DATA_DIR_TEST;

        public static string FilePrefix { get; set; }
        public static string FileDir { get; set; }
        public static int DataYear { get; set; }

        private static void Main(string[] args) {
            if (args.Length < 2) {
                Console.WriteLine("Es werden mindestens 2 Startparameter benötigt! (<Dateien-Pfad> <Datenjahr> [<Livemodus (true/false)>])");
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
            if (args.Length > 2) {
                try {
                    bool liveMode = bool.Parse(args[2]);
                    if (liveMode) {
                        BROWSER_EXPORT_DATA_DIR = BROWSER_EXPORT_DATA_DIR_LIVE;
                    }
                    else {
                        BROWSER_EXPORT_DATA_DIR = BROWSER_EXPORT_DATA_DIR_TEST;
                    }
                } catch (Exception ex) {}
            }
            try {
                if (Directory.Exists(Path.Combine(BROWSER_EXPORT_DATA_DIR, DataYear + "")))
                    Directory.Delete(Path.Combine(BROWSER_EXPORT_DATA_DIR, DataYear + ""), true);
                Directory.CreateDirectory(Path.Combine(BROWSER_EXPORT_DATA_DIR, DataYear + ""));
                Console.WriteLine("Daten-Verzeichnis " + Path.Combine(BROWSER_EXPORT_DATA_DIR, DataYear + "") + " erstellt");
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
                if (Directory.Exists(Path.Combine(BROWSER_EXPORT_DATA_DIR, DataYear + "")))
                    Directory.Delete(Path.Combine(BROWSER_EXPORT_DATA_DIR, DataYear + ""), true);
                Console.WriteLine("Verzeichnis gelöscht.");
                Console.Read();
                return;
            }
            Console.WriteLine(Environment.NewLine+"Alle Dateien wurden erfolgreich erstellt! (" +
                              Directory.GetFiles(Path.Combine(BROWSER_EXPORT_DATA_DIR, DataYear + "")).Length + " Dateien)");
            Console.WriteLine(Environment.NewLine + "Erstelle ZIP-Download-Datei...");
            try {
                string downloadDir = Path.Combine(BROWSER_EXPORT_DATA_DIR, DataYear + "", "download");
                Directory.CreateDirectory(downloadDir);
                using (ZipFile zip = new ZipFile()) {
                    zip.AddDirectory(FileDir);
                    zip.Save(Path.Combine(downloadDir, "Begleit-Daten-"+DataYear+".zip"));
                }
                Console.WriteLine("ZIP-Datei erstellt.");
            } catch (Exception ex) {
                Console.WriteLine("Fehler beim Erstellen der ZIP-Datei... bitte manuell erstellen oder Porgramm neu starten. " + Environment.NewLine + ex.Message);
            }
            Console.WriteLine("Es wurden alle Dateien erstellt. Taste zum Fortfahren drücken.");
            Console.Read();
        }
    }
}