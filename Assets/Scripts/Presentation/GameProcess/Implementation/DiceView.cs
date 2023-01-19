using UnityEngine;
using UnityEngine.UI;

namespace Presentation.GameProcess.Implementation
{
	internal class DiceView : MonoBehaviour, IDiceView
	{
		[SerializeField] private DiceCube m_DiceCube;
		[SerializeField] private Button m_OkButton;
		[SerializeField] private Button m_SkipButton;

		public bool Active
		{
			set
			{
				m_OkButton.gameObject.SetActive(false);
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
			m_OkButton.onClick.AddListener(OnOkButtonClicked);
			m_SkipButton.onClick.AddListener(OnSkipButtonClicked);
		}

		private void PlayRollAnimation(byte roll)
		{
			m_DiceCube.PlayAnimation(roll, () => m_OkButton.gameObject.SetActive(true));
		}

		private void OnOkButtonClicked()
		{
			m_OkButton.gameObject.SetActive(false);
			m_DiceCube.Hide(() => Active = false);
		}

		private void OnSkipButtonClicked()
		{
			m_DiceCube.Skip();
		}
	}
}