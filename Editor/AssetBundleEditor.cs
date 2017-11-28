using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

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

		[MenuItem("KCore/Assetbundle/Buil AB(Selected OR All)", false, 1071)]
		public static void BuildAllAB()
		{
			var obj = Selection.activeObject;
			if (obj == null)
			{
				Debug.LogError("Nothing|selected");
				return;
			}
			
			string theAssetPath = AssetDatabase.GetAssetPath(obj);
			if (!theAssetPath.StartsWith("Assets/Prefabs/"))
			{
				Debug.LogError("none|a|Prefabs|Under|Assets/Prefabs");
				return;
			}

			string outputPath = string.Empty;

			
			if (theAssetPath.Contains("/View/"))
			{
				outputPath = "Assets/StreamingAssets/View";
			}

			if (string.IsNullOrEmpty(outputPath))
			{
				Debug.LogError("no|detected|outputPath|for|" + theAssetPath);
				return;
			}

			AssetBundleBuild[] buildMap = new AssetBundleBuild[1];

			buildMap[0].assetBundleName = obj.name + ".u3d";
			buildMap[0].assetNames = new string[] { AssetDatabase.GetAssetPath(obj) };
			
			BuildPipeline.BuildAssetBundles(outputPath, buildMap, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);

			Debug.Log("BuildAB|OK|outputPath|" + outputPath + "|name|" + obj.name + ".u3d");

			AssetDatabase.Refresh();
		}

	}

}
