namespace FreshMvvm.Tests
{
	public class MockFreshBasePageModel : FreshBasePageModel
	{
		private string _name;

		public string Name
		{
			get
			{
				return _name;
			}

			set
			{
				_name = value;
				RaisePropertyChanged();
			}
		}

		public override void Init(object initData)
		{
			base.Init(initData);

			Name = initData as string;
		}

		public override void ReverseInit(object returndData)
		{
			base.ReverseInit(returndData);

			Name = returndData as string;
		}

		public MockFreshBasePageModel()
		{
		}
	}
}

