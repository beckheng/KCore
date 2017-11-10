using UnityEngine;

namespace KCore
{

	public abstract class KUIPanel : KUIObject {

		public sealed override void Awake()
		{
			tran = transform;

			InitializeComponent();
		}

		public sealed override void SetContent(object data)
		{
			SetData(data);
		}

		public sealed override void Show()
		{
			tran.gameObject.SetActive(true);
		}

		public sealed override void Hide()
		{
			tran.gameObject.SetActive(false);
		}

		public sealed override void Close()
		{
			if (OnClosing())
			{
				Destroy(tran.gameObject);
			}
		}

	}

}
