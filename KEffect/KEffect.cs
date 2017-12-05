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

		bool isLoop = false;
		float maxDuration = 0f;

		ParticleSystem[] particleSystemArray = eff.GetComponentsInChildren<ParticleSystem>(false); //inactive的不参与处理
		for (int i = 0; i < particleSystemArray.Length; i++)
		{
			isLoop = isLoop || particleSystemArray[i].loop;
			if (particleSystemArray[i].duration > maxDuration)
			{
				maxDuration = particleSystemArray[i].duration;
			}
		}

		if (!isLoop)
		{
			//非循环的特效才添加
			KEffectControl effControl = eff.gameObject.AddComponent<KEffectControl>();
			effControl.duration = maxDuration;
		}
		
		return eff;
	}
	
}
