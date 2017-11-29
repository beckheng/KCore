using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace KCoreEditor
{

	public class GameHelperCmd : Editor
	{

		/// <summary>
		/// Start Game Menu
		/// </summary>
		[MenuItem("KCore/从首场景开始游戏", false, 900)]
		public static void GameStart()
		{
			if (EditorBuildSettings.scenes.Length == 0)
			{
				EditorUtility.DisplayDialog("提示", "请先添加一个场景到Build Settings", "OK");
				return;
			}

			EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
			
			EditorSceneManager.OpenScene(EditorBuildSettings.scenes[0].path, OpenSceneMode.Single);

			EditorApplication.ExecuteMenuItem("Edit/Play");
		}
		
	}

}
