using System;
using System.IO;
using System.Text;

namespace BrowserFiles {
    public static class FileBuilder {
        public static void BuildFiles() {
            BuildHospitalStateSizeBeds(); // B_1_KH_Bundesland_Groesse(Betten).csv
            BuildHospitalProviderSizeCases(); // B_2_KH_Traeger_Groesse(Faelle).csv
            BuildHospitalSizeCmi(); // B_3_KH_Groesse(Betten)_CMI.csv
            BuildStateSizeFzVwdCmi(); // C_121_221_Bundesland_Groesse(Betten)_FZ_VWD_CMI.csv
            BuildStateSizeFzVwdCmiSum(); // C_121_221_sum.csv
            BuildMdcGender(); // C_111_211.csv
            BuildAgeClasses(); // C_112_212.csv
            BuildDrg(); // C_113_213.csv
        }

        private static StreamReader CreateReaderWithoutHeadline(string fileName) {
            var reader = new StreamReader(Path.Combine(Program.FileDir, Program.FilePrefix + fileName));
            reader.ReadLine();
            return reader;
        }

        private static StreamWriter CreateWriter(string fileName) {
            return
                new StreamWriter(
                    new FileStream(Path.Combine(Program.BROWSER_DATA_DIR, Program.DataYear + "", fileName),
                        FileMode.Create), Encoding.UTF8);
        }

        private static void PrintCreateFileSuccessfull(string fileName) {
            Console.WriteLine("Datei " + fileName + " erstellt!");
        }

        private static string ReplaceFirst(string text, string search, string replace) {
            var pos = text.IndexOf(search, StringComparison.Ordinal);
            if (pos < 0) {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        /**
            B_1_KH_Bundesland_Groesse(Betten).csv
        */

        private static void BuildHospitalStateSizeBeds() {
            var germanyFile = "B_1_KH_Groesse(Betten).csv";
            var stateFile = "B_1_KH_Bundesland_Groesse(Betten).csv";
            var headline = "Bundesland_A;Stufe;Anzahl KH;Anteil KH;BLCode;BettenstufeSort";
            var germany = "";
            var states = "";
            using (
                var reader = CreateReaderWithoutHeadline(germanyFile)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    germany += "00_Deutschland;" + line + Environment.NewLine;
                }
            }
            using (
                var reader = CreateReaderWithoutHeadline(stateFile)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    states += line + Environment.NewLine;
                }
            }
            using (
                var writer = CreateWriter(stateFile)) {
                writer.WriteLine(headline);
                writer.Write(germany);
                writer.Write(states);
            }
            PrintCreateFileSuccessfull(stateFile);
        }

        /**
            B_2_KH_Traeger_Groesse(Faelle).csv
        */

        private static void BuildHospitalProviderSizeCases() {
            var allFile = "B_2_KH_Groesse(Faelle).csv";
            var providerFile = "B_2_KH_Traeger_Groesse(Faelle).csv";
            var headline = "TraegerName;Fallstufe;Anzahl KH;Anteil KH;Traeger_Nr;TraegerSort;FallstufeSort";
            var all = "";
            var provider = "";
            using (var reader = CreateReaderWithoutHeadline(allFile)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    line = line.Insert(line.LastIndexOf(";", StringComparison.Ordinal), ";A;0");
                    all += "Alle;" + line + Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(providerFile)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    provider += line + Environment.NewLine;
                }
            }
            using (var writer = CreateWriter(providerFile)) {
                writer.WriteLine(headline);
                writer.Write(all);
                writer.Write(provider);
            }
            PrintCreateFileSuccessfull(providerFile);
        }

        /**
            B_3_KH_Groesse(Betten)_CMI.csv
        */

        private static void BuildHospitalSizeCmi() {
            var allFile = "B_3_KH_CMI.csv";
            var levelFile = "B_3_KH_Groesse(Betten)_CMI.csv";
            var headline = "BettenStufe;BereichName;Anzahl KH;Anteil KH;StufeSort;BereichSortName";
            var all = "";
            var level = "";
            using (var reader = CreateReaderWithoutHeadline(allFile)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    line = line.Insert(line.LastIndexOf(";", StringComparison.Ordinal), ";alle");
                    all += "0;" + line + Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(levelFile)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    line = ReplaceFirst(line, "bis 49 Betten", "1");
                    line = ReplaceFirst(line, "50 bis 99 Betten", "2");
                    line = ReplaceFirst(line, "100 bis 149 Betten", "3");
                    line = ReplaceFirst(line, "150 bis 199 Betten", "4");
                    line = ReplaceFirst(line, "200 bis 249 Betten", "5");
                    line = ReplaceFirst(line, "250 bis 299 Betten", "6");
                    line = ReplaceFirst(line, "300 bis 399 Betten", "7");
                    line = ReplaceFirst(line, "400 bis 499 Betten", "8");
                    line = ReplaceFirst(line, "500 bis 599 Betten", "9");
                    line = ReplaceFirst(line, "600 bis 799 Betten", "10");
                    line = ReplaceFirst(line, "800 bis 999 Betten", "11");
                    line = ReplaceFirst(line, "1000 Betten und mehr", "12");
                    level += line + Environment.NewLine;
                }
            }
            using (var writer = CreateWriter(levelFile)) {
                writer.WriteLine(headline);
                writer.Write(all);
                writer.Write(level);
            }
            PrintCreateFileSuccessfull(levelFile);
        }

        /*
            C_121_221_Bundesland_Groesse(Betten)_FZ_VWD_CMI.csv
        */

        private static void BuildStateSizeFzVwdCmi() {
            var outFileName = "C_121_221_Bundesland_Groesse(Betten)_FZ_VWD_CMI.csv";
            var c121allFile = "C_121_HA_Groesse(Betten)_FZ_VWD_CMI.csv"; // Type 1
            var c121stateFile = "C_121_HA_Bundesland_Groesse(Betten)_FZ_VWD_CMI.csv"; // Type 1
            var c221allFile = "C_221_BA_Groesse(Betten)_FZ_VWD_CMI.csv"; //Type 2
            var c221stateFile = "C_221_BA_Bundesland_Groesse(Betten)_FZ_VWD_CMI.csv"; // Type 2
            var headline = "Type;Bundesland_A;Stufe;Anzahl KH;mwFallzahlA;mwVWD;cmi;antSTF;BLCODE;BettenstufeSort";
            string c121AllContent = "", c121StateContent = "", c221AllContent = "", c221StateContent = "";
            using (var reader = CreateReaderWithoutHeadline(c121allFile)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    line = "1;00_Deutschland;" + line + Environment.NewLine;
                    line = line.Insert(line.LastIndexOf(";", StringComparison.Ordinal), ";00");
                    c121AllContent += line;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(c221allFile)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    line = "2;00_Deutschland;" + line + Environment.NewLine;
                    line = line.Insert(line.LastIndexOf(";", StringComparison.Ordinal), ";00");
                    c221AllContent += line;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(c121stateFile)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    c121StateContent += "1;" + line + Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(c221stateFile)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    c221StateContent += "2;" + line + Environment.NewLine;
                }
            }
            using (var writer = CreateWriter(outFileName)) {
                writer.WriteLine(headline);
                writer.Write(c121AllContent);
                writer.Write(c221AllContent);
                writer.Write(c121StateContent);
                writer.Write(c221StateContent);
            }
            PrintCreateFileSuccessfull(outFileName);
        }

        /*
            C_121_221_sum.csv
        */
        private static void BuildStateSizeFzVwdCmiSum() {
            var outFileName = "C_121_221_sum.csv";
            var c121Sum = "C_121_HA_Zusf.csv"; // Type 1
            var c221Sum = "C_221_BA_Zusf.csv"; // Type 2
            var c121SumState = "C_121_HA_Bundesland_FZ_VWD_CMI.csv"; // Type 1
            var c221SumState = "C_221_BA_Bundesland_FZ_VWD_CMI.csv"; // Type 2
            var headline = "Type;Gesamt;Anzahl KH;mwFallzahlHA;mwVWDkh;mwCMIkh;antSTF;BLCode";
            string c121SumContent = "", c121StateContent = "", c221SumContent = "", c221StateContent = "";
            using (var reader = CreateReaderWithoutHeadline(c121Sum)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    line = "1;00_" + line + ";00" + Environment.NewLine;
                    c121SumContent += line;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(c221Sum)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    line = "2;00_" + line + ";00" + Environment.NewLine;
                    c221SumContent += line;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(c121SumState)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    c121StateContent += "1;"+line + Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(c221SumState)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    c221StateContent += "2;"+line + Environment.NewLine;
                }
            }
            using (var writer = CreateWriter(outFileName)) {
                writer.WriteLine(headline);
                writer.Write(c121SumContent);
                writer.Write(c221SumContent);
                writer.Write(c121StateContent);
                writer.Write(c221StateContent);
            }
            PrintCreateFileSuccessfull(outFileName);
        }

        /*
            C_111_211.csv
        */
        private static void BuildMdcGender() {
            var outFileName = "C_111_211.csv";
            var c111MdcGender = "C_111_HA_MDC_Geschl.csv";
            var c211MdcGender = "C_211_BA_MDC_Geschl.csv";
            var headline = "Type;mdc;Name;SummeA;SummeAw;SummeAm;SummeAu;Anteilw;Anteilm;Anteilu";
            string c111Content = "", c211Content = "";
            using (var reader = CreateReaderWithoutHeadline(c111MdcGender)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    c111Content += "1;" + line + Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(c211MdcGender)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    c211Content += "2;" + line + Environment.NewLine;
                }
            }
            using (var writer = CreateWriter(outFileName)) {
                writer.WriteLine(headline);
                writer.Write(c111Content);
                writer.Write(c211Content);
            }
            PrintCreateFileSuccessfull(outFileName);
        }

        /*
            C_112_212.csv
        */

        private static void BuildAgeClasses() {
            var outFileName = "C_112_212.csv";
            var c112FileName = "C_112_HA_Alterskl.csv";
            var c212FileName = "C_212_BA_Alterskl.csv";
            var headline = "Type;Stufe;SummeA;SummeAw;SummeAm;SummeAu;Anteilw;Anteilm;Anteilu;SSTUFE";
            string c112Content = "", c212Content = "";
            using (var reader = CreateReaderWithoutHeadline(c112FileName)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    c112Content += "1;" + line + Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(c212FileName)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    c212Content += "2;" + line + Environment.NewLine;
                }
            }
            using (var writer = CreateWriter(outFileName)) {
                writer.WriteLine(headline);
                writer.Write(c112Content);
                writer.Write(c212Content);
            }
            PrintCreateFileSuccessfull(outFileName);
        }

        /*
            C_113_213.csv
        */
        private static void BuildDrg() {
            var outFileName = "C_113_213.csv";
            var c113FileName = "C_113_HA_DRG.csv";
            var c213FileName = "C_213_BA_DRG.csv";
            var headline = "Type;drg;name;AnzahlA;mwVwd;vwdStd;AnzKLA;AntKLA;AnzLLA;AntLLA";
            string c113Content = "", c213Content = "";
            using (var reader = CreateReaderWithoutHeadline(c113FileName)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    c113Content += "1;" + line.Replace("NULL", "0") + Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(c213FileName)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    c213Content += "2;" + line.Replace("NULL", "0") + Environment.NewLine;
                }
            }
            using (var writer = CreateWriter(outFileName)) {
                writer.WriteLine(headline);
                writer.Write(c113Content);
                writer.Write(c213Content);
            }
            PrintCreateFileSuccessfull(outFileName);
        }
    }
}