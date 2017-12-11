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

		/// <summary>
		/// 输入控制的数据结构
		/// </summary>
		private sealed class KInputStruct
		{
			/// <summary>
			/// 按钮回调
			/// </summary>
			public System.Action<float> cb;

			/// <summary>
			/// 有效输入的间隔
			/// </summary>
			public float interval;

			/// <summary>
			/// 上一次有效输入的时间,默认0,之后一直以Time.Time来赋值
			/// </summary>
			public float lastInputTime;

			public override string ToString()
			{
				return "|interval|" + interval + "|lastInputTime|" + lastInputTime;
			}
		}

		Dictionary<string, KInputStruct> inputCallbackMap = new Dictionary<string, KInputStruct>();
		List<string> bindNames = new List<string>();

		/// <summary>
		/// 添加输入控制的映射方法
		/// </summary>
		/// <param name="interval">有效输入的间隔,类似于AABB左左右右的未想好</param>
		public void AddControlBinding(string axisName, System.Action<float> cb, float interval = 0.2f)
		{
			if (cb == null)
			{
				Debug.LogError(Time.frameCount + "|" + this + "|AddControlBinding|no|callback|for|" + axisName);
				return;
			}

			if (string.IsNullOrEmpty(axisName))
			{
				Debug.LogError(Time.frameCount + "|" + this + "|AddControlBinding|no|axisName");
				return;
			}

			KInputStruct inputStruct = new KInputStruct();
			inputStruct.cb = cb;
			inputStruct.interval = interval;

			if (inputCallbackMap.ContainsKey(axisName))
			{
				inputCallbackMap[axisName] = inputStruct;
			}
			else
			{
				inputCallbackMap.Add(axisName, inputStruct);
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
					KInputStruct inputStruct = inputCallbackMap[bindNames[i]];

					if ((Time.time - inputStruct.lastInputTime) >= inputStruct.interval)
					{
						Debug.Log(Time.frameCount + "|" + this + "|" + bindNames[i] + "|" + Input.GetAxis(bindNames[i]) + "|inputStruct|" + inputStruct.ToString());
						inputStruct.lastInputTime = Time.time;

						inputStruct.cb(Input.GetAxis(bindNames[i]));
					}
				}
			}
		}
		
	}

}
