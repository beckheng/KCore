using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using KData;

namespace KCore
{

	/// <summary>
	/// 设备的输入控制
	/// </summary>
	[DisallowMultipleComponent]
	public class KInputControl : MonoBehaviour
	{

		Dictionary<string, System.Action> inputCallbackMap = new Dictionary<string, System.Action>();
		List<string> bindNames = new List<string>();

		/// <summary>
		/// 添加输入控制的映射方法
		/// </summary>
		public void AddControlBinding(string axisName, System.Action cb)
		{
			if (cb == null)
			{
				Debug.LogError(Time.frameCount + "|" + this + "|AddControlBinding|no|callback|for|" + axisName);
				return;
			}

			if (inputCallbackMap.ContainsKey(axisName))
			{
				inputCallbackMap[axisName] = cb;
			}
			else
			{
				inputCallbackMap.Add(axisName, cb);
				bindNames.Add(axisName);
			}
		}

		/// <summary>
		/// 输入控制统一在这个组件来处理了
		/// </summary>
		void Update()
		{
			for (int i = 0; i < bindNames.Count; i++)
			{
				if (Input.GetAxis(bindNames[i]) != 0)
				{
					Debug.Log(Time.frameCount + "|" + this + "|" + bindNames[i]  + "|" + Input.GetAxis(bindNames[i]));
					inputCallbackMap[bindNames[i]]();
				}
			}
		}
		
	}

}
