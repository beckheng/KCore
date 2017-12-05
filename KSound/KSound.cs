using UnityEngine;
using System.Collections;

namespace KCore
{

	/// <summary>
	/// 统一的音效控制,使用前记得调用KSound.Init初始化
	/// 这里播放的是2D音频,3D音频要使用KSoundTrigger播放
	/// </summary>
	[DisallowMultipleComponent]
	public sealed class KSound : MonoBehaviour
	{

		/// <summary>
		/// 单例
		/// </summary>
		private static KSound self = null;

		/// <summary>
		/// 用于播放背景音乐
		/// </summary>
		private AudioSource bgAudioSource = null;

		/// <summary>
		/// ARGUS: 用于播放一次性音效,暂时先添加,实际上看看后继制作上,有没有这个需要
		/// </summary>
		private AudioSource oneShotAudioSource = null;

		void Awake()
		{
			// 加载组件
			bgAudioSource = gameObject.AddComponent<AudioSource>();
			bgAudioSource.loop = true;

			oneShotAudioSource = gameObject.AddComponent<AudioSource>();
		}

		/// <summary>
		/// 实例化单例,挂在一个GO上
		/// </summary>
		public static void Init()
		{
			if (self == null)
			{
				GameObject go = new GameObject("__KSound__");
				DontDestroyOnLoad(go);
				self = go.AddComponent<KSound>();
				if (self == null)
				{
					Debug.LogError("KSound|Init|error");
				}
			}
		}


		/// <summary>
		/// 调用bgAudioSource的Play播放音频文件,,直接使用已经预加载的AB资源
		/// </summary>
		/// <param name="volumeScale">音量缩放比</param>
		public static void PlayBg(string audioABName, float volumeScale = 1.0f)
		{
			KSound.PlayBg(KAssetBundle.GetObject<AudioClip>(audioABName), volumeScale);
		}

		/// <summary>
		/// 调用bgAudioSource的Play播放音频文件
		/// </summary>
		/// <param name="volumeScale">音量缩放比</param>
		private static void PlayBg(AudioClip audioClip, float volumeScale = 1.0f)
		{
			if (self.bgAudioSource.clip != null)
			{
				//先释放旧的资源
				self.bgAudioSource.clip = null;
			}

			self.bgAudioSource.clip = audioClip;
			self.bgAudioSource.volume = 1.0f * volumeScale;
			self.bgAudioSource.Play();
		}

		/// <summary>
		/// 调用oneShotAudioSource的PlayOneShot播放音频文件,直接使用已经预加载的AB资源
		/// </summary>
		/// <param name="volumeScale">音量缩放比</param>
		public static void PlayOneShot(string audioABName, float volumeScale = 1.0f)
		{
			PlayOneShot(KAssetBundle.GetObject<AudioClip>(audioABName), volumeScale);
		}

		/// <summary>
		/// 调用oneShotAudioSource的PlayOneShot播放音频文件,游戏逻辑中请使用PlayOneShot(string)方法
		/// 似乎在场景切换的时候,未播放完的音频也会被中断哦
		/// </summary>
		/// <param name="volumeScale">音量缩放比</param>
		private static void PlayOneShot(AudioClip audioClip, float volumeScale = 1.0f)
		{
			self.oneShotAudioSource.PlayOneShot(audioClip, volumeScale);
		}
		
	}

}
