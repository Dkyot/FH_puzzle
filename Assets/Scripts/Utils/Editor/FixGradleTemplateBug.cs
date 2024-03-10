using System.IO;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Utils.Editor
{
    // class FixGradleTemplateBug : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    // {
    //     public int callbackOrder { get { return 0; } }
    //
    //     private const string GradleMainTemplatePath = "Assets/Plugins/Android/mainTemplate.gradle";
    //
    //     private string originalMainTemplateContents = null;
    //
    //     public void OnPreprocessBuild(BuildReport report)
    //     {
    //         originalMainTemplateContents = null;
    //
    //         if (File.Exists(GradleMainTemplatePath))
    //         {
    //             originalMainTemplateContents = File.ReadAllText(GradleMainTemplatePath);
    //             string projectDir = Path.GetDirectoryName(Application.dataPath);
    //             Debug.Log($"To fix a bug, replacing **DIR_UNITYPROJECT** with \"{projectDir}\" in the gradle template file \"{GradleMainTemplatePath}\"."
    //                       + " This fix can be removed after updating to Unity 2021.2.8 or later");
    //             string fixedContents = originalMainTemplateContents.Replace("**DIR_UNITYPROJECT**", projectDir);
    //             File.WriteAllText(GradleMainTemplatePath, fixedContents);
    //         }
    //     }
    //
    //     public void OnPostprocessBuild(BuildReport report)
    //     {
    //         if (!string.IsNullOrEmpty(originalMainTemplateContents))
    //         {
    //             File.WriteAllText(GradleMainTemplatePath, originalMainTemplateContents);
    //             originalMainTemplateContents = null;
    //         }
    //     }
    // }
}