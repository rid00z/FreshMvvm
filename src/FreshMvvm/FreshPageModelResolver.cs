using System;
using FreshMvvm.Base;
using FreshMvvm.CoreMethods;
using FreshMvvm.IoC;
using Xamarin.Forms;

namespace FreshMvvm
{
    public static class FreshPageModelResolver
    {
        public static IFreshPageModelMapper PageModelMapper { get; set; } = new FreshPageModelMapper();

        public static Page ResolvePageModel<T>() where T : FreshPageModel
        {
            return ResolvePageModel<T>(null);
        }

        public static Page ResolvePageModel<T>(object initData) where T : FreshPageModel
        {
            var pageModel = FreshIoC.Container.Resolve<T>();

            return ResolvePageModel(initData, pageModel);
        }

        public static Page ResolvePageModel<T>(object data, T pageModel) where T : FreshPageModel
        {
            var type = pageModel.GetType();
            return ResolvePageModel(type, data, pageModel);
        }

        public static Page ResolvePageModel(Type type, object data)
        {
            var pageModel = FreshIoC.Container.Resolve(type) as FreshPageModel;
            return ResolvePageModel(type, data, pageModel);
        }

        public static Page ResolvePageModel(Type type, object data, FreshPageModel pageModel)
        {
            var name = PageModelMapper.GetPageTypeName(type);
            var pageType = Type.GetType(name);
            if (pageType == null)
                throw new Exception(name + " not found");

            var page = (Page)FreshIoC.Container.Resolve(pageType);

            BindingPageModel(data, page, pageModel);

            return page;
        }

        public static Page BindingPageModel(object data, Page targetPage, FreshPageModel pageModel)
        {
            pageModel.WireEvents(targetPage);
            pageModel.CurrentPage = targetPage;
            pageModel.Navigation = new PageModelNavigation(targetPage, pageModel);
            pageModel.Notifications = new PageModelNotifications(targetPage);
            pageModel.Transactions = new PageModelTransactions(targetPage);
            pageModel.Init(data);
            targetPage.BindingContext = pageModel;
            return targetPage;
        }
    }
}

