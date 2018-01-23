using System;

namespace FreshMvvm
{
    public class FreshPageModelMapper : IFreshPageModelMapper
    {
        public string GetPageTypeName(Type pageModelType)
        {
            return pageModelType.AssemblyQualifiedName
                .Replace ("PageModel", "Page")
                .Replace ("ViewModel", "Page");
        }
    }
}

