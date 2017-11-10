using UnityEngine;

namespace KCore
{

	public abstract class KUIObject : MonoBehaviour {

		protected Transform tran;

		public abstract void Awake();

		/// <summary>
		/// 子类实现的初始化组件
		/// </summary>
		protected abstract void InitializeComponent();

		/// <summary>
		/// 子类实现的绑定事件方法
		/// </summary>
		protected abstract void BindEvent();

		public abstract void SetContent(object data);

		protected abstract void SetData(object data);

		/// <summary>
		/// 用于Panel的显示
		/// </summary>
		public abstract void Show();

		/// <summary>
		/// 用于Panel的隐藏
		/// </summary>
		public abstract void Hide();

		/// <summary>
		/// 销毁GameObject
		/// </summary>
		public abstract void Close();

		/// <summary>
		/// 返回TRUE允许销毁GameObject
		/// </summary>
		protected abstract bool OnClosing();
		
	}

}
