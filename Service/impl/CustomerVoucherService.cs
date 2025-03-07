using Microsoft.EntityFrameworkCore;
using WebBanAoo.Data;
using WebBanAoo.Mapper;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.CustomerVoucher;
using WebBanAoo.Models.DTO.Response;
using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Service.impl
{
    public class CustomerVoucherService : ICustomerVoucherService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICustomerVoucherMapper _mapper;

        public CustomerVoucherService(ApplicationDbContext context, ICustomerVoucherMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CustomerVoucherResponse> AssignVoucherToCustomerAsync(CustomerVoucherCreate create)
        {
            // Kiểm tra customer có tồn tại không
            var customer = await _context.Customers.FindAsync(create.CustomerId);
            if (customer == null)
                throw new KeyNotFoundException($"Không tìm thấy khách hàng với ID {create.CustomerId}");

            // Kiểm tra voucher có tồn tại và còn hiệu lực không
            var voucher = await _context.Vouchers.FindAsync(create.VoucherId);
            if (voucher == null)
                throw new KeyNotFoundException($"Không tìm thấy voucher với ID {create.VoucherId}");

            if (voucher.Status != VoucherStatus.Active)
                throw new InvalidOperationException("Voucher không còn hiệu lực");

            if (voucher.Quantity <= 0)
                throw new InvalidOperationException("Voucher đã hết lượt sử dụng");

            if (DateTime.Now > voucher.EndDate)
                throw new InvalidOperationException("Voucher đã hết hạn");

            // Kiểm tra xem customer đã có voucher này chưa
            var existingVoucher = await _context.Customer_Vouchers
                .FirstOrDefaultAsync(cv => cv.CustomerId == create.CustomerId && cv.VoucherId == create.VoucherId);

            if (existingVoucher != null)
                throw new InvalidOperationException("Khách hàng đã có voucher này");

            // Tạo customer_voucher mới
            var customerVoucher = _mapper.CreateToEntity(create);
            await _context.Customer_Vouchers.AddAsync(customerVoucher);

            // Giảm số lượng voucher
            voucher.Quantity--;

            await _context.SaveChangesAsync();

            return _mapper.EntityToResponse(customerVoucher);
        }

        public async Task<bool> CheckVoucherAvailabilityAsync(int voucherId)
        {
            var voucher = await _context.Vouchers.FindAsync(voucherId);
            if (voucher == null)
                return false;

            return voucher.Status == VoucherStatus.Active
                && voucher.Quantity > 0
                && DateTime.Now <= voucher.EndDate;
        }

        public async Task<bool> DeleteCustomerVoucherAsync(int customerVoucherId)
        {
            var customerVoucher = await _context.Customer_Vouchers.FindAsync(customerVoucherId);
            if (customerVoucher == null)
                throw new KeyNotFoundException($"Không tìm thấy customer_voucher với ID {customerVoucherId}");

            _context.Customer_Vouchers.Remove(customerVoucher);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<CustomerVoucherResponse>> GetVouchersByCustomerIdAsync(int customerId)
        {
            var customerVouchers = await _context.Customer_Vouchers
                .Where(cv => cv.CustomerId == customerId)
                .ToListAsync();

            return _mapper.ListEntityToResponse(customerVouchers);
        }

        public async Task<CustomerVoucherResponse> UpdateCustomerVoucherStatusAsync(int customerVoucherId, CustomerVoucherStatus newStatus)
        {
            var customerVoucher = await _context.Customer_Vouchers.FindAsync(customerVoucherId);
            if (customerVoucher == null)
                throw new KeyNotFoundException($"Không tìm thấy customer_voucher với ID {customerVoucherId}");

            customerVoucher.Status = newStatus;
            await _context.SaveChangesAsync();

            return _mapper.EntityToResponse(customerVoucher);
        }
    }
} 