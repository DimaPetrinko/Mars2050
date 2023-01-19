using Core.Models.GameProcess;

namespace Presentation.GameProcess.Implementation
{
	internal class DicePresenter : IDicePresenter
	{
		public IDice Model { get; }
		public IDiceView View { get; }

		public DicePresenter(IDice model, IDiceView view)
		{
			Model = model;
			View = view;

			Model.Rolled += OnRolled;
		}

		public void Initialize()
		{
			View.Active = false;
		}

		private void OnRolled(byte value)
		{
			View.Roll = value;
		}
	}
}