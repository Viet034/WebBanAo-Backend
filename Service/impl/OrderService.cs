using Microsoft.EntityFrameworkCore;
using WebBanAoo.Data;
using WebBanAoo.Mapper;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Order;
using WebBanAoo.Models.DTO.Response;
using WebBanAoo.Models.Status;
using WebBanAoo.Ultility;

namespace WebBanAoo.Service.impl
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private IOrderMapper _mapper;
        private readonly Validation<Order> _validation;

        public OrderService(ApplicationDbContext context, IOrderMapper mapper, Validation<Order> validation)
        {
            _context = context;
            _mapper = mapper;
            _validation = validation;
        }

        public async Task<string> CheckUniqueCodeAsync()
        {
            string newCode;
            bool isExist;

            do
            {
                newCode = GenerateCode.GenerateOrderCode();
                _context.ChangeTracker.Clear();
                isExist = await _context.Orders.AnyAsync(p => p.Code == newCode);
            }
            while (isExist);

            return newCode;
        }

        public async Task<OrderResponse> CreateOrderAsync(OrderCreate create)
        {
            Order entity = _mapper.CreateToEntity(create);

            if (string.IsNullOrEmpty(entity.Code) || entity.Code == "string")
            {
                entity.Code = await CheckUniqueCodeAsync();
            }
            while (await _context.Orders.AnyAsync(p => p.Code == entity.Code))
            {
                entity.Code = await CheckUniqueCodeAsync();
            }

            await _context.Orders.AddAsync(entity);

            await _context.SaveChangesAsync();
            var response = _mapper.EntityToResponse(entity);
            return response;
        }

        public async Task<OrderResponse> FindOrderByIdAsync(int id)
        {
            var coId = await  _context.Orders.FindAsync( id);
            if (coId == null)
            {
                throw new KeyNotFoundException($" Khong co Id {id} ton tai");
            }
            var response = _mapper.EntityToResponse(coId);
            return response;
        }

        public async Task<IEnumerable<OrderResponse>> GetAllOrderAsync()
        {
            var co = await _context.Orders.ToListAsync();
            if (co == null) throw new Exception($"Khong co ban ghi nao");

            var response = _mapper.ListEntityToResponse(co);

            return response;
        }

        public async Task<bool> HardDeleteOrderAsync(int id)
        {
            var co = await _context.Orders.FindAsync( id);
            if (co == null)
            {
                throw new KeyNotFoundException($" Khong co Id {id} ton tai");
            }
            _context.Orders.Remove(co);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<OrderResponse>> SearchOrderByKeyAsync(string key)
        {
            var coKey = await _context.Orders
               .FromSqlRaw("Select * from Orders where Code like {0}", "%" + key + "%").ToListAsync();

            if (coKey == null) throw new Exception($"Khong co Code {key} nao");
            var response = _mapper.ListEntityToResponse(coKey);
            return response;
        }

        public async Task<OrderResponse> SoftDeleteOrderAsync(int id, Status.OrderStatus newStatus)
        {
            var coId = await _context.Orders.FindAsync(id);
            if (coId == null) throw new Exception($"Khong co Id {id} ton tai");

            coId.Status = newStatus;

            await _context.SaveChangesAsync();

            var response = _mapper.EntityToResponse(coId);
            return response;
        }

        public async Task<OrderResponse> UpdateOrderAsync(int id, OrderUpdate update)
        {
            var coId = await _context.Orders.FindAsync( id);
            if (coId == null)
            {
                throw new KeyNotFoundException($" Khong co Id {id} ton tai");
            }
            coId.Code = await _validation.CheckAndUpdateAPIAsync(coId, coId.Code, update.Code, co => co.Code == update.Code);
            coId.Note = await _validation.CheckAndUpdateAPIAsync(coId, coId.Note, update.Note, co => co.Note == update.Note);
            coId.InitialTotalAmount = await _validation.CheckAndUpdatePriceAsync(coId, coId.InitialTotalAmount, update.InitialTotalAmount, co => co.InitialTotalAmount == update.InitialTotalAmount);
            coId.TotalAmount = await _validation.CheckAndUpdatePriceAsync(coId, coId.TotalAmount, update.TotalAmount, co => co.TotalAmount == update.TotalAmount);
            
            var result = _mapper.UpdateToEntity(update);
            
            coId.Status = result.Status;
            
            coId.OrderDate = result.OrderDate;
            coId.CreateDate = result.CreateDate;
            coId.UpdateDate = result.UpdateDate;
            coId.CreatedBy = result.CreatedBy;
            coId.UpdateBy = result.UpdateBy;


            await _context.SaveChangesAsync();

            var response = _mapper.EntityToResponse(coId);
            return response;
        }

        public async Task<OrderResponse> ApplyVoucherToOrderAsync(int orderId, int voucherId)
    {
        var order = await _context.Orders
            .Include(o => o.OrderDetails)
            .FirstOrDefaultAsync(o => o.Id == orderId);
        
        if (order == null)
            throw new KeyNotFoundException($"Order {orderId} not found");

        var voucher = await _context.Vouchers
            .FirstOrDefaultAsync(v => v.Id == voucherId);
        
        if (voucher == null)
            throw new KeyNotFoundException($"Voucher {voucherId} not found");

        // Validate voucher
        if (voucher.StartDate > DateTime.Now || voucher.EndDate < DateTime.Now)
            throw new InvalidOperationException("Voucher is expired or not yet active");
            
        if (order.InitialTotalAmount < voucher.MinimumOrderValue)
            throw new InvalidOperationException($"Order total must be at least {voucher.MinimumOrderValue}");

        // Calculate discount
        decimal discount = Math.Min(
            order.InitialTotalAmount * voucher.DiscountValue / 100,
            voucher.MaxDiscount
        );

        order.VoucherId = voucherId;
        order.TotalAmount = order.InitialTotalAmount - discount;
        
        await _context.SaveChangesAsync();
        
        return _mapper.EntityToResponse(order);
    }
    }
}
