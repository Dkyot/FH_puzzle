using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Build.Reporting;
using UnityEditor.Build;
using System.IO;

namespace YG.EditorScr.BuildModify
{
    public class PostProcessBuild : IPreprocessBuildWithReport
    {
        public int callbackOrder
        {
            get { return 0; }
        }

        public void OnPreprocessBuild(BuildReport report)
        {
#if YG_PLUGIN_YANDEX_GAME
            string buildPatch = report.summary.outputPath + "/index.html";

            if (File.Exists(buildPatch))
            {
                File.Delete(buildPatch);
            }
#endif
        }

        [PostProcessBuild]
        public static void ModifyBuildDo(BuildTarget target, string pathToBuiltProject)
        {
#if YG_PLUGIN_YANDEX_GAME
            ModifyBuildManager.ModifyIndex(pathToBuiltProject);
            ArchivingBuild.Archiving(pathToBuiltProject);
            BuildLog.WritingLog(pathToBuiltProject);
#endif
        }
    }
}