using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

using KCore;
using KData;

public class KBuildEditor : EditorWindow {

	[MenuItem("KCore/Build/Export APK(Demo型)", false, 1031)]
	public static void BuildAPK()
	{
		if (Application.isPlaying)
		{
			EditorUtility.DisplayDialog("警告", "请输入停止运行模式", "确定");
			return;
		}

		if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
		{
			EditorBuildSettingsScene[] buildScenes = EditorBuildSettings.scenes;

			List<string> buildScenePaths = new List<string>();
			for (int i = 0; i < buildScenes.Length; i++)
			{
				if (buildScenes[i].enabled)
				{
					buildScenePaths.Add(buildScenes[i].path);
				}
			}

			string apkPath = Application.dataPath + "/../../" + PlayerSettings.productName + ".apk";
			BuildPipeline.BuildPlayer(buildScenePaths.ToArray(), apkPath, EditorUserBuildSettings.activeBuildTarget, BuildOptions.None);
		}
	}

	[MenuItem("KCore/Build/Export APK(Demo型)", true)]
	public static bool BuildAPKValidation()
	{
		return EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android;
	}

	[MenuItem("KCore/Build/Export Android Project(正式环境)", false, 1032)]
	public static void BuildAndroidProj()
	{
		if (Application.isPlaying)
		{
			EditorUtility.DisplayDialog("警告", "请输入停止运行模式", "确定");
			return;
		}
	}

	[MenuItem("KCore/Build/Export Android Project(正式环境)", true)]
	public static bool BuildAndroidProjValidation()
	{
		return EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android;
	}

}
