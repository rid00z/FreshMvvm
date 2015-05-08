using Xamarin.Forms;
using PropertyChanged;
using FreshMvvm;

namespace FreshMvvmSampleApp
{
	[ImplementPropertyChanged]
	public class ContactPageModel : FreshBasePageModel
	{
		IDatabaseService _dataService;

		public ContactPageModel (IDatabaseService dataService)
		{
			_dataService = dataService;
		}

		public Contact Contact { get; set; }

		public override void Init (object initData)
		{
			if (initData != null) {
				Contact = (Contact)initData;
			} else {
				Contact = new Contact ();
			}
		}

		public Command SaveCommand
		{
			get 
			{ 
				return new Command (() =>
					{
						_dataService.UpdateContact(Contact);
                        CoreMethods.PopPageModel();
					}
				);
			}
		}

		public Command TestModal
		{
			get {
				return new Command (async () => {
                    await CoreMethods.PushPageModel<ModalPageModel>(null, true);
				});
			}
		}
	}
}

