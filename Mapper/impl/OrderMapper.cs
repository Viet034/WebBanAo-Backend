using Microsoft.AspNetCore.Http.HttpResults;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Order;
using WebBanAoo.Models.DTO.Response;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebBanAoo.Mapper.impl
{
    public class OrderMapper : IOrderMapper
    {
        
        public Order CreateToEntity(OrderCreate create)
        {
            Order order = new Order();
            order.Code = create.Code;
            order.Status = create.Status;
            order.InitialTotalAmount = create.InitialTotalAmount;
            order.TotalAmount = create.TotalAmount;
            order.Note = create.Note;
            order.CustomerId = create.CustomerId;
            order.EmployeeId = create.EmployeeId;
            order.VoucherId = create.VoucherId;
            order.OrderDate = DateTime.Now.AddHours(7);         
            order.CreatedBy = "System";
            order.CreateDate = DateTime.Now.AddHours(7);
            order.UpdateDate = DateTime.Now.AddHours(7);
            order.UpdateBy = "System";
            return order;
        }

        public Order DeleteToEntity(OrderDelete delete)
        {
            Order order = new Order();
            order.Id = delete.Id;
            order.Code = delete.Code;
            order.Status = delete.Status;
            order.InitialTotalAmount = delete.InitialTotalAmount;
            order.TotalAmount = delete.TotalAmount;
            order.Note = delete.Note;
            order.CustomerId = delete.CustomerId;
            order.EmployeeId = delete.EmployeeId;
            order.VoucherId = delete.VoucherId;
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
            response.CustomerId = entity.CustomerId;
            response.EmployeeId = entity.EmployeeId;
            response.VoucherId = entity.VoucherId;
            response.OrderDate = entity.OrderDate;
            
            return response;
        }

        public IEnumerable<OrderResponse> ListEntityToResponse(IEnumerable<Order> entities)
        {
            return entities.Select(x => EntityToResponse(x)).ToList();
        }

        public Order UpdateToEntity(OrderUpdate update)
        {
            Order order = new Order();
            order.Code = update.Code;
            order.Status = update.Status;
            order.InitialTotalAmount = update.InitialTotalAmount;
            order.TotalAmount = update.TotalAmount;
            order.Note = update.Note;
            order.CustomerId = update.CustomerId;
            order.EmployeeId = update.EmployeeId;
            order.VoucherId = update.VoucherId;
            order.OrderDate = DateTime.Now.AddHours(7);
            order.CreatedBy = "System";
            order.CreateDate = DateTime.Now.AddHours(7);
            order.UpdateDate = DateTime.Now.AddHours(7);
            order.UpdateBy = "System";
            return order;
        }
    }
}
