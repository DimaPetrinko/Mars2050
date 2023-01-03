using Core.Models;
using Core.Utils;
using NUnit.Framework;
using UnityEngine;
using Camera = Core.Models.Implementation.Camera;

namespace Tests
{
	public class Camera_Tests
	{
		private ICamera mCamera;

		[SetUp]
		public void SetUp()
		{
			mCamera = new Camera(20, new Range<float>{From = 3, To = 9});
		}

		[Test]
		public void Position_IsSetToDefaultValue()
		{
			Assert.AreEqual(Vector2.zero, mCamera.Position.Value);
		}

		[Test]
		public void Position_DoesNotExceedRadius()
		{
			mCamera.Position.Value = new Vector2(100, 100);

			Assert.AreEqual(20, mCamera.Position.Value.magnitude);
		}

		[Test]
		public void Zoom_IsSetToMaxZoomLimit()
		{
			Assert.AreEqual(mCamera.ZoomLimits.To, mCamera.Zoom.Value);
		}

		[Test]
		public void Zoom_DoesNotExceedMaxLimit()
		{
			mCamera.Zoom.Value = 100;

			Assert.AreEqual(9, mCamera.Zoom.Value);
		}

		[Test]
		public void Zoom_DoesNotFallBeyondMinLimit()
		{
			mCamera.Zoom.Value = 0;

			Assert.AreEqual(3, mCamera.Zoom.Value);
		}

		[Test]
		public void ZoomLimits_IsSetToGivenValue()
		{
			Assert.AreEqual(new Range<float> { From = 3, To = 9 }, mCamera.ZoomLimits);
		}
	}
}