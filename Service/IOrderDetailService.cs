using WebBanAoo.Models.DTO.Response;
using static WebBanAoo.Models.Status.Status;
using WebBanAoo.Models.DTO.Request.OrderDetail;

namespace WebBanAoo.Service
{
    public interface IOrderDetailService
    {
        public Task<IEnumerable<OrderDetailResponse>> GetAllOrderDetailAsync();
        public Task<IEnumerable<OrderDetailResponse>> SearchOrderDetailByCodeAsync(string key);
        public Task<OrderDetailResponse> UpdateOrderDetailAsync(int id, OrderDetailUpdate update);
        public Task<OrderDetailResponse> CreateOrderDetailAsync(OrderDetailCreate create);
        public Task<bool> HardDeleteOrderDetailAsync(int id);
        public Task<OrderDetailResponse> SoftDeleteOrderDetailAsync(int id, OrderDetailStatus newStatus);
        public Task<OrderDetailResponse> FindOrderDetailByIdAsync(int id);
        public Task<string> CheckUniqueCodeAsync();
    }
}
