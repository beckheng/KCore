using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace KCore
{

	/// <summary>
	/// 继承于MonoBehaviour,一些继承于MonoBehaviour的Component优先使用这个类,会有一些最佳实践封装一下
	/// </summary>
	public abstract class KBehaviour : MonoBehaviour
	{

		protected Transform tran = null;
		
		protected void Awake()
		{
			tran = this.transform;
			OnAwake();
		}

		/// <summary>
		/// 用于继承类Awake()时调用
		/// </summary>
		protected abstract void OnAwake();
	}

}
