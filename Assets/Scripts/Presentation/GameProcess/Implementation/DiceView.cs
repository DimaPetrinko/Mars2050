using UnityEngine;
using UnityEngine.UI;

namespace Presentation.GameProcess.Implementation
{
	internal class DiceView : MonoBehaviour, IDiceView
	{
		[SerializeField] private DiceCube m_DiceCube;
		[SerializeField] private Button m_Button;

		private bool mRolled;

		public bool Active
		{
			set
			{
				mRolled = false;
				gameObject.SetActive(value);
			}
		}

		public byte Roll
		{
			set
			{
				Active = true;
				PlayRollAnimation(value);
			}
		}

		private void Awake()
		{
			m_Button.onClick.AddListener(OnButtonClicked);
		}

		private void PlayRollAnimation(byte roll)
		{
			m_DiceCube.PlayAnimation(roll, () => mRolled = true);
		}

		private void OnOkButtonClicked()
		{
			mRolled = false;
			m_DiceCube.Hide(() => Active = false);
		}

		private void OnButtonClicked()
		{
			if (mRolled) OnOkButtonClicked();
			else m_DiceCube.Skip();
		}
	}
}