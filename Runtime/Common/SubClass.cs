using System;
using System.Linq;

namespace Phobebase.Common
{
    public class SubClasses
    {
        // This is from Unity Answers but seems handy 
        // (https://answers.unity.com/questions/1779912/how-to-find-all-classes-deriving-from-a-base-class.html)

        public IEnumerable<BaseClass> GetAll()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(BaseClass)))
                .Select(type => Activator.CreateInstance(type) as BaseClass);
        }
    }
}