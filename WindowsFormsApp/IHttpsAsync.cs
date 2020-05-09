using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp
{
    public interface IHttpsAsync<T>
    {
        void Post(T entity, string baseUrl);

        Task<string> PostAdd(T entity, string baseUrl);

        Task<T> PostAddModelReturn(T entity, string baseUrl);

        Task<List<T>> GetAll(string baseUrl);
    }
}