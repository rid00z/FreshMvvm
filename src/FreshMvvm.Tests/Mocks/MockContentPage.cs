using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FreshMvvm.Tests.Mocks
{
    class MockContentPage : ContentPage
	{
        public MockContentPage(FreshBasePageModel model)
        {
            BindingContext = model;
        }
	}
}
