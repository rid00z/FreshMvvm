using System;
using Xamarin.Forms;

namespace FreshMvvm
{
    public static class FreshPageModelResolver
    {
        private static bool _isPageModelMapperFound;

        private static IFreshPageModelMapper _pageModelMapper;

        public static IFreshPageModelMapper PageModelMapper
        {
            get { return _pageModelMapper; }

            set
            {
                _pageModelMapper = value;
                _isPageModelMapperFound = true;
            }
        }

        public static Page ResolvePageModel<T> () where T : FreshBasePageModel
        {
            return ResolvePageModel<T> (null);
        }

        public static Page ResolvePageModel<T> (object initData) where T : FreshBasePageModel
        {
            var pageModel = FreshIOC.Container.Resolve<T> ();

            return ResolvePageModel<T> (initData, pageModel);
        }

        public static Page ResolvePageModel<T> (object data, T pageModel) where T : FreshBasePageModel
        {
            var type = pageModel.GetType ();
            return ResolvePageModel (type, data, pageModel);
        }

        public static Page ResolvePageModel (Type type, object data) 
        {
            var pageModel = FreshIOC.Container.Resolve (type) as FreshBasePageModel;
            return ResolvePageModel (type, data, pageModel);
        }

        public static Page ResolvePageModel (Type type, object data, FreshBasePageModel pageModel)
        {
            Type pageType;

            if (_isPageModelMapperFound == false)
                pageType = FindPageModelMapper(type);
            else
                pageType = ResolvePageType(_pageModelMapper, type);

            var page = (Page)FreshIOC.Container.Resolve (pageType);

            BindingPageModel(data, page, pageModel);

            return page;
        }

        private static Type FindPageModelMapper(Type type)
        {
            string exceptionMessages = string.Empty;
            Type pageType;
            var pModelMapper = new FreshPageModelMapper();
            try
            {
                pageType = ResolvePageType(pModelMapper, type);
                PageModelMapper = pModelMapper;
                return pageType;
            }
            catch(Exception e)
            {
                exceptionMessages += e.Message;
            }

            var vModelMapper = new FreshViewModelMapper();
            try
            {
                pageType = ResolvePageType(vModelMapper, type);
                PageModelMapper = vModelMapper;
                return pageType;
            }
            catch(Exception e)
            {
                exceptionMessages += string.Format(", {0}", e.Message);
                throw new Exception(exceptionMessages);
            }
        }

        private static Type ResolvePageType(IFreshPageModelMapper pageModelMapper, Type type)
        {
            var name = pageModelMapper.GetPageTypeName(type);
            var pageType = Type.GetType(name);

            if (pageType == null)
                throw new Exception(string.Format("Type not found: [{0}]", name));

            if (_isPageModelMapperFound == false)
                _isPageModelMapperFound = true;

            return pageType;
        }

        public static Page BindingPageModel(object data, Page targetPage, FreshBasePageModel pageModel)
        {
            pageModel.WireEvents (targetPage);
            pageModel.CurrentPage = targetPage;
            pageModel.CoreMethods = new PageModelCoreMethods (targetPage, pageModel);
            pageModel.Init (data);
            targetPage.BindingContext = pageModel;
            return targetPage;
        }            
    }
}

