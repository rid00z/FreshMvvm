namespace FreshMvvm.Tests.Mocks
{
	class MockContentPageModel : MockFreshBasePageModel
	{
		public object Data { get; set; }


        public override void Init(object initData)
		{
			base.Init(initData);

		    if (initData != null)
		    {
		        Data = initData;
		    }
		}
	}
}
