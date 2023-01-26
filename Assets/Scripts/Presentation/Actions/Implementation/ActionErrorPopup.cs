using Core.Configs.Actions.Implementation;
using Core.Models.Enums;
using TMPro;
using UnityEngine;

namespace Presentation.Actions.Implementation
{
	internal class ActionErrorPopup : MonoBehaviour
	{
		[SerializeField] private ActionConfigs m_ActionConfigs;
		[SerializeField] private TMP_Text m_Text;

		public void Show(ActionResult error)
		{
			gameObject.SetActive(true);
			m_Text.text = m_ActionConfigs.GetResultText(error);
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}
	}
}