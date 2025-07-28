using FDWS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FDWS.Services
{
    public interface IBusinessLogic
    {
        Task<object> GetHomeDataAsync();
        Task<ResponseObject> ValidateInputAsync(List<int[]> listData);
    }
}