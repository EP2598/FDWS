using FDWS.Models;
using System.Threading.Tasks;

namespace FDWS.Services
{
    public interface IBusinessLogic
    {
        Task<object> GetHomeDataAsync();
        Task<object> ProcessDataAsync(int id);
        Task<object> GetByIdAsync(int id);
        Task<DataProcessingModel> CreateAsync(object data);
        Task<bool> ValidateDataAsync(object data);
        Task<object> CalculateResultAsync(object input);
    }
}