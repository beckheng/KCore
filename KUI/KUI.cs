using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace KCore
{
	/// <summary>
	/// UI Manager
	/// </summary>
	public class KUI
	{
		private static Dictionary<string, string> uiMap = new Dictionary<string, string>();

		private static Stack<KUIWindow> winsStatck = new Stack<KUIWindow>();

		/// <summary>
		/// 加载UI影射,需要指定UI预制的根目录
		/// </summary>
		public static void LoadUIManifest(string uiRootPath)
		{
			string resIndicator = "Resources/";

			var sw = new System.Diagnostics.Stopwatch();
			sw.Start();

			var files = Directory.GetFiles(uiRootPath, "*.prefab", System.IO.SearchOption.AllDirectories);
			for (int i = 0; i < files.Length; i++)
			{
				//统一目录分隔符
				string fullpath = files[i].Replace("\\", "/");

				//以Resources.Load方式来加载
				int resIx = fullpath.IndexOf(resIndicator);
				fullpath = fullpath.Substring(resIx + resIndicator.Length);
				
				var fileInfo = new FileInfo(fullpath);

				string key = fileInfo.Name.Replace(".prefab", "");
				string value = fullpath.Replace(".prefab", "");

				uiMap.Add(key, value);
				//KLogger.Log("file|i|" + i + "|key|" + key + "|value|" + value);
			}
			sw.Stop();

			KLogger.Log("KUI|加载Map时间|" + sw.ElapsedMilliseconds);
		}

		/// <summary>
		/// 加载Window
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="data"></param>
		/// <example>
		/// var win = KUI.LoadWindow<MainWindow>(null);
		/// </example>
		/// <returns></returns>
		public static T LoadWindow<T>(object data) where T : KUIWindow
		{
			var sw = new System.Diagnostics.Stopwatch();
			sw.Start();

			string uiName = typeof(T).Name;

			if (!uiMap.ContainsKey(uiName))
			{
				KLogger.LogError("KUI|没有找到对应的UI|" + uiName);
				return default(T);
			}
			string uiPath = uiMap[uiName];

			var swResLoad = new System.Diagnostics.Stopwatch();
			swResLoad.Start();
			var prefab = Resources.Load<T>(uiPath);
			swResLoad.Stop();
			KLogger.LogError("KUI|Resources.Load|时间|" + swResLoad.ElapsedMilliseconds);

			var swInst = new System.Diagnostics.Stopwatch();
			swInst.Start();
			T go = GameObject.Instantiate<T>(prefab);
			swInst.Stop();
			KLogger.LogError("KUI|GameObject.Instantiate|时间|" + swInst.ElapsedMilliseconds);

			sw.Stop();
			KLogger.LogError("KUI|LoadWindow|时间|" + sw.ElapsedMilliseconds);

			return go;
		}

		public static void LoadTopestWindow<T>(object data) where T : KUITopestWindow
		{

		}

		public static void LoadPanel<T>(object data) where T : KUIPanel
		{

		}

		public static void Close(KUIObject win)
		{
		}
	}
}
