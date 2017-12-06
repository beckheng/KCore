using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using System.IO;

namespace KCore
{

	[DisallowMultipleComponent]
	public class KAssetBundle : MonoBehaviour
	{

		public const string abNamePostfix = ".u3d";

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
		/// 加载持久的AB,支持AB名自动补.u3d后缀
		/// </summary>
		public static IEnumerator LoadPersistentAB(string[] relativePaths, System.Action onSucc, System.Action onError = null)
		{
			for (int i = 0; i < relativePaths.Length; i++)
			{
				string thePath = relativePaths[i];
				if (!thePath.EndsWith(abNamePostfix))
				{
					thePath += abNamePostfix;
				}

				FileInfo fi = new FileInfo(thePath);
				string theAbName = fi.Name;

				if (abMap.ContainsKey(theAbName))
				{
					Debug.LogError(Time.frameCount + "|duplicate|load|" + thePath);
					continue;
				}

				yield return LoadFromStreamAssets(thePath, (w) => {
					//Debug.Log(Time.frameCount + "|add|abmap|" + w.assetBundle.name + "|savein|" + theAbName);
					ApplyShaderForEditorMode(w.assetBundle);

					abMap.Add(theAbName, w.assetBundle);
				}, null);
			}

			if (onSucc != null)
			{
				onSucc();
			}
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

			string url = null;
			if (Application.platform == RuntimePlatform.Android)
			{
				url = Application.streamingAssetsPath + relativePath;
			}
			else
			{
				url = "file://" + Application.streamingAssetsPath + relativePath;
			}
			
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

		/// <summary>
		/// 从AB资源中加载共享的资源,这些资源不像GO那样,需要实例化的,如声音,纹理,Animator这些
		/// </summary>
		public static T GetObject<T>(string assetName) where T : Object
		{
			string viewABName = assetName;

			if (!viewABName.EndsWith(abNamePostfix))
			{
				viewABName += abNamePostfix;
			}

			T t = default(T);

			if (!abMap.ContainsKey(viewABName))
			{
				Debug.LogError(Time.frameCount + "|no|ab|loaded");
				return t;
			}

			return abMap[viewABName].LoadAsset<T>(assetName);
		}

		/// <summary>
		/// 从AB资源中实例化一个View(UI),要求AB资源必须已经提前加载进来,支持AB名自动补.u3d后缀,暂时规定assetName和ABName是一致的
		/// </summary>
		public static T InstantiateView<T>(string assetName) where T : Component
		{
			string viewABName = assetName;

			if (!viewABName.EndsWith(abNamePostfix))
			{
				viewABName += abNamePostfix;
			}

			T t = default(T);

			if (!abMap.ContainsKey(viewABName))
			{
				Debug.LogError(Time.frameCount + "|no|ab|loaded");
				return t;
			}
			
			GameObject go = abMap[viewABName].LoadAsset<GameObject>(assetName);

			GameObject cloneGo = GameObject.Instantiate(go);
			cloneGo.name = go.name;

			T comp = cloneGo.GetComponent<T>();
			if (comp != null)
			{
				Debug.Log(Time.frameCount + "|use|exists|comp");
				return comp;
			}
			else
			{
				Debug.Log(Time.frameCount + "|add|comp");
				return cloneGo.AddComponent<T>();
			}
		}

		/// <summary>
		/// 从AB资源中实例化一个GO(Model),要求AB资源必须已经提前加载进来,支持AB名自动补.u3d后缀,暂时规定assetName和ABName是一致的
		/// </summary>
		public static T InstantiateModel<T>(string assetName) where T : Component
		{
			string viewABName = assetName;

			if (!viewABName.EndsWith(abNamePostfix))
			{
				viewABName += abNamePostfix;
			}

			T t = default(T);

			if (!abMap.ContainsKey(viewABName))
			{
				Debug.LogError(Time.frameCount + "|no|ab|loaded");
				return t;
			}

			GameObject go = abMap[viewABName].LoadAsset<GameObject>(assetName);

			GameObject cloneGo = GameObject.Instantiate(go);
			cloneGo.name = go.name;

			T comp = cloneGo.GetComponent<T>();
			if (comp != null)
			{
				Debug.Log(Time.frameCount + "|use|exists|comp");
				return comp;
			}
			else
			{
				Debug.Log(Time.frameCount + "|add|comp");
				return cloneGo.AddComponent<T>();
			}
		}

		/// <summary>
		/// 从AB资源中实例化一个特效(带ParticleSystem的),要求AB资源必须已经提前加载进来,支持AB名自动补.u3d后缀,暂时规定assetName和ABName是一致的
		/// </summary>
		public static Transform InstantiateEffect(string assetName)
		{
			string viewABName = assetName;

			if (!viewABName.EndsWith(abNamePostfix))
			{
				viewABName += abNamePostfix;
			}

			if (!abMap.ContainsKey(viewABName))
			{
				Debug.LogError(Time.frameCount + "|no|ab|loaded");
				return null;
			}

			GameObject go = abMap[viewABName].LoadAsset<GameObject>(assetName);

			GameObject cloneGo = GameObject.Instantiate(go);
			cloneGo.name = go.name;

			//ApplyShaderForEditorMode(cloneGo.transform);

			return cloneGo.transform;
		}

		/// <summary>
		/// 在Editor模式下,重新赋值一次shader,以令显示正常,这里传入的是AssetBundle
		/// </summary>
		/// <param name="ab"></param>
		private static void ApplyShaderForEditorMode(AssetBundle ab)
		{
			if (Application.platform == RuntimePlatform.WindowsEditor
				|| Application.platform == RuntimePlatform.OSXEditor)
			{
				if (ab.isStreamedSceneAssetBundle)
				{
					//场景的处理
				}
				else
				{
					//预制的处理
					GameObject[] gameObjectArray = ab.LoadAllAssets<GameObject>();
					//Debug.Log("ApplyShaderForEditorMode|AssetBundle|gameObjectArray|" + gameObjectArray.Length);
					for (int i = 0; i < gameObjectArray.Length; i++)
					{
						Renderer[] matArray = gameObjectArray[i].GetComponentsInChildren<Renderer>();
						for (int j = 0; j < matArray.Length; j++)
						{
							// 注意这里要使用sharedmaterial,因为并未实例化
							//Debug.Log("ApplyShaderForEditorMode|Transform|j|" + j + "|renderer|" + matArray[j]);
							Shader theShader = Shader.Find(matArray[j].sharedMaterial.shader.name);
							matArray[j].sharedMaterial.shader = theShader;
						}
					}
				}
			}
		}
		
	}

}
