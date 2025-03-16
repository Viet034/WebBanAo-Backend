using WebBanAoo.Models.DTO.Response;
using static WebBanAoo.Models.Status.Status;
using WebBanAoo.Models.DTO.Request.Order;
using WebBanAoo.Models.DTO.Request.Cart;
using WebBanAoo.Models.Status;

namespace WebBanAoo.Service
{
    public interface IOrderService
    {
        public Task<IEnumerable<OrderResponse>> GetAllOrderAsync();
        public Task<IEnumerable<OrderResponse>> SearchOrderByKeyAsync(string key);
        public Task<IEnumerable<OrderResponse>> GetOrderByCustomerIdAsync(int id);
        public Task<OrderResponse> UpdateOrderAsync(int id, OrderUpdate update);
        public Task<OrderResponse> CreateOrderAsync(OrderCreate create);
        public Task<bool> HardDeleteOrderAsync(int id);
        public Task<OrderResponse> SoftDeleteOrderAsync(int id, Status.OrderStatus newStatus);
        public Task<OrderResponse> FindOrderByIdAsync(int id);
        public Task<OrderResponse> ApplyVoucherToOrderAsync(int orderId, int voucherId);
        public Task<OrderResponse> CheckoutFromCartAsync(CartCheckoutRequest request);
        public Task<OrderResponse> AssignEmployeeToOrderAsync(int orderId, int employeeId);
        public Task<IEnumerable<OrderResponse>> GetPendingOrdersAsync();
        public Task<IEnumerable<OrderResponse>> GetOrdersByEmployeeIdAsync(int employeeId);

        public Task<string> CheckUniqueCodeAsync();
    }
}
