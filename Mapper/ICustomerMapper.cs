using WebBanAoo.Models.DTO.Response;
using WebBanAoo.Models.DTO.Request.Customer;
using WebBanAoo.Models;

namespace WebBanAoo.Mapper;

public interface ICustomerMapper
{
    // request => Entity(DTO)
    Customer CreateToEntity(CustomerCreate create);
    Customer UpdateToEntity(CustomerUpdate update);
    Customer DeleteToEntity(CustomerDelete delete);

    // Entity(DTO) => Response
    CustomerResponse EntityToResponse(Customer entity);
    IEnumerable<CustomerResponse> ListEntityToResponse(IEnumerable<Customer> entities);
}
