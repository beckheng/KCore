using UnityEngine;

namespace KCore
{

	public abstract class KUIWindow : KUIObject {

		/// <summary>
		/// 当前窗口的数据
		/// </summary>
		protected object curData = null;

		public sealed override void Awake()
		{
			tran = transform;

			InitializeComponent();

			BindEvent();
		}

		public sealed override void SetContent(object data)
		{
			SetData(data);
		}

		public sealed override void Show()
		{
			//nothing to do
		}

		public sealed override void Hide()
		{
			//nothing to do
		}

		public sealed override void Close()
		{
			if (OnClosing())
			{
				KUI.Close(this);
				Destroy(tran.gameObject);
			}
		}

	}

}
