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
            BuildC122_C222(); // C_122_222.csv
            BuildNumOperations(); // C_123_223.csv
            BuildD(); // D.csv
            BuildC115sum(); // C_115_sum.csv
            BuildC215sum(); // C_215_sum.csv
            BuildC_1_2abc(); // C_1_2abc.csv
            BuildC114sum(); // C_114_sum.csv
            BuildC214sum(); // C_214_sum.csv
            BuildA2DataQuality(); // A_2_Datenqualitaet.csv
            BuildA3UnspecificCoding(); // A_3_Unspezif_Kodierung.csv
            BuildA1KH(); // A_1_KH.csv
            BuildA1KHsum(); // A_1_sum.csv
            BuildE(); // E.csv
            BuildD1sum(); // D_1_sum.csv
            BuildD2sum(); // D_2_sum.csv
            BuildE1Asum(); // E_1a_sum.csv
            BuildE2Asum(); // E_2a_sum.csv
            BuildE3Asum(); // E_3a_sum.csv
            BuildE1Bsum(); // E_1b_sum.csv
            BuildE2Bsum(); // E_2b_sum.csv
            BuildE3Bsum(); // E_3b_sum.csv
            BuildPercentageHABA(); // Proz_HA_BA.csv
            BuildC111sum(); // C_111_sum.csv
            BuildC211sum(); // C_211_sum.csv
            BuildC112sum(); // C_112_sum.csv
            BuildC212sum(); // C_212_sum.csv
            BuildC113sum(); // C_113_sum.csv
            BuildC213sum(); // C_213_sum.csv
        }

        private static StreamReader CreateReaderWithoutHeadline(string fileName) {
            var reader = new StreamReader(Path.Combine(Program.FileDir, Program.FilePrefix + fileName));
            reader.ReadLine();
            return reader;
        }

        private static StreamWriter CreateWriter(string fileName) {
            return
                new StreamWriter(
                    new FileStream(Path.Combine(Program.BROWSER_EXPORT_DATA_DIR, Program.DataYear + "", fileName),
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

        private static int IndexOfNth(this string input,
                             string value, int startIndex, int nth) {
            if (nth < 1)
                throw new NotSupportedException("Param 'nth' must be greater than 0!");
            if (nth == 1)
                return input.IndexOf(value, startIndex);
            var idx = input.IndexOf(value, startIndex);
            if (idx == -1)
                return -1;
            return input.IndexOfNth(value, idx + 1, --nth);
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
                    line = line.Insert(line.LastIndexOf(";"), ";" + "00");
                    germany += "00_Deutschland;" + line.Replace(",", ".") + Environment.NewLine;
                }
            }
            using (
                var reader = CreateReaderWithoutHeadline(stateFile)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    states += line.Replace(",", ".") + Environment.NewLine;
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
                    all += "Alle;" + line.Replace(",", ".") + Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(providerFile)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    provider += line.Replace(",", ".") + Environment.NewLine;
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
                    level += line.Replace(",", ".") + Environment.NewLine;
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
                    c121AllContent += line.Replace(",", ".");
                }
            }
            using (var reader = CreateReaderWithoutHeadline(c221allFile)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    line = "2;00_Deutschland;" + line + Environment.NewLine;
                    line = line.Insert(line.LastIndexOf(";", StringComparison.Ordinal), ";00");
                    c221AllContent += line.Replace(",", ".");
                }
            }
            using (var reader = CreateReaderWithoutHeadline(c121stateFile)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    c121StateContent += "1;" + line.Replace(",", ".") + Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(c221stateFile)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    c221StateContent += "2;" + line.Replace(",", ".") + Environment.NewLine;
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
                    c121SumContent += line.Replace(",", ".");
                }
            }
            using (var reader = CreateReaderWithoutHeadline(c221Sum)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    line = "2;00_" + line + ";00" + Environment.NewLine;
                    c221SumContent += line.Replace(",", ".");
                }
            }
            using (var reader = CreateReaderWithoutHeadline(c121SumState)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    c121StateContent += "1;"+line.Replace(",", ".") + Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(c221SumState)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    c221StateContent += "2;"+line.Replace(",", ".") + Environment.NewLine;
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
                    string[] lineSplit = line.Split(';');
                    for (int i = 0; i < lineSplit.Length; i++) {
                        if (i == 1)
                            continue;
                        lineSplit[i] = lineSplit[i].Replace(",", ".");
                    }
                    line = string.Join(";", lineSplit);
                    c111Content += "1;" + line + Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(c211MdcGender)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    string[] lineSplit = line.Split(';');
                    for (int i = 0; i < lineSplit.Length; i++) {
                        if (i == 1)
                            continue;
                        lineSplit[i] = lineSplit[i].Replace(",", ".");
                    }
                    line = string.Join(";", lineSplit);
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
                    c112Content += "1;" + line.Replace(",", ".") + Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(c212FileName)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    c212Content += "2;" + line.Replace(",", ".") + Environment.NewLine;
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
                    var lineSplit = line.Split(';');
                    for (int i = 0; i < lineSplit.Length; i++) {
                        if (i == 1)
                            continue;
                        lineSplit[i] = lineSplit[i].Replace("NULL", "0").Replace(",", ".");
                    }
                    line = string.Join(";", lineSplit);
                    c113Content += "1;" + line+ Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(c213FileName)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    var lineSplit = line.Split(';');
                    for (int i = 0; i < lineSplit.Length; i++) {
                        if (i == 1)
                            continue;
                        lineSplit[i] = lineSplit[i].Replace("NULL", "0").Replace(",", ".");
                    }
                    line = string.Join(";", lineSplit);
                    c213Content += "2;" + line + Environment.NewLine;
                }
            }
            using (var writer = CreateWriter(outFileName)) {
                writer.WriteLine(headline);
                writer.Write(c113Content);
                writer.Write(c213Content);
            }
            PrintCreateFileSuccessfull(outFileName);
        }

        private static void BuildC122_C222() {
            var outFileName = "C_122_222.csv";
            var headline = "Type;Type2;AufAnlName;Anzahl;sortnr";
            var c122HAAFileName = "C_122_HA_Aufnahme.csv";
            var c122HAEFileName = "C_122_HA_Entlassung.csv";
            var c222BAAFileName = "C_222_BA_Aufnahme.csv";
            var c222BAEFileName = "C_222_BA_Entlassung.csv";
            string content = "";
            using (var reader = CreateReaderWithoutHeadline(c122HAAFileName)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    var lineSplit = line.Split(';');
                    for (int i = 0; i < lineSplit.Length; i++) {
                        if (i == 0)
                            continue;
                        lineSplit[i] = lineSplit[i].Replace("NULL", "0").Replace(",", ".");
                    }
                    line = string.Join(";", lineSplit);
                    content += "1;A;" + line + Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(c122HAEFileName)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    var lineSplit = line.Split(';');
                    for (int i = 0; i < lineSplit.Length; i++) {
                        if (i == 0)
                            continue;
                        lineSplit[i] = lineSplit[i].Replace("NULL", "0").Replace(",", ".");
                    }
                    line = string.Join(";", lineSplit);
                    content += "1;E;" + line + Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(c222BAAFileName)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    var lineSplit = line.Split(';');
                    for (int i = 0; i < lineSplit.Length; i++) {
                        if (i == 0)
                            continue;
                        lineSplit[i] = lineSplit[i].Replace("NULL", "0").Replace(",", ".");
                    }
                    line = string.Join(";", lineSplit);
                    content += "2;A;" + line + Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(c222BAEFileName)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    var lineSplit = line.Split(';');
                    for (int i = 0; i < lineSplit.Length; i++) {
                        if (i == 0)
                            continue;
                        lineSplit[i] = lineSplit[i].Replace("NULL", "0").Replace(",", ".");
                    }
                    line = string.Join(";", lineSplit);
                    content += "2;E;" + line + Environment.NewLine;
                }
            }
            using (var writer = CreateWriter(outFileName)) {
                writer.WriteLine(headline);
                writer.Write(content);
            }
            PrintCreateFileSuccessfull(outFileName);
        }

        private static void BuildNumOperations() {
            var outFileName = "C_123_223.csv";
            var headline = "Type;viersteller;vierstellername;AnzahlA;Anzahl KH;mwNennungenKH;antKHTop10Prozent;antKHTop20Prozent;antKHTop30Prozent;antKHTop40Prozent;antKHTop50Prozent";
            var c123HAFileName = "C_123_HA_Haeufig_OP.csv";
            var c223BAFileName = "C_223_BA_Haeufig_OP.csv";
            string content = "";
            using (var reader = CreateReaderWithoutHeadline(c123HAFileName)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    var lineSplit = line.Split(';');
                    for (int i = 0; i < lineSplit.Length; i++) {
                        if (i == 1)
                            continue;
                        lineSplit[i] = lineSplit[i].Replace("NULL", "0").Replace(",", ".");
                    }
                    line = string.Join(";", lineSplit);
                    content += "1;" + line + Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(c223BAFileName)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    var lineSplit = line.Split(';');
                    for (int i = 0; i < lineSplit.Length; i++) {
                        if (i == 1)
                            continue;
                        lineSplit[i] = lineSplit[i].Replace("NULL", "0").Replace(",", ".");
                    }
                    line = string.Join(";", lineSplit);
                    content += "2;" + line + Environment.NewLine;
                }
            }
            using (var writer = CreateWriter(outFileName)) {
                writer.WriteLine(headline);
                writer.Write(content);
            }
            PrintCreateFileSuccessfull(outFileName);
        }

        private static void BuildD() {
            var outFileName = "D.csv";
            var headline = "Type;Bereich;Bezeichnung;Anzahl KH;AnzahlTS";
            var d1FileName = "D_1a_TS_HD_Kapitel.csv";
            var d2FileName = "D_1b_TS_HD_Gruppe.csv";
            var d3FileName = "D_1c_TS_HD_Kategorie.csv";
            var d4FileName = "D_2a_TS_Proz_Kapitel.csv";
            var d5FileName = "D_2b_TS_Proz_Bereich.csv";
            var content = "";
            using (var reader = CreateReaderWithoutHeadline(d1FileName)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    var lineSplit = line.Split(';');
                    for (int i = 0; i < lineSplit.Length; i++) {
                        if (i == 1)
                            continue;
                        lineSplit[i] = lineSplit[i].Replace("NULL", "0").Replace(",", ".");
                    }
                    line = string.Join(";", lineSplit);
                    content += "1;" + line + Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(d2FileName)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    var lineSplit = line.Split(';');
                    for (int i = 0; i < lineSplit.Length; i++) {
                        if (i == 1)
                            continue;
                        lineSplit[i] = lineSplit[i].Replace("NULL", "0").Replace(",", ".");
                    }
                    line = string.Join(";", lineSplit);
                    content += "2;" + line + Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(d3FileName)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    var lineSplit = line.Split(';');
                    for (int i = 0; i < lineSplit.Length; i++) {
                        if (i == 1)
                            continue;
                        lineSplit[i] = lineSplit[i].Replace("NULL", "0").Replace(",", ".");
                    }
                    line = string.Join(";", lineSplit);
                    content += "3;" + line + Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(d4FileName)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    var lineSplit = line.Split(';');
                    for (int i = 0; i < lineSplit.Length; i++) {
                        if (i == 1)
                            continue;
                        lineSplit[i] = lineSplit[i].Replace("NULL", "0").Replace(",", ".");
                    }
                    line = string.Join(";", lineSplit);
                    content += "4;" + line+ Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(d5FileName)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    var lineSplit = line.Split(';');
                    for (int i = 0; i < lineSplit.Length; i++) {
                        if (i == 1)
                            continue;
                        lineSplit[i] = lineSplit[i].Replace("NULL", "0").Replace(",", ".");
                    }
                    line = string.Join(";", lineSplit);
                    content += "5;" + line + Environment.NewLine;
                }
            }
            using (var writer = CreateWriter(outFileName)) {
                writer.WriteLine(headline);
                writer.Write(content);
            }
            PrintCreateFileSuccessfull(outFileName);
        }

        private static void BuildC115sum() {
            var outFileName = "C_115_sum.csv";
            var headline = "Anzahl HA;Summe00J;Summe0105J;Summe0510J;Summe1015J;Summe1520J;Summe2025J;Summe2530J;Summe3035J;Summe3540J;Summe4045J;Summe4550J;Summe5055J;Summe5560J;Summe6065J;Summe6570J;Summe7075J;Summe7580J;Summe8085J;Summe8590J;Summe90ffJ;Ant00Jges;Ant0105Jges;Ant0510Jges;Ant1015Jges;Ant1520Jges;Ant2025Jges;Ant2530Jges;Ant3035Jges;Ant3540Jges;Ant4045Jges;Ant4550Jges;Ant5055Jges;Ant5560Jges;Ant6065Jges;Ant6570Jges;Ant7075Jges;Ant7580Jges;Ant8085Jges;Ant8590Jges;Ant90ffJges";
            var c115sum = "C_115_Zusf.csv";

            var content = "";
            using (var reader = CreateReaderWithoutHeadline(c115sum)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    content += line.Replace("NULL", "0").Replace(",", ".") + Environment.NewLine;
                }
            }
            using (var writer = CreateWriter(outFileName)) {
                writer.WriteLine(headline);
                writer.Write(content);
            }
            PrintCreateFileSuccessfull(outFileName);
        }

        private static void BuildC215sum() {
            var outFileName = "C_215_sum.csv";
            var headline = "Anzahl BA;Summe00J;Summe0105J;Summe0510J;Summe1015J;Summe1520J;Summe2025J;Summe2530J;Summe3035J;Summe3540J;Summe4045J;Summe4550J;Summe5055J;Summe5560J;Summe6065J;Summe6570J;Summe7075J;Summe7580J;Summe8085J;Summe8590J;Summe90ffJ;Ant00Jges;Ant0105Jges;Ant0510Jges;Ant1015Jges;Ant1520Jges;Ant2025Jges;Ant2530Jges;Ant3035Jges;Ant3540Jges;Ant4045Jges;Ant4550Jges;Ant5055Jges;Ant5560Jges;Ant6065Jges;Ant6570Jges;Ant7075Jges;Ant7580Jges;Ant8085Jges;Ant8590Jges;Ant90ffJges";
            var c215sum = "C_215_Zusf.csv";
            var content = "";
            using (var reader = CreateReaderWithoutHeadline(c215sum)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    content += line.Replace("NULL", "0").Replace(",", ".") + Environment.NewLine;
                }
            }
            using (var writer = CreateWriter(outFileName)) {
                writer.WriteLine(headline);
                writer.Write(content);
            }
            PrintCreateFileSuccessfull(outFileName);
        }

        private static void BuildC_1_2abc() {
            var outFileName = "C_1_2abc.csv";
            var headline =
                "Type;Type2;Bereich;Bereichsname;Anzahl;VwdMw;stdVwd;unter 1 Jahr;1 bis unter 5 Jahre;5 bis unter 10 Jahre;10 bis unter 15 Jahre;15 bis unter 20 Jahre;20 bis unter 25 Jahre;25 bis unter 30 Jahre;30 bis unter 35 Jahre;35 bis unter 40 Jahre;40 bis unter 45 Jahre;45 bis unter 50 Jahre;50 bis unter 55 Jahre;55 bis unter 60 Jahre;60 bis unter 65 Jahre;65 bis unter 70 Jahre;70 bis unter 75 Jahre;75 bis unter 80 Jahre;80 bis unter 85 Jahre;85 bis unter 90 Jahre;90 Jahre und älter;Ant00J;Ant0105J;Ant0510J;Ant1015J;Ant1520J;Ant2025J;Ant2530J;Ant3035J;Ant3540J;Ant4045J;Ant4550J;Ant5055J;Ant5560J;Ant6065J;Ant6570J;Ant7075J;Ant7580J;Ant8085J;Ant8590J;Ant90ffJ";
            var c114a = "C_114a_HA_HD_Kapitel.csv"; // Types: 1 1
            var c114b = "C_114b_HA_HD_Gruppe.csv"; // Types: 2 1
            var c114c = "C_114c_HA_HD_Kategorie.csv"; // Types: 3 1
            var c115a = "C_115a_HA_Proz_Kapitel.csv"; // Types: 1 2
            var c115b = "C_115b_HA_Proz_Bereich.csv"; // Types: 2 2
            var c115c = "C_115c_HA_Proz_4Steller.csv"; // Types: 3 2
            var c214a = "C_214a_BA_HD_Kapitel.csv"; // Types: 1 3
            var c214b = "C_214b_BA_HD_Gruppe.csv"; // Types: 2 3
            var c214c = "C_214c_BA_HD_Kategorie.csv"; // Types: 3 3
            var c215a = "C_215a_BA_Proz_Kapitel.csv"; // Types: 1 4
            var c215b = "C_215b_BA_Proz_Bereich.csv"; // Types: 2 4
            var c215c = "C_215c_BA_Proz_4Steller.csv"; // Types: 3 4
            var content = "";
            using (var reader = CreateReaderWithoutHeadline(c114a)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    line = line.Replace(";0,00;", ";0;");
                    line = line.Replace(";0.00;", ";0;");
                    var lineSplit = line.Split(';');
                    for (int i = 0; i < lineSplit.Length; i++) {
                        if (i == 1)
                            continue;
                        lineSplit[i] = lineSplit[i].Replace("NULL", "0").Replace(",", ".");
                    }
                    line = string.Join(";", lineSplit);
                    content += "1;1;" + line + Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(c114b)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    line = line.Replace(";0,00;", ";0;");
                    line = line.Replace(";0.00;", ";0;");
                    var lineSplit = line.Split(';');
                    for (int i = 0; i < lineSplit.Length; i++) {
                        if (i == 1)
                            continue;
                        lineSplit[i] = lineSplit[i].Replace("NULL", "0").Replace(",", ".");
                    }
                    line = string.Join(";", lineSplit);
                    content += "2;1;" + line + Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(c114c)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    line = line.Replace(";0,00;", ";0;");
                    line = line.Replace(";0.00;", ";0;");
                    var lineSplit = line.Split(';');
                    for (int i = 0; i < lineSplit.Length; i++) {
                        if (i == 1)
                            continue;
                        lineSplit[i] = lineSplit[i].Replace("NULL", "0").Replace(",", ".");
                    }
                    line = string.Join(";", lineSplit);
                    content += "3;1;" + line + Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(c115a)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    int index = IndexOfNth(line, ";", 0, 3);
                    line = line.Insert(index, ";0;0");
                    line = line.Replace(";0,00;", ";0;");
                    line = line.Replace(";0.00;", ";0;");
                    content += "1;2;" + line + Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(c115b)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    int index = IndexOfNth(line, ";", 0, 3);
                    line = line.Insert(index, ";0;0");
                    line = line.Replace(";0,00;", ";0;");
                    line = line.Replace(";0.00;", ";0;");
                    var lineSplit = line.Split(';');
                    for (int i = 0; i < lineSplit.Length; i++) {
                        if (i == 1)
                            continue;
                        lineSplit[i] = lineSplit[i].Replace("NULL", "0").Replace(",", ".");
                    }
                    line = string.Join(";", lineSplit);
                    content += "2;2;" + line + Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(c115c)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    int index = IndexOfNth(line, ";", 0, 3);
                    line = line.Insert(index, ";0;0");
                    line = line.Replace(";0,00;", ";0;");
                    line = line.Replace(";0.00;", ";0;");
                    var lineSplit = line.Split(';');
                    for (int i = 0; i < lineSplit.Length; i++) {
                        if (i == 1)
                            continue;
                        lineSplit[i] = lineSplit[i].Replace("NULL", "0").Replace(",", ".");
                    }
                    line = string.Join(";", lineSplit);
                    content += "3;2;" + line + Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(c214a)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    line = line.Replace(";0,00;", ";0;");
                    line = line.Replace(";0.00;", ";0;");
                    var lineSplit = line.Split(';');
                    for (int i = 0; i < lineSplit.Length; i++) {
                        if (i == 1)
                            continue;
                        lineSplit[i] = lineSplit[i].Replace("NULL", "0").Replace(",", ".");
                    }
                    line = string.Join(";", lineSplit);
                    content += "1;3;" + line + Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(c214b)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    line = line.Replace(";0,00;", ";0;");
                    line = line.Replace(";0.00;", ";0;");
                    var lineSplit = line.Split(';');
                    for (int i = 0; i < lineSplit.Length; i++) {
                        if (i == 1)
                            continue;
                        lineSplit[i] = lineSplit[i].Replace("NULL", "0").Replace(",", ".");
                    }
                    line = string.Join(";", lineSplit);
                    content += "2;3;" + line + Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(c214c)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    line = line.Replace("0,00", "0");
                    line = line.Replace("0.00", "0");
                    var lineSplit = line.Split(';');
                    for (int i = 0; i < lineSplit.Length; i++) {
                        if (i == 1)
                            continue;
                        lineSplit[i] = lineSplit[i].Replace("NULL", "0").Replace(",", ".");
                    }
                    line = string.Join(";", lineSplit);
                    content += "3;3;" + line + Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(c215a)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    int index = IndexOfNth(line, ";", 0, 3);
                    line = line.Insert(index, ";0;0");
                    line = line.Replace(";0,00;", ";0;");
                    line = line.Replace(";0.00;", ";0;");
                    var lineSplit = line.Split(';');
                    for (int i = 0; i < lineSplit.Length; i++) {
                        if (i == 1)
                            continue;
                        lineSplit[i] = lineSplit[i].Replace("NULL", "0").Replace(",", ".");
                    }
                    line = string.Join(";", lineSplit);
                    content += "1;4;" + line + Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(c215b)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    int index = IndexOfNth(line, ";", 0, 3);
                    line = line.Insert(index, ";0;0");
                    line = line.Replace(";0,00;", ";0;");
                    line = line.Replace(";0.00;", ";0;");
                    var lineSplit = line.Split(';');
                    for (int i = 0; i < lineSplit.Length; i++) {
                        if (i == 1)
                            continue;
                        lineSplit[i] = lineSplit[i].Replace("NULL", "0").Replace(",", ".");
                    }
                    line = string.Join(";", lineSplit);
                    content += "2;4;" + line + Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(c215c)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    int index = IndexOfNth(line, ";", 0, 3);
                    line = line.Insert(index, ";0;0");
                    line = line.Replace(";0,00;", ";0;");
                    line = line.Replace(";0.00;", ";0;");
                    var lineSplit = line.Split(';');
                    for (int i = 0; i < lineSplit.Length; i++) {
                        if (i == 1)
                            continue;
                        lineSplit[i] = lineSplit[i].Replace("NULL", "0").Replace(",", ".");
                    }
                    line = string.Join(";", lineSplit);
                    content += "3;4;" + line + Environment.NewLine;
                }
            }
            using (var writer = CreateWriter(outFileName)) {
                writer.WriteLine(headline);
                writer.Write(content);
            }
            PrintCreateFileSuccessfull(outFileName);
        }

        private static void BuildC114sum() {
            var outFileName = "C_114_sum.csv";
            var headline = "AnzahlHAges;VwdMwGes;stdVwdGes;Summe00J;Summe0105J;Summe0510J;Summe1015J;Summe1520J;Summe2025J;Summe2530J;Summe3035J;Summe3540J;Summe4045J;Summe4550J;Summe5055J;Summe5560J;Summe6065J;Summe6570J;Summe7075J;Summe7580J;Summe8085J;Summe8590J;Summe90ffJ;Ant00Jges;Ant0105Jges;Ant0510Jges;Ant1015Jges;Ant1520Jges;Ant2025Jges;Ant2530Jges;Ant3035Jges;Ant3540Jges;Ant4045Jges;Ant4550Jges;Ant5055Jges;Ant5560Jges;Ant6065Jges;Ant6570Jges;Ant7075Jges;Ant7580Jges;Ant8085Jges;Ant8590Jges;Ant90ffJges";
            var c114sum = "C_114_Zusf.csv";
            var content = "";
            using (var reader = CreateReaderWithoutHeadline(c114sum)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    content += line.Replace("NULL", "0").Replace(",", ".") + Environment.NewLine;
                }
            }
            using (var writer = CreateWriter(outFileName)) {
                writer.WriteLine(headline);
                writer.Write(content);
            }
            PrintCreateFileSuccessfull(outFileName);
        }

        private static void BuildC214sum() {
            var outFileName = "C_214_sum.csv";
            var headline =
                "AnzahlBAges;VwdMwGes;stdVwdGes;Summe00J;Summe0105J;Summe0510J;Summe1015J;Summe1520J;Summe2025J;Summe2530J;Summe3035J;Summe3540J;Summe4045J;Summe4550J;Summe5055J;Summe5560J;Summe6065J;Summe6570J;Summe7075J;Summe7580J;Summe8085J;Summe8590J;Summe90ffJ;Ant00Jges;Ant0105Jges;Ant0510Jges;Ant1015Jges;Ant1520Jges;Ant2025Jges;Ant2530Jges;Ant3035Jges;Ant3540Jges;Ant4045Jges;Ant4550Jges;Ant5055Jges;Ant5560Jges;Ant6065Jges;Ant6570Jges;Ant7075Jges;Ant7580Jges;Ant8085Jges;Ant8590Jges;Ant90ffJges";
            var c214sum = "C_214_Zusf.csv";
            var content = "";
            using (var reader = CreateReaderWithoutHeadline(c214sum)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    content += line.Replace("NULL", "0").Replace(",", ".") + Environment.NewLine;
                }
            }
            using (var writer = CreateWriter(outFileName)) {
                writer.WriteLine(headline);
                writer.Write(content);
            }
            PrintCreateFileSuccessfull(outFileName);
        }

        private static void BuildA2DataQuality() {
            var outFileName = "A_2_Datenqualitaet.csv";
            var headline = "Art;Kennzahl;Anteil";
            var a2 = "A_2_Datenqualitaet.csv";
            var content = "";
            using (var reader = CreateReaderWithoutHeadline(a2)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    content += line.Replace("NULL", "0").Replace(",", ".") + Environment.NewLine;
                }
            }
            using (var writer = CreateWriter(outFileName)) {
                writer.WriteLine(headline);
                writer.Write(content);
            }
            PrintCreateFileSuccessfull(outFileName);
        }

        private static void BuildA3UnspecificCoding() {
            var outFileName = "A_3_Unspezif_Kodierung.csv";
            var headline = "Art;AnzNDGes;AnzNDGesU;AnteilNDU;AnzProzGes;AnzProzGesU;AnteilProzU";
            var a3 = "A_3_Unspezif_Kodierung.csv";
            var content = "";
            using (var reader = CreateReaderWithoutHeadline(a3)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    content += line.Replace("NULL", "0").Replace(",", ".") + Environment.NewLine;
                }
            }
            using (var writer = CreateWriter(outFileName)) {
                writer.WriteLine(headline);
                writer.Write(content);
            }
            PrintCreateFileSuccessfull(outFileName);
        }

        private static void BuildA1KH() {
            var outFileName = "A_1_KH.csv";
            var headline = "Bundesland_A;AnzahlKH;AnzahlFall;BLCode";
            var a1Kh = "A_1_KH.csv";
            var content = "";
            using (var reader = CreateReaderWithoutHeadline(a1Kh)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    content += line.Replace("NULL", "0").Replace(",", ".") + Environment.NewLine;
                }
            }
            using (var writer = CreateWriter(outFileName)) {
                writer.WriteLine(headline);
                writer.Write(content);
            }
            PrintCreateFileSuccessfull(outFileName);
        }

        private static void BuildA1KHsum() {
            var outFileName = "A_1_sum.csv";
            var headline = "Gesamt;AnzahlKHs;AnzahlFall";
            var a1sum = "A_1_KH_Zusf.csv";
            var content = "";
            using (var reader = CreateReaderWithoutHeadline(a1sum)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    content += line.Replace("NULL", "0").Replace(",", ".") + Environment.NewLine;
                }
            }
            using (var writer = CreateWriter(outFileName)) {
                writer.WriteLine(headline);
                writer.Write(content);
            }
            PrintCreateFileSuccessfull(outFileName);
        }

        private static void BuildE() {
            var outFileName = "E.csv";
            var headline = "Type;DRG;name;Relativgewicht;AnzahlHA;AntAnGesFallzahl";
            var e1a = "E_1a_HA_20_wenig_kompl.csv";
            var e1b = "E_1b_BA_20_wenig_kompl.csv";
            var e2a = "E_2a_HA_20_kompl.csv";
            var e2b = "E_2b_BA_20_kompl.csv";
            var e3a = "E_3a_HA_20_haeufig.csv";
            var e3b = "E_3b_BA_20_haeufig.csv";
            var content = "";
            using (var reader = CreateReaderWithoutHeadline(e1a)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    var lineSplit = line.Split(';');
                    for (int i = 0; i < lineSplit.Length; i++) {
                        if (i == 1)
                            continue;
                        lineSplit[i] = lineSplit[i].Replace("NULL", "0").Replace(",", ".");
                    }
                    line = string.Join(";", lineSplit);
                    content += "1;" + line + Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(e1b)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    var lineSplit = line.Split(';');
                    for (int i = 0; i < lineSplit.Length; i++) {
                        if (i == 1)
                            continue;
                        lineSplit[i] = lineSplit[i].Replace("NULL", "0").Replace(",", ".");
                    }
                    line = string.Join(";", lineSplit);
                    content += "2;" + line + Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(e2a)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    var lineSplit = line.Split(';');
                    for (int i = 0; i < lineSplit.Length; i++) {
                        if (i == 1)
                            continue;
                        lineSplit[i] = lineSplit[i].Replace("NULL", "0").Replace(",", ".");
                    }
                    line = string.Join(";", lineSplit);
                    content += "3;" + line + Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(e2b)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    var lineSplit = line.Split(';');
                    for (int i = 0; i < lineSplit.Length; i++) {
                        if (i == 1)
                            continue;
                        lineSplit[i] = lineSplit[i].Replace("NULL", "0").Replace(",", ".");
                    }
                    line = string.Join(";", lineSplit);
                    content += "4;" + line+ Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(e3a)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    var lineSplit = line.Split(';');
                    for (int i = 0; i < lineSplit.Length; i++) {
                        if (i == 1)
                            continue;
                        lineSplit[i] = lineSplit[i].Replace("NULL", "0").Replace(",", ".");
                    }
                    line = string.Join(";", lineSplit);
                    content += "5;" + line + Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(e3b)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    var lineSplit = line.Split(';');
                    for (int i = 0; i < lineSplit.Length; i++) {
                        if (i == 1)
                            continue;
                        lineSplit[i] = lineSplit[i].Replace("NULL", "0").Replace(",", ".");
                    }
                    line = string.Join(";", lineSplit);
                    content += "6;" + line + Environment.NewLine;
                }
            }
            using (var writer = CreateWriter(outFileName)) {
                writer.WriteLine(headline);
                writer.Write(content);
            }
            PrintCreateFileSuccessfull(outFileName);
        }

        private static void BuildD1sum() {
            var outFileName = "D_1_sum.csv";
            var headline = "Anzahl KH ges;AnzahlTSges";
            var d1sum = "D_1_TS_Zusf.csv";
            var content = "";
            using (var reader = CreateReaderWithoutHeadline(d1sum)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    content += line.Replace("NULL", "0").Replace(",", ".") + Environment.NewLine;
                }
            }
            using (var writer = CreateWriter(outFileName)) {
                writer.WriteLine(headline);
                writer.Write(content);
            }
            PrintCreateFileSuccessfull(outFileName);
        }
        private static void BuildD2sum() {
            var outFileName = "D_2_sum.csv";
            var headline = "Anzahl KH ges;AnzahlTSges";
            var d2sum = "D_2_TS_Zusf.csv";
            var content = "";
            using (var reader = CreateReaderWithoutHeadline(d2sum)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    content += line.Replace("NULL", "0").Replace(",", ".") + Environment.NewLine;
                }
            }
            using (var writer = CreateWriter(outFileName)) {
                writer.WriteLine(headline);
                writer.Write(content);
            }
            PrintCreateFileSuccessfull(outFileName);
        }

        private static void BuildE1Asum() {
            var outFileName = "E_1a_sum.csv";
            var headline = "AnzahlHAges;AntAnGesFallzahlGes";
            var e1a = "E_1a_Zusf.csv";
            var content = "";
            using (var reader = CreateReaderWithoutHeadline(e1a)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    content += line.Replace("NULL", "0").Replace(",", ".") + Environment.NewLine;
                }
            }
            using (var writer = CreateWriter(outFileName)) {
                writer.WriteLine(headline);
                writer.Write(content);
            }
            PrintCreateFileSuccessfull(outFileName);
        }

        private static void BuildE2Asum() {
            var outFileName = "E_2a_sum.csv";
            var headline = "AnzahlHAges;AntAnGesFallzahlGes";
            var e2a = "E_2a_Zusf.csv";
            var content = "";
            using (var reader = CreateReaderWithoutHeadline(e2a)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    content += line.Replace("NULL", "0").Replace(",", ".") + Environment.NewLine;
                }
            }
            using (var writer = CreateWriter(outFileName)) {
                writer.WriteLine(headline);
                writer.Write(content);
            }
            PrintCreateFileSuccessfull(outFileName);
        }

        private static void BuildE3Asum() {
            var outFileName = "E_3a_sum.csv";
            var headline = "AnzahlHAges;AntAnGesFallzahlGes";
            var e3a = "E_3a_Zusf.csv";
            var content = "";
            using (var reader = CreateReaderWithoutHeadline(e3a)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    content += line.Replace("NULL", "0").Replace(",", ".") + Environment.NewLine;
                }
            }
            using (var writer = CreateWriter(outFileName)) {
                writer.WriteLine(headline);
                writer.Write(content);
            }
            PrintCreateFileSuccessfull(outFileName);
        }

        private static void BuildE1Bsum() {
            var outFileName = "E_1b_sum.csv";
            var headline = "AnzahlBAges;AntAnGesFallzahlGes";
            var e1b = "E_1b_Zusf.csv";
            var content = "";
            using (var reader = CreateReaderWithoutHeadline(e1b)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    content += line.Replace("NULL", "0").Replace(",", ".") + Environment.NewLine;
                }
            }
            using (var writer = CreateWriter(outFileName)) {
                writer.WriteLine(headline);
                writer.Write(content);
            }
            PrintCreateFileSuccessfull(outFileName);
        }

        private static void BuildE2Bsum() {
            var outFileName = "E_2b_sum.csv";
            var headline = "AnzahlBAges;AntAnGesFallzahlGes";
            var e2b = "E_2b_Zusf.csv";
            var content = "";
            using (var reader = CreateReaderWithoutHeadline(e2b)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    content += line.Replace("NULL", "0").Replace(",", ".") + Environment.NewLine;
                }
            }
            using (var writer = CreateWriter(outFileName)) {
                writer.WriteLine(headline);
                writer.Write(content);
            }
            PrintCreateFileSuccessfull(outFileName);
        }

        private static void BuildE3Bsum() {
            var outFileName = "E_3b_sum.csv";
            var headline = "AnzahlBAges;AntAnGesFallzahlGes";
            var e3b = "E_3b_Zusf.csv";
            var content = "";
            using (var reader = CreateReaderWithoutHeadline(e3b)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    content += line.Replace("NULL", "0").Replace(",", ".") + Environment.NewLine;
                }
            }
            using (var writer = CreateWriter(outFileName)) {
                writer.WriteLine(headline);
                writer.Write(content);
            }
            PrintCreateFileSuccessfull(outFileName);
        }

        private static void BuildPercentageHABA() {
            var outFileName = "Proz_HA_BA.csv";
            var headline = "Type;AnzProzProFall";
            var percHa = "C_115_HA_Proz_Fall.csv";
            var percBa = "C_215_BA_Proz_Fall.csv";
            var content = "";
            using (var reader = CreateReaderWithoutHeadline(percHa)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    content += "1;" + line.Replace("NULL", "0").Replace("0,", "0,0") + Environment.NewLine;
                }
            }
            using (var reader = CreateReaderWithoutHeadline(percBa)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    content += "2;" + line.Replace("NULL", "0").Replace("0,", "0,0") + Environment.NewLine;
                }
            }
            using (var writer = CreateWriter(outFileName)) {
                writer.WriteLine(headline);
                writer.Write(content);
            }
            PrintCreateFileSuccessfull(outFileName);
        }

        private static void BuildC111sum() {
            var outFileName = "C_111_sum.csv";
            var headline = "SummeHAges;SummeHAwGes;SummeHAmGes;SummeHAuGes;AnteilwGes;AnteilmGes;AnteiluGes";
            var c111sum = "C_111_Zusf.csv";
            var content = "";
            using (var reader = CreateReaderWithoutHeadline(c111sum)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    content += line.Replace("NULL", "0").Replace(",", ".") + Environment.NewLine;
                }
            }
            using (var writer = CreateWriter(outFileName)) {
                writer.WriteLine(headline);
                writer.Write(content);
            }
            PrintCreateFileSuccessfull(outFileName);
        }
        private static void BuildC211sum() {
            var outFileName = "C_211_sum.csv";
            var headline = "SummeBAges;SummeBAwGes;SummeBAmGes;SummeBAuGes;AnteilwGes;AnteilmGes;AnteiluGes";
            var cSum = "C_211_Zusf.csv";
            var content = "";
            using (var reader = CreateReaderWithoutHeadline(cSum)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    content += line.Replace("NULL", "0").Replace(",", ".") + Environment.NewLine;
                }
            }
            using (var writer = CreateWriter(outFileName)) {
                writer.WriteLine(headline);
                writer.Write(content);
            }
            PrintCreateFileSuccessfull(outFileName);
        }
        private static void BuildC112sum() {
            var outFileName = "C_112_sum.csv";
            var headline = "SummeHAges;SummeHAwGes;SummeHAmGes;SummeHAuGes;AnteilwGes;AnteilmGes;AnteiluGes";
            var cSum = "C_112_Zusf.csv";
            var content = "";
            using (var reader = CreateReaderWithoutHeadline(cSum)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    content += line.Replace("NULL", "0").Replace(",", ".") + Environment.NewLine;
                }
            }
            using (var writer = CreateWriter(outFileName)) {
                writer.WriteLine(headline);
                writer.Write(content);
            }
            PrintCreateFileSuccessfull(outFileName);
        }
        private static void BuildC212sum() {
            var outFileName = "C_212_sum.csv";
            var headline = "SummeBAges;SummeBAwGes;SummeBAmGes;SummeBAuGes;AnteilwGes;AnteilmGes;AnteiluGes";
            var cSum = "C_212_Zusf.csv";
            var content = "";
            using (var reader = CreateReaderWithoutHeadline(cSum)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    content += line.Replace("NULL", "0").Replace(",", ".") + Environment.NewLine;
                }
            }
            using (var writer = CreateWriter(outFileName)) {
                writer.WriteLine(headline);
                writer.Write(content);
            }
            PrintCreateFileSuccessfull(outFileName);
        }
        private static void BuildC113sum() {
            var outFileName = "C_113_sum.csv";
            var headline = "AnzahlHAges;vwdMWges;vwdStdGes;AnzKLHAges;AntKLHAges;AnzLLHAges;AntLLHAges";
            var cSum = "C_113_Zusf.csv";
            var content = "";
            using (var reader = CreateReaderWithoutHeadline(cSum)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    content += line.Replace("NULL", "0").Replace(",", ".") + Environment.NewLine;
                }
            }
            using (var writer = CreateWriter(outFileName)) {
                writer.WriteLine(headline);
                writer.Write(content);
            }
            PrintCreateFileSuccessfull(outFileName);
        }
        private static void BuildC213sum() {
            var outFileName = "C_213_sum.csv";
            var headline = "AnzahlBAges;vwdMWges;vwdStdGes;AnzKLBAges;AntKLBAges;AnzLLBAges;AntLLBAges";
            var cSum = "C_213_Zusf.csv";
            var content = "";
            using (var reader = CreateReaderWithoutHeadline(cSum)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    content += line.Replace("NULL", "0").Replace(",", ".") + Environment.NewLine;
                }
            }
            using (var writer = CreateWriter(outFileName)) {
                writer.WriteLine(headline);
                writer.Write(content);
            }
            PrintCreateFileSuccessfull(outFileName);
        }
    }
}