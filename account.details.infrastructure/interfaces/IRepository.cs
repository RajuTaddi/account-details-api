using account.details.infrastructure.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace account.details.infrastructure.interfaces
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAllById(string accountNumber);
        Task<T> GetById(string id);
        //Task<int> Update(int id, T record);
        //Task<int> Insert(T record);
    }
}
