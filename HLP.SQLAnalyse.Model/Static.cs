using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HLP.SQLAnalyse.Model
{
    public class Static
    {
        public static object ExecuteMethodByReflection(string xNamespace, string xType, string xMethod, object[] parameters)
        {
            Type t = GetTypeByReflection(xNamespace: xNamespace, xType: xType);

            if (t != null)
            {
                MethodInfo method = t.GetMethod(name: xMethod);

                if (method != null)
                    return method.Invoke(obj: t, parameters: parameters);
            }

            return null;
        }
        public static Type GetTypeByReflection(string xNamespace, string xType)
        {
            Type t = null;

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            if (assemblies != null)
            {
                Assembly selectedAssembly = assemblies.ToList().FirstOrDefault(
                    i => i.ManifestModule.Name.Equals(value: xNamespace));

                if (selectedAssembly != null)
                    t = selectedAssembly.GetTypes().FirstOrDefault(i => i.Name == xType);
            }

            return t;
        }
    }
}
