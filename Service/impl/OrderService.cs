using Microsoft.EntityFrameworkCore;
using WebBanAoo.Data;
using WebBanAoo.Mapper;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Order;
using WebBanAoo.Models.DTO.Response;
using WebBanAoo.Models.Status;
using WebBanAoo.Models.ultility;

namespace WebBanAoo.Service.impl
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private IOrderMapper _mapper;

        public OrderService(ApplicationDbContext context, IOrderMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
            var coId = _context.Orders.FirstOrDefault(co => co.Id == id);
            if (coId == null)
            {
                throw new Exception($" Khong co Id {id} ton tai");
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
            var co = _context.Orders.FirstOrDefault(co => co.Id == id);
            if (co == null)
            {
                throw new Exception($" Khong co Id {id} ton tai");
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
            var coId = await _context.Orders.FirstOrDefaultAsync(co => co.Id == id);
            if (coId == null)
            {
                throw new Exception($" Khong co Id {id} ton tai");
            }
            if (!string.IsNullOrEmpty(update.Code) && update.Code != "string" && update.Code != coId.Code)
            {
                bool isExist = await _context.Orders.AnyAsync(p => p.Code == update.Code);
                if (isExist)
                {
                    throw new Exception();
                }
                coId.Code = update.Code;
            }
            else
            {
                update.Code = coId.Code;
            }
            var result = _mapper.UpdateToEntity(update);
            
            coId.Status = result.Status;
            coId.InitialTotalAmount = result.InitialTotalAmount;
            coId.TotalAmount = result.TotalAmount;
            coId.OrderDate = result.OrderDate;
            coId.CustomerId = result.CustomerId;
            coId.Note = result.Note;
            coId.EmployeeId = result.EmployeeId;
            coId.VoucherId = result.VoucherId;
            coId.CreateDate = result.CreateDate;
            coId.UpdateDate = result.UpdateDate;
            coId.CreatedBy = result.CreatedBy;
            coId.UpdateBy = result.UpdateBy;


            await _context.SaveChangesAsync();

            var response = _mapper.EntityToResponse(coId);
            return response;
        }
    }
}
