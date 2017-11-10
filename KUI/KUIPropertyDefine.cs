using UnityEngine;

namespace KCore
{

	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	public class KUIPropertyDefine : MonoBehaviour
	{

		[Header("名称")]
		public string varName = string.Empty;

		[Header("类型 (留白则使用系统自定义顺序)")]
		public string varType = string.Empty;

		[Header("摘要")]
		public string varSummary = string.Empty;
		
		void Awake()
		{
		}

#if UNITY_EDITOR
		void OnEnable()
		{
			if (string.IsNullOrEmpty(varName))
			{
				varName = transform.name;
			}
		}
#endif

		void Start()
		{

		}

	}

}