using UnityEngine;
using System.Collections;
using UnityEditor;

using KCore;

public class KGameSettingEditor : Editor
{

	[MenuItem("KCore/游戏开发设定", false, 951)]
	private static void CreateKGameSettingAsset()
	{
		KGameSetting kGameSetting = KGameSetting.LoadKGameSetting();
		if (kGameSetting == null)
		{
			// 没有则先创建
			kGameSetting = ScriptableObject.CreateInstance<KGameSetting>();
			AssetDatabase.CreateAsset(kGameSetting, "Assets/Resources/KGameSetting.asset");
			AssetDatabase.Refresh();

			// 需要选中的是Resources下的.asset文件
			kGameSetting = KGameSetting.LoadKGameSetting();
		}

		Selection.activeObject = kGameSetting;
	}

}
