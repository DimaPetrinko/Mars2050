using Core.Models;
using UnityEngine;

namespace Presentation.Implementation
{
	internal class CameraPresenter : ICameraPresenter
	{
		public ICamera Model { get; }
		public ICameraView View { get; }

		public CameraPresenter(ICamera model, ICameraView view)
		{
			Model = model;
			View = view;

			Model.Position.Changed += OnPositionChanged;
			Model.Zoom.Changed += OnZoomChanged;

			View.PositionDelta += OnViewPositionDelta;
			View.ZoomDelta += OnViewZoomDelta;
		}

		public void Initialize()
		{
			OnPositionChanged(Model.Position.Value);
			OnZoomChanged(Model.Zoom.Value);
		}

		private void OnPositionChanged(Vector2 value)
		{
			View.Position = value;
		}

		private void OnZoomChanged(float value)
		{
			View.Zoom = value;
		}

		private void OnViewPositionDelta(Vector2 value)
		{
			Model.Position.Value += value;
		}

		private void OnViewZoomDelta(float value)
		{
			Model.Zoom.Value += value;
		}
	}
}