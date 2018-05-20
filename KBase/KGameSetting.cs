using UnityEngine;

namespace KCore
{

	public class KGameSetting : ScriptableObject
	{

		/// <summary>
		/// 游戏运行的分辨率宽度(无视横屏/竖屏)
		/// </summary>
		[Header("分辨率宽度")]
		public int resolutionWidth = 1136;

		/// <summary>
		/// 游戏运行的分辨率高度(无视横屏/竖屏)
		/// </summary>
		[Header("分辨率高度")]
		public int resolutionHeight = 640;

		/// <summary>
		/// 安卓平台运行时的帧率(FPS)
		/// </summary>
		[Header("安卓平台帧率")]
		[Range(24, 120)]
		public int androidFrameRate = 30;

		/// <summary>
		/// iOS平台运行时的帧率(FPS)
		/// </summary>
		[Header("iOS平台帧率")]
		[Range(24, 120)]
		public int iOSFrameRate = 30;

		/// <summary>
		/// 其它平台(Console,PC等)运行时的帧率(FPS)
		/// </summary>
		[Header("其它平台帧率")]
		[Range(24,120)]
		public int otherPlatformFrameRate = 60;

		/// <summary>
		/// 当图片尺寸不超过的此值的时候自动为Sprite设置packing tag,其中tag和ui名字规范有关
		/// </summary>
		[Header("自动设置图片packing tag的阀值上限")]
		[Range(32, 1024)]
		public int setPackingTagInWidth = 512;


		public static KGameSetting LoadKGameSetting()
		{
			return Resources.Load<KGameSetting>("KGameSetting");
		}

	}

}
