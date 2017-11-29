using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class KUIExt {

	/// <summary>
	/// 
	/// </summary>
	/// <param name="self"></param>
	/// <param name="name">transform.FindChild的形式</param>
	public static T GetComponentByName<T>(this Component self, string name)
	{
		if (self == null)
		{
			Debug.LogError("GetComponentByName|self=null|name=" + name);
			return default(T);
		}

		Transform tran = self.transform.FindChild(name);
		if (tran == null)
		{
			Debug.LogError("GetComponentByName|cannot find child|name=" + name);
			return default(T);
		}

		T comp = tran.GetComponent<T>();
		if (comp == null)
		{
			Debug.LogError("GetComponentByName|name=" + name);
		}

		return comp;
	}

	/// <summary>
	/// 为Button添加onClick事件响应
	/// </summary>
	public static void AddClickListener(this Button self, UnityAction call)
	{
		self.onClick.AddListener(call);
	}


}
