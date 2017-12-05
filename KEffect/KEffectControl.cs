using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace KCore
{

	/// <summary>
	/// 控制特效销毁与否,如果是循环的,不会添加此组件的,由KEffect来控制添加与否
	/// </summary>
	[DisallowMultipleComponent]
	public class KEffectControl : MonoBehaviour
	{

		/// <summary>
		/// 特效的最大时值
		/// </summary>
		public float duration = 0f;

		/// <summary>
		/// 计时器
		/// </summary>
		private float timer = 0f;

		void OnEnable()
		{
			timer = 0f;
		}
		
		// Update is called once per frame
		void Update()
		{
			timer += Time.deltaTime;
			if (timer >= duration)
			{
				GameObject.Destroy(this.gameObject);
			}
		}
	}

}
