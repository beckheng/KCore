using UnityEngine;
using System.Collections;
using KData;

namespace KCore
{

	[DisallowMultipleComponent]
	public class KConfigLoader : MonoBehaviour
	{

		public static bool allConfigLoaded = false;

		// Use this for initialization
		void Start()
		{
			// 用协程来加载配置表
			StartCoroutine(LoadAllConfigData());
		}
		
		private IEnumerator LoadAllConfigData()
		{
			allConfigLoaded = false;

			yield return StartCoroutine(ConfigLoaderAutoGen.LoadAllData());
			
			allConfigLoaded = true;
		}

	}

}
