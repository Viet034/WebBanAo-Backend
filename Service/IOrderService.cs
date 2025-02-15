using WebBanAoo.Models.DTO.Response;
using static WebBanAoo.Models.Status.Status;
using WebBanAoo.Models.DTO.Request.Order;

namespace WebBanAoo.Service
{
    public interface IOrderService
    {
        public Task<IEnumerable<OrderResponse>> GetAllOrderAsync();
        public Task<IEnumerable<OrderResponse>> SearchOrderByKeyAsync(string key);
        public Task<OrderResponse> UpdateOrderAsync(int id, OrderUpdate update);
        public Task<OrderResponse> CreateOrderAsync(OrderCreate create);
        public Task<bool> HardDeleteOrderAsync(int id);
        public Task<OrderResponse> SoftDeleteOrderAsync(int id, OrderStatus newStatus);
        public Task<OrderResponse> FindOrderByIdAsync(int id);
        public Task<string> CheckUniqueCodeAsync();
    }
}
