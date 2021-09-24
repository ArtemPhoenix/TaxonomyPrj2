using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Interfaces
{
     public interface IRepository<T>  where T: class, new () 
    {
        T Get(int id);
        List<T> GetList();
    }
}
