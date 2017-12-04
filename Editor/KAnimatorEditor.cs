using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Animator))]
public class KAnimatorEditor : Editor {

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (Application.isPlaying)
		{
			Animator ani = target as Animator;
			if (ani.isInitialized)
			{
				foreach (var item in ani.runtimeAnimatorController.animationClips)
				{
					if (GUILayout.Button("播放|动作|" + item.name))
					{
						ani.CrossFade(item.name, 0f);
					}
				}
			}
		}
	}

}
