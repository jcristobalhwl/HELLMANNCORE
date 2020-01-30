using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IServiceBase<T>
    {
        ResponseBase<T> GetList();
        ResponseBase<T> Get(int ID);
        ResponseBase<T> Insert(T model);
        ResponseBase<T> Update(T model);
        ResponseBase<T> Delete(int ID);
    }
}
