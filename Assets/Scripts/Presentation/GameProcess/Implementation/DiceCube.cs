using System;
using UnityEngine;

namespace Presentation.GameProcess.Implementation
{
	[RequireComponent(typeof(Animator))]
	internal class DiceCube : MonoBehaviour
	{
		[SerializeField] private Transform m_Cube;
		[SerializeField] private Vector3[] m_Orientations;

		private static readonly int ShownParameter = Animator.StringToHash("Shown");
		private static readonly int SkipTrigger = Animator.StringToHash("Skip");

		private Animator mAnimator;
		private Action mRolledCallback;
		private Action mCompletedCallback;

		public void PlayAnimation(byte roll, Action onCompleted)
		{
			gameObject.SetActive(true);
			mRolledCallback = onCompleted;
			m_Cube.rotation = GetRotationForRoll(roll);
			mAnimator.ResetTrigger(SkipTrigger);
			mAnimator.SetBool(ShownParameter, true);
		}

		public void Hide(Action onCompleted)
		{
			mCompletedCallback = onCompleted;
			mAnimator.ResetTrigger(SkipTrigger);
			mAnimator.SetBool(ShownParameter, false);
		}

		public void Skip()
		{
			mAnimator.SetTrigger(SkipTrigger);
		}

		private void Awake()
		{
			mAnimator = GetComponent<Animator>();
			gameObject.SetActive(false);
		}

		private Quaternion GetRotationForRoll(byte roll)
		{
			return Quaternion.Euler(m_Orientations[roll - 1]);
		}

		#region AnimationEvents
		public void OnRollCompleted()
		{
			mRolledCallback?.Invoke();
		}

		public void OnHideCompleted()
		{
			mCompletedCallback?.Invoke();
			gameObject.SetActive(false);
		}
		#endregion
	}
}