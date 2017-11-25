using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace KScene
{

	[DisallowMultipleComponent]
	public abstract class KSceneManager : MonoBehaviour
	{
		protected object data = null;

		/// <summary>
		/// 加载跳转到某一场景
		/// TODO 跳转到自己会是怎么样呢?
		/// </summary>
		/// <param name="showLoading">默认是TRUE,大概只有在Splash后的跳转,才是FALSE吧</param>
		public static void GotoScene<T>(object data, bool showLoading = true) where T : KSceneManager
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

			//如果没有,则先添加组件
			KSceneManager ks = GetCurKScene();
			if (null == ks)
			{
				GameObject go = new GameObject();
				go.name = "__" + scene.name + "__";

				System.Type t = System.Type.GetType(scene.name);

				ks = (KSceneManager)go.AddComponent(t);
			}

			ks.LoadData();
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
