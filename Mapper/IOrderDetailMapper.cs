using WebBanAoo.Models.DTO.Response;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.OrderDetail;

namespace WebBanAoo.Mapper
{
    public interface IOrderDetailMapper
    {
        // request => Entity(DTO)
        OrderDetail CreateToEntity(OrderDetailCreate create);
        OrderDetail UpdateToEntity(OrderDetailUpdate update);
        OrderDetail DeleteToEntity(OrderDetailDelete delete);

        // Entity(DTO) => Response
        OrderDetailResponse EntityToResponse(OrderDetail entity);
        IEnumerable<OrderDetailResponse> ListEntityToResponse(IEnumerable<OrderDetail> entities);
    }
}
