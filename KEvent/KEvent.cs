using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace KCore
{

	/// <example>
	/// 调用示例
	/// <code>
	/// void Tx(TestChangePlayerEvent eventData){
	///		Debug.Log("haha");
	/// }
	/// 
	/// KEvent.Subscribe<TestChangePlayerEvent>(Tx);
	/// KEvent.Publish(new TestChangePlayerEvent() { playerName = "beckheng"});
	/// KEvent.Unsubscribe<TestChangePlayerEvent>(Tx);
	/// </code>
	/// </example>
	//-----------------

	/// <summary>
	/// 消息系统,使用发布/订阅模型
	/// 发布/订阅（Publish/subscribe 或pub/sub）是一种消息范式
	/// </summary>
	public class KEvent
	{
		/// <summary>
		/// 内部管理用的数据结构
		/// 最后要测试一下这个数据结构带来的内存消耗
		/// </summary>
		private class EventStruct
		{
			public object uniqueId; //唯一ID,用于添加,删除时的对比
			public System.Action<object> cb; //将传入的委托,转为匿名方法后保存到此,因为Delegate的调用性能开销较较较大
		}

		/// <summary>
		/// 订阅者列表
		/// </summary>
		private static Dictionary<string, List<EventStruct>> subscriberMap = new Dictionary<string, List<EventStruct>>();

		/// <summary>
		/// 获取委托方法的唯一ID,用于添加,删除时的对比
		/// </summary>
		private static object GetUniqueId<T>(System.Action<T> cb) where T : KEventData
		{
			return cb;
		}

		/// <summary>
		/// 订阅消息,要先定义消息数据结构
		/// </summary>
		public static void Subscribe<T>(System.Action<T> cb) where T : KEventData
		{
			System.Type t = typeof(T);
			string typeName = t.FullName;

			EventStruct es = new EventStruct();
			es.uniqueId = GetUniqueId<T>(cb);
			es.cb = (item) => cb((T)item);

			if (subscriberMap.ContainsKey(typeName))
			{
				//不允许重复订阅
				bool isAlreadyAdded = false;
				List<EventStruct> callbackList = subscriberMap[typeName];
				for (int i = callbackList.Count - 1; i >= 0; i--)
				{
					if (callbackList[i].uniqueId.Equals(es.uniqueId))
					{
						isAlreadyAdded = true;
						break;
					}
				}
				if (!isAlreadyAdded)
				{
					subscriberMap[typeName].Add(es);
				}
				else
				{
					Debug.LogError("not|allow|add|again|" + t + "|" + cb);
				}
			}
			else
			{
				subscriberMap.Add(typeName, new List<EventStruct>() {es});
			}
		}

		/// <summary>
		/// 取消订阅
		/// </summary>
		public static void Unsubscribe<T>(System.Action<T> cb) where T : KEventData
		{
			System.Type t = typeof(T);
			string typeName = t.FullName;

			if (subscriberMap.ContainsKey(typeName))
			{
				object uniqueId = GetUniqueId<T>(cb);
				
				List<EventStruct> callbackList = subscriberMap[typeName];
				for (int i = callbackList.Count - 1; i >= 0; i--)
				{
					if (callbackList[i].uniqueId.Equals(uniqueId))
					{
						callbackList.RemoveAt(i);
					}
				}
			}
		}

		/// <summary>
		/// 发布消息
		/// </summary>
		public static void Publish(KEventData eventData)
		{
			System.Type t = eventData.GetType();
			string typeName = t.FullName;
			
			if (subscriberMap.ContainsKey(typeName))
			{
				List<EventStruct> callbackList = subscriberMap[typeName];
				for (int i = 0; i < callbackList.Count; i++)
				{
					callbackList[i].cb(eventData);
				}
			}
		}
		
	}

}
