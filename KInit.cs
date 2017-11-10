﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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
			KScene.Load();
		}
		
		/// <summary>
		/// 当场景加载完成后会自动调用此方法
		/// </summary>
		/// <param name="level"></param>
		void OnLevelWasLoaded(int level)
		{
			KScene.Load();
		}

	}

}
