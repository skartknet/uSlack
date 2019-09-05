using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using uSlack.Interactive;

namespace uSlack.Services
{
    public class InteractiveControllerTypeResolver
    {
        private readonly Func<Assembly, Type[]> _getTypesFunc = new Func<Assembly, Type[]>(InteractiveControllerTypeResolver.GetTypes);

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
                                    return this.IsControllerType(x);
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

        internal bool IsControllerType(Type t)
        {
            if (t != (Type)null && t.IsClass && (t.IsVisible && !t.IsAbstract) && typeof(InteractiveApiControllerBase).IsAssignableFrom(t))
                return InteractiveControllerTypeResolver.HasValidControllerName(t);
            return false;
        }

        private bool TypeIsVisible(Type type)
        {
            if (type != (Type)null)
                return type.IsVisible;
            return false;
        }

    }
}
