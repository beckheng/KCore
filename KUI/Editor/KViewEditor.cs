using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

using System.Collections;

using KCore;
using KData;
using KScene;

public class KViewEditor : EditorWindow {

	/// <summary>
	/// 显示窗口
	/// </summary>
	[MenuItem("KCore/新建View-Window", false, 2050)]
	public static void ShowMySelf()
	{
		KViewEditor editorWin = EditorWindow.GetWindowWithRect<KViewEditor>(new Rect(0, 0, 640, 320));
		editorWin.titleContent = new GUIContent("新建View-Window");
	}

	private string theWindowName = string.Empty;

	/// <summary>
	/// 显示控件
	/// </summary>
	private void OnGUI()
	{
		EditorGUILayout.LabelField("说明: ", "在Hierarchy创建Canvas,并设置默认参数");
		
		EditorGUILayout.BeginHorizontal();
		{
			theWindowName = EditorGUILayout.TextField("场景名字: ", theWindowName);
		}
		EditorGUILayout.EndHorizontal();

		if (GUILayout.Button("创  建"))
		{
			string finalWindowName = theWindowName.Trim();

			if (string.IsNullOrEmpty(finalWindowName))
			{
				EditorUtility.DisplayDialog("错误", "请输入一个合法的Window名字", "确定");
				return;
			}

			//判断首字母大写
			char firstCH = finalWindowName[0];
			if (firstCH < 'A' || firstCH > 'Z')
			{
				EditorUtility.DisplayDialog("错误", "Window名字需要首字母大写", "确定");
				return;
			}
			
			if (EditorApplication.ExecuteMenuItem("GameObject/UI/Canvas"))
			{
				GameObject insertControl = Selection.activeGameObject;
				insertControl.name = finalWindowName;

				CanvasScaler cs = insertControl.GetComponent<CanvasScaler>();
				cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
				cs.referenceResolution = new Vector2(1136, 640);
				cs.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
				cs.matchWidthOrHeight = 0;
			}
		}
	}

}
