using System;

namespace FreshMvvm
{
    public interface IFreshPageModelMapper
    {
        string GetPageTypeName(Type pageModelType);
    }
}

