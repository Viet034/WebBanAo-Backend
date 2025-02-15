using Microsoft.EntityFrameworkCore;
using WebBanAoo.Data;
using WebBanAoo.Mapper;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.OrderDetail;
using WebBanAoo.Models.DTO.Response;
using WebBanAoo.Models.Status;
using WebBanAoo.Models.ultility;

namespace WebBanAoo.Service.impl
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly ApplicationDbContext _context;
        private IOrderDetailMapper _mapper;

        public OrderDetailService(ApplicationDbContext context, IOrderDetailMapper mapper)
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
                newCode = GenerateCode.GenerateOrderDetailCode();
                _context.ChangeTracker.Clear();
                isExist = await _context.OrderDetails.AnyAsync(p => p.Code == newCode);
            }
            while (isExist);

            return newCode;
        }

        public async Task<OrderDetailResponse> CreateOrderDetailAsync(OrderDetailCreate create)
        {
            OrderDetail entity = _mapper.CreateToEntity(create);

            if (string.IsNullOrEmpty(entity.Code) || entity.Code == "string")
            {
                entity.Code = await CheckUniqueCodeAsync();
            }
            while (await _context.OrderDetails.AnyAsync(p => p.Code == entity.Code))
            {
                entity.Code = await CheckUniqueCodeAsync();
            }

            await _context.OrderDetails.AddAsync(entity);

            await _context.SaveChangesAsync();
            var response = _mapper.EntityToResponse(entity);
            return response;
        }

        public async Task<OrderDetailResponse> FindOrderDetailByIdAsync(int id)
        {
            var coId = _context.OrderDetails.FirstOrDefault(co => co.Id == id);
            if (coId == null)
            {
                throw new Exception($" Khong co Id {id} ton tai");
            }
            var response = _mapper.EntityToResponse(coId);
            return response;
        }

        public async Task<IEnumerable<OrderDetailResponse>> GetAllOrderDetailAsync()
        {
            var co = await _context.OrderDetails.ToListAsync();
            if (co == null) throw new Exception($"Khong co ban ghi nao");

            var response = _mapper.ListEntityToResponse(co);

            return response;
        }

        public async Task<bool> HardDeleteOrderDetailAsync(int id)
        {
            var co = _context.OrderDetails.FirstOrDefault(co => co.Id == id);
            if (co == null)
            {
                throw new Exception($" Khong co Id {id} ton tai");
            }
            _context.OrderDetails.Remove(co);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<OrderDetailResponse>> SearchOrderDetailByCodeAsync(string key)
        {
            var coKey = await _context.OrderDetails
               .FromSqlRaw("Select * from OrderDetails where Code like {0}", "%" + key + "%").ToListAsync();

            if (coKey == null) throw new Exception($"Khong co Code {key} nao");
            var response = _mapper.ListEntityToResponse(coKey);
            return response;
        }

        public async Task<OrderDetailResponse> SoftDeleteOrderDetailAsync(int id, Status.OrderDetailStatus newStatus)
        {
            var coId = await _context.OrderDetails.FindAsync(id);
            if (coId == null) throw new Exception($"Khong co Id {id} ton tai");

            coId.Status = newStatus;

            await _context.SaveChangesAsync();

            var response = _mapper.EntityToResponse(coId);
            return response;
        }

        public async Task<OrderDetailResponse> UpdateOrderDetailAsync(int id, OrderDetailUpdate update)
        {
            var coId = await _context.OrderDetails.FirstOrDefaultAsync(co => co.Id == id);
            if (coId == null)
            {
                throw new Exception($" Khong co Id {id} ton tai");
            }
            if (!string.IsNullOrEmpty(update.Code) && update.Code != "string" && update.Code != coId.Code)
            {
                bool isExist = await _context.OrderDetails.AnyAsync(p => p.Code == update.Code);
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
            coId.Quantity = result.Quantity;
            coId.UnitPrice = result.UnitPrice;
            coId.Discount = result.Discount;
            coId.TotalAmount = result.TotalAmount;
            coId.Note = result.Note;
            coId.OrderId = result.OrderId;
            coId.ProductDetailId = result.ProductDetailId;      
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
