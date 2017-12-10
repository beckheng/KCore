using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

using KCore;

namespace KScene
{

	/// <summary>
	/// 每个子类的KsceneManager都是会挂到一个GameObject的
	/// </summary>
	[DisallowMultipleComponent]
	public abstract class KSceneManager : MonoBehaviour
	{
		protected object data = null;

		/// <summary>
		/// 缓存的对象本身
		/// </summary>
		protected Transform tran = null;

		/// <summary>
		/// 继承于MonoBehaviour的方法
		/// </summary>
		void Awake()
		{
			tran = transform;
		}

		/// <summary>
		/// 切换场景的统一调用,简化切换场景的代码
		/// </summary>
		public static void SwitchScene(string[] preloadABs, string sceneName)
		{
			KSceneManager ks = GetCurKScene();

			ks.StartCoroutine(KAssetBundle.LoadPersistentAB(preloadABs, () => {
				SceneManager.LoadSceneAsync(sceneName);
			}));
		}

		/// <summary>
		/// 加载跳转到某一场景
		/// TODO 跳转到自己会是怎么样呢?
		/// 暂时未用到
		/// </summary>
		/// <param name="showLoading">默认是TRUE,大概只有在Splash后的跳转,才是FALSE吧</param>
		private static void GotoScene<T>(object data, bool showLoading = true) where T : KSceneManager
		{
			string typeName = typeof(T).Name;
			
			Debug.Log("开始加载|" + new System.Exception().StackTrace);

			Debug.Break();

			if (showLoading)
			{
				//显示Loading的处理方法
			}

			KSceneManager ks = GetCurKScene();
			if (null != ks)
			{
				DestroyImmediate(ks.gameObject);
				ks = null;
			}
			
			SceneManager.LoadSceneAsync(typeName);

			if (null == ks)
			{
				GameObject go = new GameObject();
				go.name = "__" + typeName + "__";
				DontDestroyOnLoad(go); //这里的组件需要手动来删除了

				ks = go.AddComponent<T>();
			}

			ks.data = data;
		}
		
		/// <summary>
		/// 当场景加载完后,调用此方法来处理
		/// </summary>
		public static void Load()
		{
			Scene scene = SceneManager.GetActiveScene(); //默认组件与场景名一致
			
			// 处理天空盒
			if (Application.platform == RuntimePlatform.WindowsEditor
				|| Application.platform == RuntimePlatform.OSXEditor)
			{
				if (RenderSettings.skybox != null)
				{
					Shader theShader = Shader.Find(RenderSettings.skybox.shader.name);

					RenderSettings.skybox.shader = theShader;
				}
			}

			// 添加EventSystem
			EventSystem eventSystem = GameObject.FindObjectOfType<EventSystem>();
			Debug.Log("eventSystem|" + eventSystem);
			if (eventSystem == null)
			{
				Debug.Log("load|EventSystem|Prefab");
				GameObject origGo = Resources.Load<GameObject>("EventSystem");
				GameObject newGo = GameObject.Instantiate<GameObject>(origGo);
				newGo.name = origGo.name;
			}
			
			//如果没有,则先添加组件
			KSceneManager ks = GetCurKScene();
			if (null == ks)
			{
				GameObject go = new GameObject();
				go.name = "__" + scene.name + "__";

				System.Type t = null;

				string defaultClassName = "KScene." + scene.name + "Manager";

				// 场景管理组件钩子
				t = KSceneHook.GetManagerType(scene.name);

				if (t == null)
				{
					Debug.Log("use|default|type|" + defaultClassName);
					t = System.Type.GetType(defaultClassName);
				}

				if (t == null)
				{
					Debug.LogError("cannot|find|default|type|" + defaultClassName);
				}
				else
				{
					ks = (KSceneManager)go.AddComponent(t);
				}
			}

			if (ks != null)
			{
				ks.LoadData();
			}
			else
			{
				Debug.LogError("cannot|find|ks");
			}
		}

		/// <summary>
		/// 加载场景所需资源
		/// </summary>
		public abstract void LoadData();

		/// <summary>
		/// 取得当前场景的KScene组件,不允许在同个场景有两个KScene组件
		/// </summary>
		/// <returns></returns>
		private static KSceneManager GetCurKScene()
		{
			KSceneManager ks = GameObject.FindObjectOfType<KSceneManager>();

			return ks;
		}
		
	}

}
