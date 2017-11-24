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

	}

}
