using UnityEngine;

namespace KOF
{
	/// <summary>
	/// 文件系统相关的方法
	/// </summary>
	public class KFileUtil
	{

		/// <summary>
		/// 返回文件相应的URL,用于WWW这种调用
		/// </summary>
		/// <param name="originalPath"></param>
		/// <returns></returns>
		public string GetFileURL(string originalPath)
		{
			if (originalPath.StartsWith("/"))
			{
				return "file://" + originalPath;
			}

			return string.Empty;
		}

		/// <summary>
		/// 返回文件系统的绝对路径,用于直接读取文件,仅用于StreamingAssets路径的特殊性处理,其它路径请直接使用相应的Application.XXXPath
		/// </summary>
		/// <param name="originalPath"></param>
		/// <returns></returns>
		public string GetFileAbsolutePath(string originalPath)
		{
			return string.Empty;
		}


	}
}
