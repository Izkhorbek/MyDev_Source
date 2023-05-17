using System;
using System.Collections.Generic;
using System.Text;

namespace LibCommonDef
{
    public class SingletonObj<T> where T : new()
    {
        public static T _instance;
        public static T Instance => _instance ??= new T();
    }
}
