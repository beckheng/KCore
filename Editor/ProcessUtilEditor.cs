using UnityEngine;
using UnityEditor;
using System.Collections;

namespace KCoreEditor
{

	/// <summary>
	/// 仅在编辑器模式下调用
	/// </summary>
	public class ProcessUtilEditor : EditorWindow
	{

		/// <summary>
		/// 调用外部进程
		/// </summary>
		public static void StartProcess(string processPath, string args)
		{
			processPath = processPath.Replace("/", "\\");

			System.Diagnostics.Process p = new System.Diagnostics.Process();
			p.StartInfo.FileName = processPath;

			p.StartInfo.Arguments = args;

			p.StartInfo.UseShellExecute = true;
			p.StartInfo.RedirectStandardInput = false;
			p.StartInfo.RedirectStandardOutput = false;
			p.StartInfo.RedirectStandardError = false;

			p.Start();
			p.WaitForExit();
		}

		public static void OpenURL(string url)
		{
			System.Diagnostics.Process.Start(url);
		}

	}

}
