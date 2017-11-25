using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEditor;
using UnityEditor.SceneManagement;

using System.Collections;
using System.Collections.Generic;
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

	private static bool saveSceneOK = false;
	private string sceneName = "";
	private List<string> tips = new List<string>();

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
			tips.Clear();

			bool isNeedCreateScene = false; //标志位
			bool isWriteFile = false; //标志位

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
			
			// 生成一个新的场景
			isNeedCreateScene = CreateEmptyScene(finalSceneName);

			string scriptDir = Application.dataPath + "/Scripts/KScene";
			string scriptPath = scriptDir + "/" + finalSceneName + "Manager.cs";

			if (File.Exists(scriptPath))
			{
				EditorUtility.DisplayDialog("错误", "场景逻辑层文件已经存在|" + scriptPath + ",不会被覆盖", "确定");
			}
			else
			{
				//不存在才生成文件
				if (!Directory.Exists(scriptDir))
				{
					//需要先创建目录
					Directory.CreateDirectory(scriptDir);
				}

				WriteManagerFileContent(scriptPath, finalSceneName);

				isWriteFile = true;
			}

			if (isNeedCreateScene)
			{
				tips.Add("创建场景: " + finalSceneName + " " + (saveSceneOK ? "成功" : "失败"));
			}
			else
			{
				tips.Add("不需创建场景: " + finalSceneName);
			}

			if (isWriteFile)
			{
				tips.Add("创建场景逻辑层文件: " + scriptPath);
			}
			else
			{
				tips.Add("不会覆盖逻辑层文件: " + finalSceneName);
			}

			if (isNeedCreateScene || isWriteFile)
			{
				AssetDatabase.Refresh(ImportAssetOptions.Default);
			}
		}

		for (int i = 0; i < tips.Count; i++)
		{
			EditorGUILayout.LabelField(" ", tips[i]);
		}
	}

	/// <summary>
	/// 在编辑器模式下创建一个新的场景,场景放在Assets/Scenes目录下
	/// <returns>true:需要创建; false:不需创建</returns>
	/// </summary>
	private static bool CreateEmptyScene(string theSceneName)
	{
		//string scenePathABS = Application.dataPath;

		string scenePath = "Assets/Scenes/" + theSceneName + ".unity";
		Scene theScene = EditorSceneManager.GetSceneByPath(scenePath);
		Debug.Log("scenePath|" + scenePath + "|theScene| " + theScene.IsValid());
		if (!theScene.IsValid())
		{
			//不存在则创建Scene

			//提示保存有更改的场景
			EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
			
			//创建并保存
			Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
		 	saveSceneOK = EditorSceneManager.SaveScene(newScene, scenePath);

			return true;
		}
		else
		{
			return false;
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
