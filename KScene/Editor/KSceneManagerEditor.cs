using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

using KCore;
using KData;
using KScene;

public class KSceneManagerEditor : EditorWindow {
	
	/// <summary>
	/// 显示窗口
	/// </summary>
	[MenuItem("KCore/新建场景", false, 1101)]
	public static void ShowMySelf()
	{
		KSceneManagerEditor editorWin = EditorWindow.GetWindowWithRect<KSceneManagerEditor>(new Rect(0, 0, 640, 320));
		editorWin.titleContent = new GUIContent("新建场景");
	}

	private string sceneName = "";

	/// <summary>
	/// 显示控件
	/// </summary>
	private void OnGUI()
	{
		EditorGUILayout.LabelField("说明: ", "根据场景名来生成场景名Scene.unity以及场景名SceneManager.cs");
		EditorGUILayout.LabelField(" ", "自动补充Scene后缀,请确保首字母大写");

		EditorGUILayout.BeginHorizontal();
		{
			sceneName = EditorGUILayout.TextField("场景名字: ", sceneName);
			EditorGUILayout.LabelField("Scene");
		}
		EditorGUILayout.EndHorizontal();

		if (GUILayout.Button("生  成"))
		{
			string finalSceneName = sceneName.Trim();
			
			if (string.IsNullOrEmpty(finalSceneName))
			{
				EditorUtility.DisplayDialog("错误", "请输入一个合法的场景名字", "确定");
				return;

			}

			//判断首字母大写
			char firstCH = finalSceneName[0];
			if (firstCH < 'A' || firstCH > 'Z')
			{
				EditorUtility.DisplayDialog("错误", "场景名字需要首字母大写", "确定");
				return;
			}

			if (!finalSceneName.EndsWith("Scene"))
			{
				//补充Scene后缀
				finalSceneName += "Scene";
			}

			string scriptDir = Application.dataPath + "/Scripts/KScene";
			string scriptPath = scriptDir + "/" + finalSceneName + "Manager.cs";

			if (File.Exists(scriptPath))
			{
				EditorUtility.DisplayDialog("错误", "场景文件已经存在|" + scriptPath, "确定");
				return;
			}

			Debug.Log("scriptDir|" + scriptDir);
			Debug.Log("scriptPath|" + scriptPath);

			//不存在才生成文件
			if (!Directory.Exists(scriptDir))
			{
				//需要先创建目录
				Directory.CreateDirectory(scriptDir);
			}

			WriteManagerFileContent(scriptPath, finalSceneName);

			AssetDatabase.Refresh(ImportAssetOptions.Default);
		}
	}

	/// <summary>
	/// 生成代码写到path去
	/// </summary>
	/// <param name="path"></param>
	/// <param name="theSceneName">要求首字母大写</param>
	private static void WriteManagerFileContent(string path, string theSceneName)
	{
		string fileContent = 
			"using UnityEngine;\n" +
			"using System.Collections;\n" +
			"using UnityEngine.SceneManagement;\n\n" +

			"using KCore;\n" +
			"using KData;\n\n" +

			"namespace KScene\n" +
			"{\n\n" + 
			
			"	[DisallowMultipleComponent]\n" +
			"	public sealed class " + theSceneName + "Manager : KSceneManager\n" +
			"	{\n\n" +
			
			"		public override void LoadData()\n" +
			"		{\n" +
			"			Debug.LogError(\"" + theSceneName + "Manager|do|nothing\");\n" +
			"		}\n\n" +
			
			"	}\n\n" +
			
			"}\n\n";

		File.WriteAllText(path, fileContent, System.Text.Encoding.UTF8);
	}

}
