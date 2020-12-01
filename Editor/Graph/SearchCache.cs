using System;
using RedOwl.UIX.Engine;

namespace RedOwl.UIX.Editor
{
    public interface ISearchProvider
    {
        
    }
    
    public class SearchReflection : ITypeStorage
    {
        public bool ShouldCache(Type type)
        {
            return true;
        }
    }
    

    public static partial class Reflector
    {
        private static readonly TypeCache<ISearchProvider, SearchReflection> SearchCache = new TypeCache<ISearchProvider, SearchReflection>();
    }
}