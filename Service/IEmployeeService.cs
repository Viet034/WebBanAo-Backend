using WebBanAoo.Models.DTO.Response;
using static WebBanAoo.Models.Status.Status;
using WebBanAoo.Models.DTO.Request.Employee;

namespace WebBanAoo.Service
{
    public interface IEmployeeService
    {
        public Task<IEnumerable<EmployeeResponse>> GetAllEmployeeAsync();
        public Task<IEnumerable<EmployeeResponse>> SearchEmployeeByKeyAsync(string key);
        public Task<EmployeeResponse> UpdateEmployeeAsync(int id, EmployeeUpdate update);
        public Task<EmployeeResponse> CreateEmployeeAsync(EmployeeCreate create);
        public Task<bool> HardDeleteEmployeeAsync(int id);
        public Task<EmployeeResponse> SoftDeleteEmployeeAsync(int id, EmployeeStatus newStatus);
        public Task<EmployeeResponse> ChangeGenderAsync(int id, Gender newStatus);
        public Task<EmployeeResponse> FindEmployeeByIdAsync(int id);
        public Task<string> CheckUniqueCodeAsync();
    }
}
