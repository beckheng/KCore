using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using KCore;

public class KEffect {

	/// <summary>
	/// 在预加载的AB实例化特效,可以指定parent,为方便额外的控制,返回值是Transform
	/// </summary>
	public static Transform PlayFX(string abName, Transform parent = null)
	{
		Transform eff = KAssetBundle.InstantiateEffect(abName);
		if (parent != null)
		{
			eff.SetParent(parent, false);
		}

		return eff;
	}
	
}
