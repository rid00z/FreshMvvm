using System;

namespace FreshMvvm
{
    public class FreshViewModelMapper : IFreshPageModelMapper
    {
        public string GetPageTypeName(Type pageModelType)
        {
            return pageModelType.AssemblyQualifiedName
                .Replace ("PageModel", "View")
                .Replace ("ViewModel", "View");
        }
    }
}

