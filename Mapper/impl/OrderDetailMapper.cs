using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.OrderDetail;
using WebBanAoo.Models.DTO.Response;

namespace WebBanAoo.Mapper.impl;

public class OrderDetailMapper : IOrderDetailMapper
{
    private readonly OrderDetail ord = new OrderDetail();

    public OrderDetail CreateToEntity(OrderDetailCreate create)
    {
        ord.Code = create.Code;
        ord.Status = create.Status;
        ord.Quantity = create.Quantity;
        ord.UnitPrice = create.UnitPrice;
        ord.Discount = create.Discount;
        ord.TotalAmount = create.TotalAmount;
        ord.Note = create.Note;
        ord.OrderId = create.OrderId;
        ord.ProductDetailId = create.ProductDetailId;
        ord.CreatedBy = "System";
        ord.CreateDate = DateTime.Now.AddHours(7);
        ord.UpdateDate = DateTime.Now.AddHours(7);
        ord.UpdateBy = "System";
        return ord;
    }

    public OrderDetail DeleteToEntity(OrderDetailDelete delete)
    {
        ord.Id = delete.Id;
        ord.Code = delete.Code;
        ord.Status = delete.Status;
        ord.Quantity = delete.Quantity;
        ord.UnitPrice = delete.UnitPrice;
        ord.Discount = delete.Discount;
        ord.TotalAmount = delete.TotalAmount;
        ord.Note = delete.Note;
        ord.OrderId = delete.OrderId;
        ord.ProductDetailId = delete.ProductDetailId;
        ord.CreatedBy = "System";
        ord.CreateDate = DateTime.Now.AddHours(7);
        ord.UpdateDate = DateTime.Now.AddHours(7);
        ord.UpdateBy = "System";
        return ord;
    }

    public OrderDetailResponse EntityToResponse(OrderDetail entity)
    {
        OrderDetailResponse response = new OrderDetailResponse();
        response.Id = entity.Id;
        response.Code = entity.Code;
        response.Status = entity.Status;
        response.Quantity = entity.Quantity;
        response.UnitPrice = entity.UnitPrice;
        response.Discount = entity.Discount;
        response.TotalAmount = entity.TotalAmount;
        response.Note = entity.Note;
        response.OrderId = entity.OrderId;
        response.ProductDetailId = entity.ProductDetailId;
        return response;
    }

    public IEnumerable<OrderDetailResponse> ListEntityToResponse(IEnumerable<OrderDetail> entities)
    {
        return entities.Select(x => EntityToResponse(x)).ToList();
    }

    public OrderDetail UpdateToEntity(OrderDetailUpdate update)
    {
        ord.Code = update.Code;
        ord.Status = update.Status;
        ord.Quantity = update.Quantity;
        ord.UnitPrice = update.UnitPrice;
        ord.Discount = update.Discount;
        ord.TotalAmount = update.TotalAmount;
        ord.Note = update.Note;
        ord.OrderId = update.OrderId;
        ord.ProductDetailId = update.ProductDetailId;
        ord.CreatedBy = "System";
        ord.CreateDate = DateTime.Now.AddHours(7);
        ord.UpdateDate = DateTime.Now.AddHours(7);
        ord.UpdateBy = "System";
        return ord;
    }
}
