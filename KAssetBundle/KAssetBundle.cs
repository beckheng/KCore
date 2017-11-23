using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace KCore
{

	[DisallowMultipleComponent]
	public class KAssetBundle : MonoBehaviour
	{

		/// <summary>
		/// 挂组件的GO
		/// </summary>
		private static KAssetBundle self = null;

		/// <summary>
		/// 记录已经加载的AB
		/// </summary>
		private static Dictionary<string, AssetBundle> abMap = new Dictionary<string, AssetBundle>();

		private void OnDestroy()
		{
			foreach (var item in abMap)
			{
				item.Value.Unload(true);
			}
		}

		private static void Init()
		{
			//已经初始化则不处理了
			if (null != self)
			{
				return;
			}

			self = GameObject.FindObjectOfType<KAssetBundle>();
			if (self == null)
			{

				KLogger.Log("开始初始化|KAssetBundle");

				GameObject go = new GameObject();
				go.name = "__KAssetBundle__";
				DontDestroyOnLoad(go); //场景切换也保留

				self = go.AddComponent<KAssetBundle>();
			}
		}

		public static void LoadByPath(string path, System.Action<AssetBundle> cb)
		{
			//总是先尝试初始化
			Init();

			path = fixedPath(path);
			if (abMap.ContainsKey(path) && null != abMap[path])
			{
				KLogger.Log("使用缓存的AB|" + path);
				cb(abMap[path]);
				return;
			}

			self.StartCoroutine(_loadAB(path, cb));
		}

		/// <summary>
		/// 这个方法要做成适合Editor与各个平台公用
		/// </summary>
		private static IEnumerator _loadAB(string path, System.Action<AssetBundle> cb)
		{
			path = fixedPath(path);

			WWW www = WWW.LoadFromCacheOrDownload(path, 1);
			yield return www;

			if (string.IsNullOrEmpty(www.error))
			{
				AssetBundle assetBundle = www.assetBundle;
				abMap.Add(path, assetBundle);

				if (null != cb)
				{
					cb(assetBundle);
				}
			}
			else
			{
				KLogger.LogError(www.error);
			}
		}

		/// <summary>
		/// 修正路径适用各个平台
		/// </summary>
		private static string fixedPath(string path)
		{
			if (!path.StartsWith("jar:"))
			{
				//安卓平台会带这个前缀
				//其它的需要补充适合用的URL

				if (!path.StartsWith("/"))
				{
					int ix = path.IndexOf("Assets/");
					if (ix >= 0)
					{
						path = path.Substring(ix + "Assets/".Length);
					}

					//非绝对路径,都认为是在Unity Application目录下
					path = Application.dataPath + "/" + path;
				}

				path = "file://" + path;
			}

			return path;
		}

		/// <summary>
		/// 仅仅是从StreamAssets加载
		/// </summary>
		/// <param name="relativePath">Assets/StreamingAssets下的相对路径</param>
		public static IEnumerator LoadFromStreamAssets(string relativePath, System.Action<WWW> onSucc, System.Action<WWW> onError = null)
		{
			if (!relativePath.StartsWith("/"))
			{
				relativePath = "/" + relativePath;
			}

			string url = "file://" + Application.streamingAssetsPath + relativePath;
			using (WWW w = new WWW(url))
			{
				yield return w;

				if (w.error != null)
				{
					// 出错了
					Debug.LogError("www|error|url|" + url  + "|message|" + w.error);

					if (onError != null)
					{
						onError(w);
					}
				}
				else
				{
					// 成功
					if (onSucc != null)
					{
						onSucc(w);
					}
				}
			};
		}

		/// <summary>
		/// 从外部或者是StreamAssets加载,外部优先
		/// </summary>
		public static void LoadFromExternalOrStreamAssets()
		{

		}

	}

}
