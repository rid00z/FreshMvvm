using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FreshMvvm.Tests.Mocks
{
	class MockContentPageModel : MockFreshPageModel
	{
		public object Data { get; set; }

		public override void Init(object initData)
		{
			base.Init(initData);

			Data = initData;
		}
	}
}
