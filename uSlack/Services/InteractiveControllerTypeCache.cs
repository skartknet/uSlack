using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dispatcher;

namespace uSlack.Services
{
    internal sealed class InteractiveControllerTypeCache
    {
        private readonly IInteractiveControllerTypeResolver _typeResolver;

        private readonly Lazy<Dictionary<string, ILookup<string, Type>>> _cache;

        public InteractiveControllerTypeCache(IInteractiveControllerTypeResolver typeResolver)
        {
            _typeResolver = typeResolver;
            this._cache = new Lazy<Dictionary<string, ILookup<string, Type>>>(new Func<Dictionary<string, ILookup<string, Type>>>(this.InitializeCache));
        }

        internal Dictionary<string, ILookup<string, Type>> Cache
        {
            get
            {
                return this._cache.Value;
            }
        }

        public ICollection<Type> GetControllerTypes(string controllerName)
        {
            HashSet<Type> typeSet = new HashSet<Type>();
            ILookup<string, Type> lookup;
            if (this._cache.Value.TryGetValue(controllerName, out lookup))
            {
                foreach (IGrouping<string, Type> grouping in (IEnumerable<IGrouping<string, Type>>)lookup)
                    typeSet.UnionWith((IEnumerable<Type>)grouping);
            }
            return (ICollection<Type>)typeSet;
        }

        private Dictionary<string, ILookup<string, Type>> InitializeCache()
        {
            return this._typeResolver.GetControllerTypes(this.GetAssemblies()).GroupBy<Type, string>((Func<Type, string>)(t => t.Name.Substring(0, t.Name.Length - DefaultHttpControllerSelector.ControllerSuffix.Length)), (IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase).ToDictionary<IGrouping<string, Type>, string, ILookup<string, Type>>((Func<IGrouping<string, Type>, string>)(g => g.Key), (Func<IGrouping<string, Type>, ILookup<string, Type>>)(g => g.ToLookup<Type, string>((Func<Type, string>)(t => t.Namespace ?? string.Empty), (IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase)), (IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns a list of assemblies available for the application.
        /// </summary>
        /// <returns>A <see cref="Collection{T}"/> of assemblies.</returns>
        private ICollection<Assembly> GetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies().ToList();
        }

    }
}
