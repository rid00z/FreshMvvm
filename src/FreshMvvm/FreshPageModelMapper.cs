using System;

namespace FreshMvvm
{
    public class FreshPageModelMapper : IFreshPageModelMapper
    {
        public string GetPageTypeName(Type pageModelType)
        {
            return pageModelType.AssemblyQualifiedName
                .Replace ("PageModel", "Page")
                .Replace ("ViewModel", "View");
			//Changed for my implementation. Reference https://github.com/rid00z/FreshMvvm/issues/72
		}
    }
}

