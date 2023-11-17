using MyUnit.Exeption;
using MyUnit.Extensions;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace MyUnit.Attributes
{
    public class BlendAttributeException : Exception
    {
        public BlendAttributeException(string message) : base(message) { }
    }
    public static class MyTestRunner
    {
        delegate void Logic(object attribute, MethodInfo method, object instance, out string text);
        delegate bool IsMyAttribute(MethodInfo method);
        public static event Action<string, string>? TestFailed;
        public static event Action<string>? TestPassed;

        public static void MyFuct(object attribute, MethodInfo method, object instance, out string text)
        {
            text = method.CreateDescription();
            method.Invoke(instance, Array.Empty<object>());
        }
        public static void MyInlineData(object attribute, MethodInfo method, object instance, out string text)
        {
            string stringArgs = "";
            object[]? args = null;
            if (attribute != null)
            {
                args = ((MyInlineDataAttribute)attribute).Arguments;
                stringArgs = String.Join(", ", args);
            }

            text = method.CreateDescription(stringArgs);
            method.Invoke(instance, args);
            
        }
        
        public static bool CheckAtrribute<T>(MethodInfo method)
        {
            return method.GetCustomAttributes(false).Any(o => o.GetType() == typeof(T));
        }
        private static void CheckAndCatch(
            object? attribute, 
            MethodInfo method, 
            Logic logic, 
            object instance, 
            Exception? err)
        {
            string testName = "";
            try
            {
                if (err != null) { throw err; }
                logic(attribute, method, instance, out testName);

                TestPassed?.Invoke(testName);
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException is TestFailureExeption) TestFailed?.Invoke(testName, ex.InnerException.Message);
            }
            catch (TestFailureExeption ex) {
                throw new Exception(testName, ex);
            }
            catch (TargetParameterCountException ex)
            {
                throw new Exception($"{testName} количество атрибутов и принимаемых параметров не соответствует друг другу!", ex);
            }
            catch (ArgumentException ex)
            {
                throw new Exception($"{testName}не соответствуют принимаемым параметрам!", ex);
            }
            catch (BlendAttributeException ex)
            {
                throw new Exception($"{testName} Смешивание атрибутов!", ex);
            }
        }
        private static void RunTestMethod(MethodInfo method)
        {
            Exception? err = null;
            var factAttribute = CheckAtrribute<MyFactAttribute>(method);
            var inlineDataAttribute = CheckAtrribute<MyInlineDataAttribute>(method);
            
            var instance = Activator.CreateInstance(method.DeclaringType);

            if (factAttribute && inlineDataAttribute) err = new BlendAttributeException("");
            

            if (factAttribute) 
                CheckAndCatch(null, method, MyFuct, instance, err);

            if (inlineDataAttribute)
                foreach (var attribute in method.GetCustomAttributes(typeof(MyInlineDataAttribute)))
                {
                    CheckAndCatch(attribute, method, MyInlineData, instance, err);
                }
        }
        
        public static void Run(Type type)
        {
            var methods = type.GetMethods();
            foreach (var method in methods)
            {
                RunTestMethod(method);
            }

        }
    }


    [AttributeUsage(AttributeTargets.Method)]
    public class MyFactAttribute : Attribute { }


    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class MyInlineDataAttribute : Attribute
    {
        public object[] Arguments { get; }
       public MyInlineDataAttribute(params object[] arguments) => Arguments = arguments;

    }
    public class MyAssert
    {
        public static void Equal<T>(T arg1, T arg2)
        {
            if (arg1?.Equals(arg2) == false) { throw new TestFailureExeption($"Ожидали {arg1}, а получили {arg2}"); }
        }

        public static void Throws<TException>(Action action) where TException : Exception
        {
            try
            {
                action?.Invoke();
                throw new TestFailureExeption("Не соблюдены условия исключения!" );
            } catch (TException)
            {

            } catch (TestFailureExeption e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                throw new TestFailureExeption($"Ожидалось исключение ${typeof(TException).Name}, а пришло исключение ${ex.GetType().Name}");
            }


        }
        
        public static void False(bool condition)
        {
            if (condition)  {
                throw new TestFailureExeption("Операция не ложна!");
            }
        }

        public static void True(bool condition)
        {
            if (!condition)
            {
                throw new TestFailureExeption("Операция не истина!");
            }
        }
    }

}