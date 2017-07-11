using FreshMvvm.Base;
using Xamarin.Forms;

namespace FreshMvvm.Extensions
{
    public static class PageExtensions
    {
        public static FreshPageModel GetModel(this Page page)
        {
            var pageModel = page.BindingContext as FreshPageModel;
            return pageModel;
        }

        public static void NotifyAllChildrenPopped(this NavigationPage navigationPage)
        {
            foreach (var page in navigationPage.Navigation.ModalStack)
            {
                var pageModel = page.GetModel();
                pageModel?.RaisePageWasPopped();
            }

            foreach (var page in navigationPage.Navigation.NavigationStack)
            {
                var pageModel = page.GetModel();
                pageModel?.RaisePageWasPopped();
            }
        }
    }
}

