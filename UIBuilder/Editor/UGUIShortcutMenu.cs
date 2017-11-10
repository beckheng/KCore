using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace KCore.UIBuilder
{
	public class UGUIShortcutMenu
	{

		private static string uGUIMenuPrefix = "GameObject/UI";

		[MenuItem("KCore/添加水平自适应布局", false, 1100)]
		public static void AddHorizontalLayoutGroup()
		{
			GameObject[] allGo = Selection.gameObjects;
			if (allGo.Length == 0)
			{
				EditorUtility.DisplayDialog("", "Please select a gameobject!", "ok");
				return;
			}

			for (int i = 0; i < allGo.Length; i++)
			{
				GameObject go = allGo[i];
				if (go == null)
				{
					EditorUtility.DisplayDialog("", "Please select a gameobject!", "ok");
					continue;
				}

				HorizontalLayoutGroup hg = go.GetComponent<HorizontalLayoutGroup>();
				if (hg == null)
				{
					hg = go.AddComponent<HorizontalLayoutGroup>();
					hg.childForceExpandWidth = false;
					hg.childForceExpandHeight = false;
					hg.childAlignment = TextAnchor.MiddleCenter;
				}

				ContentSizeFitter csf = go.GetComponent<ContentSizeFitter>();
				if (csf == null)
				{
					csf = go.AddComponent<ContentSizeFitter>();
					csf.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
					csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
				}
			}
		}

		[MenuItem("KCore/添加垂直自适应布局", false, 1101)]
		public static void AddVerticalLayoutGroup()
		{
			GameObject[] allGo = Selection.gameObjects;
			if (allGo.Length == 0)
			{
				EditorUtility.DisplayDialog("", "Please select a gameobject!", "ok");
				return;
			}

			for (int i = 0; i < allGo.Length; i++)
			{
				GameObject go = allGo[i];
				if (go == null)
				{
					EditorUtility.DisplayDialog("", "Please select a gameobject!", "ok");
					continue;
				}

				VerticalLayoutGroup vg = go.GetComponent<VerticalLayoutGroup>();
				if (vg == null)
				{
					vg = go.AddComponent<VerticalLayoutGroup>();
					vg.childForceExpandWidth = false;
					vg.childForceExpandHeight = false;
					vg.childAlignment = TextAnchor.MiddleCenter;
				}

				ContentSizeFitter csf = go.GetComponent<ContentSizeFitter>();
				if (csf == null)
				{
					csf = go.AddComponent<ContentSizeFitter>();
					csf.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
					csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
				}
			}
		}

		/// <summary>
		/// 添加Button控件
		/// </summary>
		[MenuItem("KCore/Insert Button Control &b", false, 1102)]
		public static void FastInsertButtonControl()
		{
			GameObject selectedGo = Selection.activeGameObject;

			if (EditorApplication.ExecuteMenuItem(uGUIMenuPrefix + "/Button"))
			{
				GameObject insertControl = Selection.activeGameObject;

				SetControlPos(selectedGo, insertControl);
			}
		}

		/// <summary>
		/// 添加Image控件
		/// </summary>
		[MenuItem("KCore/Insert Image Control &i", false, 1103)]
		public static void FastInsertImageControl()
		{
			GameObject selectedGo = Selection.activeGameObject;

			if (EditorApplication.ExecuteMenuItem(uGUIMenuPrefix + "/Image"))
			{
				GameObject insertControl = Selection.activeGameObject;

				Image img = insertControl.GetComponent<Image>();
				img.raycastTarget = false;

				SetControlPos(selectedGo, insertControl);
			}
		}

		/// <summary>
		/// 添加Panel控件
		/// </summary>
		[MenuItem("KCore/Insert Panel Control &p", false, 1104)]
		public static void FastInsertPanelControl()
		{
			GameObject selectedGo = Selection.activeGameObject;

			if (EditorApplication.ExecuteMenuItem(uGUIMenuPrefix + "/Panel"))
			{
				GameObject insertControl = Selection.activeGameObject;

				SetControlPos(selectedGo, insertControl);
			}
		}

		/// <summary>
		/// 添加Text控件
		/// </summary>
		[MenuItem("KCore/Insert Text Control &t", false, 1105)]
		public static void FastInsertTextControl()
		{
			GameObject selectedGo = Selection.activeGameObject;

			if (EditorApplication.ExecuteMenuItem(uGUIMenuPrefix + "/Text"))
			{
				GameObject insertControl = Selection.activeGameObject;
				Text t = insertControl.GetComponent<Text>();
				t.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
				//AssetDatabase.LoadAssetAtPath<Font>("Assets/Art/UI/Font/msyhbd.");
				t.color = Color.white;

				SetControlPos(selectedGo, insertControl);
			}
		}

		///// <summary>
		///// 添加Image控件为Child
		///// </summary>
		//[MenuItem("KCore/Insert Image Control As Child #&i", false, 1106)]
		//public static void FastInsertImageChildControl()
		//{
		//	GameObject selectedGo = Selection.activeGameObject;

		//	if (EditorApplication.ExecuteMenuItem(uGUIMenuPrefix + "/Image"))
		//	{
		//		GameObject insertControl = Selection.activeGameObject;

		//		if (selectedGo != null)
		//		{
		//			insertControl.transform.SetParent(selectedGo.transform);
		//		}
		//	}
		//}

		///// <summary>
		///// 添加Panel控件为Child
		///// </summary>
		//[MenuItem("KCore/Insert Panel Control #&p", false, 1107)]
		//public static void FastInsertPanelChildControl()
		//{
		//	GameObject selectedGo = Selection.activeGameObject;

		//	if (EditorApplication.ExecuteMenuItem(uGUIMenuPrefix + "/Panel"))
		//	{
		//		GameObject insertControl = Selection.activeGameObject;

		//		if (selectedGo != null)
		//		{
		//			insertControl.transform.SetParent(selectedGo.transform);
		//		}
		//	}
		//}

		///// <summary>
		///// 添加Text控件为Child
		///// </summary>
		//[MenuItem("KCore/Insert Text Control #&t", false, 1108)]
		//public static void FastInsertTextChildControl()
		//{
		//	GameObject selectedGo = Selection.activeGameObject;

		//	if (EditorApplication.ExecuteMenuItem(uGUIMenuPrefix + "/Text"))
		//	{
		//		GameObject insertControl = Selection.activeGameObject;

		//		if (selectedGo != null)
		//		{
		//			insertControl.transform.SetParent(selectedGo.transform);
		//		}
		//	}
		//}

		private static void SetControlPos(GameObject selectedGo, GameObject insertControl)
		{
			if (selectedGo == null)
			{
				return;
			}

			if (selectedGo.transform.parent != null)
			{
				insertControl.transform.SetParent(selectedGo.transform.parent, true);
			}

			insertControl.transform.SetSiblingIndex(selectedGo.transform.GetSiblingIndex() + 1);
		}
		
		/// <summary>
		/// 增加宽度
		/// </summary>
		[MenuItem("KCore/Expand Width #\\", false, 1109)]
		public static void ExpandControlWidth()
		{
			GameObject selectedGo = Selection.activeGameObject;
			
			ExpandControl(selectedGo, 1, 0);
		}

		/// <summary>
		/// 缩减宽度
		/// </summary>
		[MenuItem("KCore/Reduce Width #[", false, 1110)]
		public static void ReduceControlWidth()
		{
			GameObject selectedGo = Selection.activeGameObject;

			ExpandControl(selectedGo, -1, 0);
		}

		/// <summary>
		/// 增加高度
		/// </summary>
		[MenuItem("KCore/Expand Height #]", false, 1111)]
		public static void ExpandControlHeight()
		{
			GameObject selectedGo = Selection.activeGameObject;

			ExpandControl(selectedGo, 0, 1);
		}

		/// <summary>
		/// 缩减高度
		/// </summary>
		[MenuItem("KCore/Reduce Height #=", false, 1112)]
		public static void ReduceControlHeight()
		{
			GameObject selectedGo = Selection.activeGameObject;

			ExpandControl(selectedGo, 0, -1);
		}

		/// <summary>
		/// 左移一像素
		/// </summary>
		[MenuItem("KCore/Move Left &[", false, 1113)]
		public static void MoveControlToLeft()
		{
			MoveControl(-1, 0);
		}

		/// <summary>
		/// 右移一像素
		/// </summary>
		[MenuItem("KCore/Move Right &\\", false, 1114)]
		public static void MoveControlToRight()
		{
			MoveControl(1, 0);
		}

		/// <summary>
		/// 上移一像素
		/// </summary>
		[MenuItem("KCore/Move Up &=", false, 1115)]
		public static void MoveControlToUp()
		{
			MoveControl(0, 1);
		}

		/// <summary>
		/// 下移一像素
		/// </summary>
		[MenuItem("KCore/Move Down &]", false, 1116)]
		public static void MoveControlToDown()
		{
			MoveControl(0, -1);
		}

		/// <summary>
		/// 移动控件位置
		/// </summary>
		private static void MoveControl(int offsetX, int offsetY)
		{
			GameObject [] gos = Selection.gameObjects;

			if (gos.Length == 0)
			{
				return;
			}

			foreach (var selectedGo in gos)
			{
				RectTransform rectTr = selectedGo.GetComponent<RectTransform>();
				if (rectTr == null)
				{
					return;
				}

				Vector2 pos = rectTr.anchoredPosition;
				pos.x += offsetX;
				pos.y += offsetY;

				rectTr.anchoredPosition = pos;
			}
		}

		/// <summary>
		/// 更改控件尺寸,直接修改anchoredPosition3D及sizeDelta即可
		/// </summary>
		private static void ExpandControl(GameObject selectedGo, int expandX, int expandY)
		{
			if (selectedGo == null)
			{
				return;
			}

			RectTransform rectTran = selectedGo.GetComponent<RectTransform>();
			if (rectTran == null)
			{
				return;
			}

			Vector2 sizeDelta = rectTran.sizeDelta;
			Vector3 anchoredPosition3D = rectTran.anchoredPosition3D;

			float leftX = expandX * rectTran.pivot.x;
			float topY = expandY * rectTran.pivot.y;

			anchoredPosition3D.x += leftX;
			anchoredPosition3D.y -= topY; // Y轴是左下角起始

			sizeDelta.x += expandX;
			sizeDelta.y += expandY;

			rectTran.anchoredPosition3D = anchoredPosition3D;
			rectTran.sizeDelta = sizeDelta;
		}
	}
}
