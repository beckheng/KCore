using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

using KScene;

namespace KCore
{

	/// <summary>
	/// 游戏初始化时调用的类,基本上就是Unity加载第一个场景时会被调用一次的方法
	/// </summary>
	[DisallowMultipleComponent]
	public class KInit : MonoBehaviour
	{

		/// <summary>
		/// 首个场景开始加载前调用此方法
		/// </summary>
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void KGameStart()
		{
#if UNITY_EDITOR
			Application.runInBackground = true;
#endif

			if (Application.isMobilePlatform)
			{
				Debug.Log(Time.frameCount + "|" + Time.timeSinceLevelLoad + "|set|targetFrameRate|30");
				Application.targetFrameRate = 30;
			}
			else
			{
				Debug.Log(Time.frameCount + "|" + Time.timeSinceLevelLoad + "|set|targetFrameRate|60");
				Application.targetFrameRate = 60;
			}

			GameObject go = new GameObject();
			go.name = "__KInit__";
			DontDestroyOnLoad(go);

			go.AddComponent<KInit>();
		}

		/// <summary>
		/// 首个场景完成加载后调用此方法
		/// </summary>
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void KGameEnd()
		{
			var configLoader = new GameObject("ConfigLoader");
			configLoader.AddComponent<KConfigLoader>();

			KSceneManager.Load();
		}
		
		/// <summary>
		/// 当场景加载完成后会自动调用此方法
		/// </summary>
		/// <param name="level"></param>
		void OnLevelWasLoaded(int level)
		{
			KSceneManager.Load();
		}

	}

}
