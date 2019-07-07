using System;
using Xamarin.Forms;

namespace FreshMvvm.Tests
{
    public static class HelperExtensions
    {
        public static Page GetPageFromNav(this Page page)
        {
            return (page as NavigationPage).CurrentPage;
        }
    }
}

