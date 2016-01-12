using System;
using Xamarin.Forms;

namespace FreshMvvm
{
    public static class PageExtensions
    {
        public static FreshBasePageModel GetModel(this Page page)
        {
            var pageModel = page.BindingContext as FreshBasePageModel;
            if (pageModel == null)
                throw new Exception ("BindingContext was not a FreshBasePageModel on this Page");
            return pageModel;
        }
    }
}

