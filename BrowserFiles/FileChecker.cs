using System;
using System.IO;

namespace BrowserFiles {
    public static class FileChecker {
        public static string[] Files = {
                                           "A_1_KH.csv", "A_1_KH_Zusf.csv", "A_2_Datenqualitaet.csv",
                                           "A_3_Unspezif_Kodierung.csv",
                                           "B_1_KH_Bundesland.csv", "B_1_KH_Bundesland_Groesse(Betten).csv",
                                           "B_1_KH_Groesse(Betten).csv", "B_1_Zusf.csv",
                                           "B_2_KH_Groesse(Faelle).csv", "B_2_KH_Traeger.csv",
                                           "B_2_KH_Traeger_Groesse(Faelle).csv",
                                           "B_2_Zusf.csv", "B_3_KH_CMI.csv", "B_3_KH_Groesse(Betten).csv",
                                           "B_3_KH_Groesse(Betten)_CMI.csv",
                                           "B_3_Zusf.csv", "C_111_HA_MDC_Geschl.csv", "C_111_Zusf.csv",
                                           "C_112_HA_Alterskl.csv", "C_112_Zusf.csv",
                                           "C_113_HA_DRG.csv", "C_113_Zusf.csv", "C_114a_HA_HD_Kapitel.csv",
                                           "C_114b_HA_HD_Gruppe.csv",
                                           "C_114c_HA_HD_Kategorie.csv", "C_114_Zusf.csv", "C_115a_HA_Proz_Kapitel.csv",
                                           "C_115b_HA_Proz_Bereich.csv",
                                           "C_115c_HA_Proz_4Steller.csv", "C_115_HA_Proz_Fall.csv", "C_115_Zusf.csv",
                                           "C_121_HA_Bundesland_FZ_VWD_CMI.csv",
                                           "C_121_HA_Bundesland_Groesse(Betten)_FZ_VWD_CMI.csv",
                                           "C_121_HA_Groesse(Betten)_FZ_VWD_CMI.csv", "C_121_HA_Zusf.csv",
                                           "C_122_HA_Aufnahme.csv", "C_122_HA_Entlassung.csv", "C_123_HA_Haeufig_OP.csv",
                                           "C_211_BA_MDC_Geschl.csv", "C_211_Zusf.csv", "C_212_BA_Alterskl.csv",
                                           "C_212_Zusf.csv",
                                           "C_213_BA_DRG.csv", "C_213_Zusf.csv", "C_214a_BA_HD_Kapitel.csv",
                                           "C_214b_BA_HD_Gruppe.csv",
                                           "C_214c_BA_HD_Kategorie.csv", "C_214_Zusf.csv", "C_215a_BA_Proz_Kapitel.csv",
                                           "C_215b_BA_Proz_Bereich.csv",
                                           "C_215c_BA_Proz_4Steller.csv", "C_215_BA_Proz_Fall.csv", "C_215_Zusf.csv",
                                           "C_221_BA_Bundesland_FZ_VWD_CMI.csv",
                                           "C_221_BA_Bundesland_Groesse(Betten)_FZ_VWD_CMI.csv",
                                           "C_221_BA_Groesse(Betten)_FZ_VWD_CMI.csv",
                                           "C_221_BA_Zusf.csv", "C_222_BA_Aufnahme.csv", "C_222_BA_Entlassung.csv",
                                           "C_223_BA_Haeufig_OP.csv",
                                           "D_1a_TS_HD_Kapitel.csv", "D_1b_TS_HD_Gruppe.csv", "D_1c_TS_HD_Kategorie.csv",
                                           "D_1_TS_Zusf.csv",
                                           "D_2a_TS_Proz_Kapitel.csv", "D_2b_TS_Proz_Bereich.csv", "D_2_TS_Zusf.csv",
                                           "E_1a_HA_20_wenig_kompl.csv",
                                           "E_1a_Zusf.csv", "E_1b_BA_20_wenig_kompl.csv", "E_1b_Zusf.csv",
                                           "E_2a_HA_20_kompl.csv", "E_2a_Zusf.csv",
                                           "E_2b_BA_20_kompl.csv", "E_2b_Zusf.csv", "E_3a_HA_20_haeufig.csv",
                                           "E_3a_Zusf.csv", "E_3b_BA_20_haeufig.csv", "E_3b_Zusf.csv"
                                       };

        public static void CheckDirectoy(string filePrefix, string fileDir, int dataYear) {
            var sourceFiles = Directory.GetFiles(fileDir);
            foreach (var file in Files) {
                var found = false;
                foreach (var sourceFile in sourceFiles) {
                    var tmp = sourceFile.Replace(fileDir + "\\", "");
                    tmp = tmp.Replace(filePrefix, "");
                    if (tmp == file) {
                        found = true;
                        break;
                    }
                }
                if (!found)
                    throw new Exception("Fehler: Konnte Datei " + filePrefix + file + " nicht finden!");
            }
        }


    }
}