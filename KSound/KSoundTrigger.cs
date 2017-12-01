using UnityEngine;
using System.Collections;

namespace KCore
{

	/// <summary>
	/// 要挂载AudioSource并实现播放控制的,都使用此组件,此组件要求只与一个AudioSource来对应,要使用多个AudioSource,需要使用多个GameObject
	/// 此组件能播放2D/3D音频
	/// </summary>
	[DisallowMultipleComponent]
	[RequireComponent(typeof(AudioSource))]
	public sealed class KSoundTrigger : MonoBehaviour
	{

		private AudioSource audioSource = null;

		void Awake()
		{
			audioSource = transform.GetComponent<AudioSource>();
		}
		
		// Update is called once per frame
		void Update()
		{

		}
	}

}
