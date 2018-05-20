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
	public sealed class KInit : MonoBehaviour
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

			KGameSetting kGameSetting = KGameSetting.LoadKGameSetting();
			if (kGameSetting == null)
			{
				//重要的事情说三遍
				for (int i = 0; i < 3; i++)
				{
					Debug.LogError("KGameSetting|was|not|found|on|Resources/KGameSetting.asset|请点击菜单 \"KCore/游戏开发设定\"");
				}
				return;
			}

			if (Application.platform == RuntimePlatform.Android)
			{
				Debug.Log(Time.frameCount + "|" + Time.timeSinceLevelLoad + "|set|targetFrameRate|" + kGameSetting.androidFrameRate);
				Application.targetFrameRate = kGameSetting.androidFrameRate;
			}
			else if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Debug.Log(Time.frameCount + "|" + Time.timeSinceLevelLoad + "|set|targetFrameRate|" + kGameSetting.iOSFrameRate);
				Application.targetFrameRate = kGameSetting.iOSFrameRate;
			}
			else
			{
				Debug.Log(Time.frameCount + "|" + Time.timeSinceLevelLoad + "|set|targetFrameRate|" + kGameSetting.otherPlatformFrameRate);
				Application.targetFrameRate = kGameSetting.otherPlatformFrameRate;
			}

			GameObject go = new GameObject();
			go.name = "__KInit__";
			DontDestroyOnLoad(go);

			go.AddComponent<KInit>();

			KSound.Init();
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
