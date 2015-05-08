using System;
using FreshMvvm;
using Xamarin.Forms;

namespace FreshMvvmSampleApp
{
	public class ModalPageModel : FreshBasePageModel
	{
		public ModalPageModel ()
		{
		}

		public Command CloseCommand
		{
			get {
				return new Command (() => {
                    CoreMethods.PopPageModel(true);
				});
			}
		}
	}
}

