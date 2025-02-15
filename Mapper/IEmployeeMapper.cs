using WebBanAoo.Models.DTO.Request.Employee;
using WebBanAoo.Models.DTO.Response;
using WebBanAoo.Models;

namespace WebBanAoo.Mapper;

public interface IEmployeeMapper
{
    // request => Entity(DTO)
    Employee CreateToEntity(EmployeeCreate create);
    Employee UpdateToEntity(EmployeeUpdate update);
    Employee DeleteToEntity(EmployeeDelete delete);

    // Entity(DTO) => Response
    EmployeeResponse EntityToResponse(Employee entity);
    IEnumerable<EmployeeResponse> ListEntityToResponse(IEnumerable<Employee> entities);
}
