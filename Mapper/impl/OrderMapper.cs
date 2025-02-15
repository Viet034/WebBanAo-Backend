using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Order;
using WebBanAoo.Models.DTO.Response;

namespace WebBanAoo.Mapper.impl
{
    public class OrderMapper : IOrderMapper
    {
        private readonly Order order = new Order();
        public Order CreateToEntity(OrderCreate create)
        {
            order.Code = create.Code;
            order.Status = create.Status;
            order.InitialTotalAmount = create.InitialTotalAmount;
            order.TotalAmount = create.TotalAmount;
            order.Note = create.Note;
            order.OrderDate = DateTime.Now.AddHours(7);         
            order.CreatedBy = "System";
            order.CreateDate = DateTime.Now.AddHours(7);
            order.UpdateDate = DateTime.Now.AddHours(7);
            order.UpdateBy = "System";
            return order;
        }

        public Order DeleteToEntity(OrderDelete delete)
        {
            order.Id = delete.Id;
            order.Code = delete.Code;
            order.Status = delete.Status;
            order.InitialTotalAmount = delete.InitialTotalAmount;
            order.TotalAmount = delete.TotalAmount;
            order.Note = delete.Note;
            order.OrderDate = DateTime.Now.AddHours(7);
            order.CreatedBy = "System";
            order.CreateDate = DateTime.Now.AddHours(7);
            order.UpdateDate = DateTime.Now.AddHours(7);
            order.UpdateBy = "System";
            return order;
        }

        public OrderResponse EntityToResponse(Order entity)
        {
            OrderResponse response = new OrderResponse();
            response.Id = entity.Id;
            response.Code = entity.Code;
            response.Status = entity.Status;
            response.InitialTotalAmount = entity.InitialTotalAmount;
            response.TotalAmount = entity.TotalAmount;
            response.Note = entity.Note;
            order.OrderDate = DateTime.Now.AddHours(7);
            order.CreatedBy = "System";
            return response;
        }

        public IEnumerable<OrderResponse> ListEntityToResponse(IEnumerable<Order> entities)
        {
            return entities.Select(x => EntityToResponse(x)).ToList();
        }

        public Order UpdateToEntity(OrderUpdate update)
        {
            order.Code = update.Code;
            order.Status = update.Status;
            order.InitialTotalAmount = update.InitialTotalAmount;
            order.TotalAmount = update.TotalAmount;
            order.Note = update.Note;
            order.OrderDate = DateTime.Now.AddHours(7);
            order.CreatedBy = "System";
            order.CreateDate = DateTime.Now.AddHours(7);
            order.UpdateDate = DateTime.Now.AddHours(7);
            order.UpdateBy = "System";
            return order;
        }
    }
}
