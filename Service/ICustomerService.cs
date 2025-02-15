using WebBanAoo.Models.DTO.Request.Customer;
using WebBanAoo.Models.DTO.Response;
using static WebBanAoo.Models.Status.Status;
namespace WebBanAoo.Service
{
    public interface ICustomerService
    {
        public Task<IEnumerable<CustomerResponse>> GetAllCustomerAsync();
        public Task<IEnumerable<CustomerResponse>> SearchCustomerByKeyAsync(string key);
        public Task<CustomerResponse> UpdateCustomerAsync(int id, CustomerUpdate update);
        public Task<CustomerResponse> CreateCustomerAsync(CustomerCreate create);
        public Task<bool> HardDeleteCustomerAsync(int id);
        public Task<CustomerResponse> SoftDeleteCustomerAsync(int id, CustomerStatus newStatus);
        public Task<CustomerResponse> ChangeGenderAsync(int id, Gender newStatus);
        public Task<CustomerResponse> FindCustomerByIdAsync(int id);
        public Task<string> CheckUniqueCodeAsync();
    }
}
