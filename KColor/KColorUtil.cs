using UnityEngine;
using System.Collections;

using KCore;
using KData;
using KScene;

namespace KCore
{

	public class KColorUtil
	{

		/// <summary>
		/// 解析颜色字符串,两种格式,抄自官方文档
		/// Strings that do not begin with '#' will be parsed as literal colors, with the following supported:
		/// red, cyan, blue, darkblue, lightblue, purple, yellow, lime, fuchsia, white, silver, grey, black, orange, brown, maroon, green, olive, navy, teal, aqua, magenta..
		/// </summary>
		public static Color ParseColor(string colorHtmlString)
		{
			Color color;

			if (ColorUtility.TryParseHtmlString(colorHtmlString, out color))
			{
				return color;
			}
			else
			{
				Debug.LogError("ParseColor|colorHtmlString|" + colorHtmlString + "|use|Color.black|instead");
				return Color.black;
			}
		}

	}

}
