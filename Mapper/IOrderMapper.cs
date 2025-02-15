using WebBanAoo.Models.DTO.Response;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Order;

namespace WebBanAoo.Mapper;

public interface IOrderMapper
{
    // request => Entity(DTO)
    Order CreateToEntity(OrderCreate create);
    Order UpdateToEntity(OrderUpdate update);
    Order DeleteToEntity(OrderDelete delete);

    // Entity(DTO) => Response
    OrderResponse EntityToResponse(Order entity);
    IEnumerable<OrderResponse> ListEntityToResponse(IEnumerable<Order> entities);
}
