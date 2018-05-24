using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

using System.IO;

using KCore;

namespace KCoreEditor
{

	public class AssetBundleEditor : Editor
	{
		
		/// <summary>
		/// 加载所选的AB资源
		/// </summary>
		[MenuItem("KCore/Assetbundle/Load AB", false, 1000)]
		private static void LoadSelectedAssetBundle()
		{
			Object obj = Selection.activeObject;
			string path = AssetDatabase.GetAssetPath(obj);

			KAssetBundle.LoadByPath(path, (ab) =>
			{
				Sprite sprite = ab.LoadAsset<Sprite>(ab.name);

				Image image = GameObject.FindObjectOfType<Image>();
				image.sprite = sprite;
				image.SetNativeSize();

				image.SetAllDirty();
			});
		}

		/// <summary>
		/// 将所选目录的场景打包为AB
		/// </summary>
		[MenuItem("KCore/Assetbundle/Build Scene", false, 1051)]
		private static void BuildSceneAB()
		{
			BuildPipeline.BuildAssetBundles("Assets/OOO");
		}

		/// <summary>
		/// 将所选文件打包为AB
		/// </summary>
		[MenuItem("KCore/Assetbundle/Buil AB(Selected OR All)", false, 1071)]
		[MenuItem("Assets/KCore/Build AB(Selected)", false, 1000)]
		public static void BuildAllAB()
		{
			if (Selection.objects.Length == 0)
			{
				Debug.LogError("Nothing|selected");
				return;
			}

			var selectedObjects = Selection.objects;
			foreach (var obj in selectedObjects)
			{
				string theAssetPath = AssetDatabase.GetAssetPath(obj);

				string outputPath = GetABOutputPath(theAssetPath);

				if (string.IsNullOrEmpty(outputPath))
				{
					Debug.LogError("no|detected|outputPath|for|" + theAssetPath);
					return;
				}

				if (!Directory.Exists(outputPath))
				{
					Debug.Log("Create|Directory|" + outputPath);
					Directory.CreateDirectory(outputPath);
				}

				AssetBundleBuild[] buildMap = new AssetBundleBuild[1];

				buildMap[0].assetBundleName = obj.name.ToLower() + KAssetBundle.abNamePostfix;
				buildMap[0].assetNames = new string[] { AssetDatabase.GetAssetPath(obj) };

				BuildPipeline.BuildAssetBundles(outputPath, buildMap, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);

				Debug.Log("BuildAB|OK|outputPath|" + outputPath + "|name|" + obj.name + KAssetBundle.abNamePostfix);
			}

			AssetDatabase.Refresh();
		}

		/// <summary>
		/// 根据源路径,统一Assets/StreamingAssets的输出路径,没有则返回string.Empty
		/// </summary>
		/// <returns></returns>
		private static string GetABOutputPath(string assetPath)
		{
			//先判断类型
			if (assetPath.EndsWith(".controller"))
			{
				return "Assets/StreamingAssets/Animators";
			}

			//再判断路径
			if (assetPath.Contains("/Prefabs/View/"))
			{
				return "Assets/StreamingAssets/View";
			}
			else if (assetPath.Contains("/Arts/Sounds/"))
			{
				return "Assets/StreamingAssets/Sounds";
			}
			else if (assetPath.Contains("/Arts/Effects/"))
			{
				return "Assets/StreamingAssets/Effects";
			}
			else if (assetPath.Contains("/Prefabs/Models/"))
			{
				return "Assets/StreamingAssets/Models";
			}
			else if (assetPath.Contains("/Scenes/"))
			{
				return "Assets/StreamingAssets/Scenes";
			}

			return string.Empty;
		}

	}

}
