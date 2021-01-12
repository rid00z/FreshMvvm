using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FreshMvvm
{
    public static class FreshPageModelResolver
    {
        public static IFreshPageModelMapper PageModelMapper { get; set; } = new FreshPageModelMapper();

        public static async Task<Page> ResolvePageModel<T> () where T : FreshBasePageModel
        {
            return await ResolvePageModel<T> (null);
        }

        public static async Task<Page> ResolvePageModel<T> (object initData) where T : FreshBasePageModel
        {
            var pageModel = FreshIOC.Container.Resolve<T> ();

            return await ResolvePageModel<T> (initData, pageModel);
        }

        public static async Task<Page> ResolvePageModel<T> (object data, T pageModel) where T : FreshBasePageModel
        {
            var type = pageModel.GetType ();
            return await ResolvePageModel (type, data, pageModel);
        }

        public static async Task<Page> ResolvePageModel (Type type, object data) 
        {
            var pageModel = FreshIOC.Container.Resolve (type) as FreshBasePageModel;
            return await  ResolvePageModel (type, data, pageModel);
        }

        public static async Task<Page> ResolvePageModel (Type type, object data, FreshBasePageModel pageModel)
        {
            var name = PageModelMapper.GetPageTypeName (type);
            var pageType = Type.GetType (name);
            if (pageType == null)
                throw new Exception (name + " not found");

            var page = (Page)FreshIOC.Container.Resolve (pageType);

            await BindingPageModel(data, page, pageModel);

            return page;
        }

        public static async Task<Page> BindingPageModel(object data, Page targetPage, FreshBasePageModel pageModel)
        {
            pageModel.WireEvents (targetPage);
            pageModel.CurrentPage = targetPage;
            pageModel.CoreMethods = new PageModelCoreMethods (targetPage, pageModel);
            pageModel.Init (data);
            await pageModel.InitAsync(data);
            targetPage.BindingContext = pageModel;
            return targetPage;
        }            
    }
}

