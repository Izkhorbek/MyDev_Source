using System;
using System.Collections.Generic;
using System.Text;

namespace LibCommonDef
{
    public interface IApplication<T> where T: class
    {
        void Run(T t);
    }
}
