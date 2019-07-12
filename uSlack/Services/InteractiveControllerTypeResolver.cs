using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dispatcher;

namespace uSlack.Services
{
    internal class InteractiveControllerTypeResolver : IInteractiveControllerTypeResolver
    {
        private Func<Assembly, Type[]> _getTypesFunc = new Func<Assembly, Type[]>(InteractiveControllerTypeResolver.GetTypes);


        public virtual ICollection<Type> GetControllerTypes(
            ICollection<Assembly> assemblies)
        {

            List<Type> typeList = new List<Type>();
            foreach (Assembly assembly in assemblies)
            {
                if (!(assembly == (Assembly)null))
                {
                    if (!assembly.IsDynamic)
                    {
                        Type[] typeArray;
                        try
                        {
                            typeArray = this._getTypesFunc(assembly);
                        }
                        catch (ReflectionTypeLoadException ex)
                        {
                            typeArray = ex.Types;
                        }
                        catch
                        {
                            continue;
                        }
                        if (typeArray != null)
                            typeList.AddRange(((IEnumerable<Type>)typeArray).Where<Type>((Func<Type, bool>)(x =>
                            {
                                if (this.TypeIsVisible(x))
                                    return this.IsControllerTypePredicate(x);
                                return false;
                            })));
                    }
                }
            }
            return (ICollection<Type>)typeList;
        }

        internal static bool HasValidControllerName(Type controllerType)
        {
            string controllerSuffix = InteractiveControllerSelector.ControllerSuffix;
            if (controllerType.Name.Length > controllerSuffix.Length)
                return controllerType.Name.EndsWith(controllerSuffix, StringComparison.OrdinalIgnoreCase);
            return false;
        }

        internal static Type[] GetTypes(Assembly assembly)
        {
            return assembly.GetTypes();
        }

        private bool TypeIsVisible(Type type)
        {
            if (type != (Type)null)
                return type.IsVisible;
            return false;
        }

    }
}
