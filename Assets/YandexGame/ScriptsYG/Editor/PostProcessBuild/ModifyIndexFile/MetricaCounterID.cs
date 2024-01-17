using UnityEngine;
using System.IO;

namespace YG.EditorScr.BuildModify
{
    public partial class ModifyBuildManager
    {
        public static void SetMetrica()
        {
            if (infoYG.metricaEnable)
            {
                string donorPatch = Application.dataPath +
                                    "/YandexGame/ScriptsYG/Editor/PostProcessBuild/ModifyIndexFile/MetricaDonor.html";
                string donorText = File.ReadAllText(donorPatch);
                if (infoYG.metricaSendWhenInit)
                {
                    donorText = donorText.Replace("METRICA_INIT_EVENT_NAME", infoYG.metricaInitEventName);
                }
                else
                {
                    donorText = donorText.Replace(
                        "ym(METRICA_COUNTER_ID, 'reachGoal', 'METRICA_INIT_EVENT_NAME'); //Send init event",
                        "//Send init event disable");
                }

                donorText = donorText.Replace("METRICA_COUNTER_ID", infoYG.metricaCounterID.ToString());
                AddIndexCode(donorText, CodeType.head);
            }
        }
    }
}