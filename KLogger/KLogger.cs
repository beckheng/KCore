using System.IO;
using UnityEngine;

namespace KCore
{

	public static class KLogger
	{

		/// <summary>
		/// 日志文件输出位置,格式如[logPathPrefix]_yyyyMMdd.log
		/// </summary>
		public static string logFilePrefix = string.Empty;

		/// <summary>
		/// 是否同时将日志输出到控制台,默认为TRUE
		/// </summary>
		public static bool logToConsole = true;

		/// <summary>
		/// 一般日志信息
		/// </summary>
		public static void Log(string str)
		{
			if (logToConsole)
			{
				Debug.Log(str);
			}

			AppendLog(str);
		}

		/// <summary>
		/// 警告日志信息
		/// </summary>
		public static void LogError(string str)
		{
			LogError(str, null);
		}

		/// <summary>
		/// 警告日志信息
		/// </summary>
		public static void LogError(string str, Object obj)
		{
			if (logToConsole)
			{
				Debug.LogError(str);
			}

			AppendLog(str);
		}

		/// <summary>
		/// 出错的日志信息
		/// </summary>
		public static void LogWarning(string str)
		{
			if (logToConsole)
			{
				Debug.LogWarning(str);
			}

			AppendLog(str);
		}

		private static void AppendLog(string str)
		{
			string logDir = Application.persistentDataPath + "/logs";
			if (!Directory.Exists(logDir))
			{
				Directory.CreateDirectory(logDir);
			}

			var now = System.DateTime.Now;

			string path = logDir + "/" + logFilePrefix + "_" + now.ToString("yyyyMMdd") + ".log";

			File.AppendAllText(path, now.ToString("yyyy-MM-dd HH:mm:ss") + "\t" + str + "\r\n", System.Text.Encoding.UTF8);
		}
	}

}
