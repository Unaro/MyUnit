using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyUnit.Extensions
{
    internal static class MethodNameConstructorExtension
    {
        public static string CreateDescription(this MethodInfo method, string? args = null)
        {
            var text = $"В классе {method.DeclaringType?.Name} методе {method.Name}";
            if (args != null) text += " c атрибутами: " + args;
            return text;
        }
    }
}
