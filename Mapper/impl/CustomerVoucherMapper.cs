using Microsoft.EntityFrameworkCore;
using WebBanAoo.Data;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.CustomerVoucher;
using WebBanAoo.Models.DTO.Response;

namespace WebBanAoo.Mapper.impl
{
    public class CustomerVoucherMapper : ICustomerVoucherMapper
    {
        private readonly ApplicationDbContext _context;

        public CustomerVoucherMapper(ApplicationDbContext context)
        {
            _context = context;
        }

        public Customer_Voucher CreateToEntity(CustomerVoucherCreate create)
        {
            return new Customer_Voucher
            {
                CustomerId = create.CustomerId,
                VoucherId = create.VoucherId,
                Status = create.Status
            };
        }

        public CustomerVoucherResponse EntityToResponse(Customer_Voucher entity)
        {
            var customer = _context.Customers.Find(entity.CustomerId);
            var voucher = _context.Vouchers.Find(entity.VoucherId);

            return new CustomerVoucherResponse
            {
                Id = entity.Id,
                Status = entity.Status,
                CustomerId = entity.CustomerId,
                VoucherId = entity.VoucherId,
                CustomerName = customer?.FullName ?? "",
                VoucherCode = voucher?.Code ?? "",
                VoucherName = voucher?.Name ?? "",
                DiscountValue = voucher?.DiscountValue ?? 0,
                MinimumOrderValue = voucher?.MinimumOrderValue ?? 0,
                MaxDiscount = voucher?.MaxDiscount ?? 0,
                StartDate = voucher?.StartDate ?? DateTime.MinValue,
                EndDate = voucher?.EndDate ?? DateTime.MinValue
            };
        }

        public IEnumerable<CustomerVoucherResponse> ListEntityToResponse(IEnumerable<Customer_Voucher> entities)
        {
            return entities.Select(entity => EntityToResponse(entity));
        }
    }
} 