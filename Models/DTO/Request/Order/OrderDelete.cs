﻿using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models.DTO.Request.Order
{
    public class OrderDelete
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public decimal InitialTotalAmount { get; set; } // tổng tiền ban đầu

        public decimal TotalAmount { get; set; }
        public string? Note { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        public OrderDelete()
        {
        }

        public OrderDelete(int id, string code, OrderStatus status, decimal initialTotalAmount, decimal totalAmount, string? note, DateTime orderDate, DateTime createDate)
        {
            Id = id;
            Code = code;
            Status = status;
            InitialTotalAmount = initialTotalAmount;
            TotalAmount = totalAmount;
            Note = note;
            OrderDate = orderDate;
            CreateDate = createDate;
        }
    }
}
