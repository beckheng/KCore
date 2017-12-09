using UnityEngine;
using System.Collections;

namespace KCore
{

	/// <summary>
	/// Trigger类型的物理检测物理组件用这个接口,因为会显式要求实现
	/// </summary>
	public interface KPhysicTrigger
	{

		/// <summary>
		/// OnTriggerEnter is called when the Collider other enters the trigger.
		/// </summary>
		void OnTriggerEnter(Collider other);

		/// <summary>
		/// OnTriggerStay is called almost all the frames for every Collider other that is touching the trigger. The function is on the physics timer so it won't necessarily run every frame.
		/// </summary>
		void OnTriggerStay(Collider other);

		/// <summary>
		/// OnTriggerExit is called when the Collider other has stopped touching the trigger.
		/// </summary>
		void OnTriggerExit(Collider other);

	}

}
