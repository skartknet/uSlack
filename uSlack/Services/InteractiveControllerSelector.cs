using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing;
using SlackAPI;
using uSlack.Services.Models;

namespace uSlack.Services
{
    public class InteractiveControllerSelector
    {
        public static readonly string ControllerSuffix = "Controller";
        private readonly InteractiveControllerTypeCache _controllerTypeCache;
        private readonly Lazy<ConcurrentDictionary<string, InteractiveControllerDescriptor>> _controllerInfoCache;

        public InteractiveControllerSelector(InteractiveControllerTypeResolver typeResolver)
        {
            _controllerInfoCache = new Lazy<ConcurrentDictionary<string, InteractiveControllerDescriptor>>(InitializeControllerInfoCache);
            _controllerTypeCache = new InteractiveControllerTypeCache(typeResolver);
        }


        public virtual InteractiveControllerDescriptor SelectController(string controller)
        {


            if (string.IsNullOrEmpty(controller))
                throw new NullReferenceException("Controller not found");

            if (this._controllerInfoCache.Value.TryGetValue(controller, out var routeController))
                return routeController;

            ICollection<Type> controllerTypes = this._controllerTypeCache.GetControllerTypes(controller);

            if (controllerTypes.Count == 0)
                throw new Exception("Controller not found");
            else
                throw new Exception("Ambiguous controller");
        }



        private ConcurrentDictionary<string, InteractiveControllerDescriptor> InitializeControllerInfoCache()
        {
            ConcurrentDictionary<string, InteractiveControllerDescriptor> concurrentDictionary = new ConcurrentDictionary<string, InteractiveControllerDescriptor>((IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase);
            HashSet<string> stringSet = new HashSet<string>();
            foreach (KeyValuePair<string, ILookup<string, Type>> keyValuePair in this._controllerTypeCache.Cache)
            {
                string key = keyValuePair.Key;
                foreach (IEnumerable<Type> types in (IEnumerable<IGrouping<string, Type>>)keyValuePair.Value)
                {
                    foreach (Type controllerType in types)
                    {
                        if (concurrentDictionary.Keys.Contains(key))
                        {
                            stringSet.Add(key);
                            break;
                        }
                        concurrentDictionary.TryAdd(key, new InteractiveControllerDescriptor(key, controllerType));
                    }
                }
            }
            foreach (string key in stringSet)
            {
                concurrentDictionary.TryRemove(key, out InteractiveControllerDescriptor controllerDescriptor);
            }
            return concurrentDictionary;
        }
    }
}
