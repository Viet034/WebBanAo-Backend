using Microsoft.EntityFrameworkCore;
using WebBanAoo.Data;
using WebBanAoo.Mapper;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.OrderDetail;
using WebBanAoo.Models.DTO.Response;
using WebBanAoo.Models.Status;
using WebBanAoo.Ultility;

namespace WebBanAoo.Service.impl
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly ApplicationDbContext _context;
        private IOrderDetailMapper _mapper;
        private readonly Validation<OrderDetail> _validation;

        public OrderDetailService(ApplicationDbContext context, IOrderDetailMapper mapper, Validation<OrderDetail> validation)
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
            var coId = await _context.OrderDetails.FindAsync( id);
            if (coId == null)
            {
                throw new KeyNotFoundException($" Khong co Id {id} ton tai");
            }
            var response = _mapper.EntityToResponse(coId);
            return response;
        }
        public async Task<IEnumerable<OrderDetailResponse>> GetOrderDetailByOrderIdAsync(int id)
        {
            var tId = await _context.OrderDetails.Where(pr => pr.OrderId == id).ToListAsync();
            if (!tId.Any())
            {
                throw new Exception($"Không có Order nào thuộc id = {id}.");
            }
            var response = _mapper.ListEntityToResponse(tId);
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
            var co = await _context.OrderDetails.FindAsync( id);
            if (co == null)
            {
                throw new KeyNotFoundException($" Khong co Id {id} ton tai");
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
            var coId = await _context.OrderDetails.FindAsync( id);
            if (coId == null)
            {
                throw new KeyNotFoundException($" Khong co Id {id} ton tai");
            }
            coId.Code = await _validation.CheckAndUpdateAPIAsync(coId, coId.Code, update.Code, co => co.Code == update.Code);
            coId.Note = await _validation.CheckAndUpdateAPIAsync(coId, coId.Note, update.Note, co => co.Note == update.Note);
            coId.Quantity = await _validation.CheckAndUpdateQuantityAsync(coId, coId.Quantity, update.Quantity, co => co.Quantity == update.Quantity);
            coId.TotalAmount = await _validation.CheckAndUpdatePriceAsync(coId, coId.TotalAmount, update.TotalAmount, co => co.TotalAmount == update.TotalAmount);
            coId.UnitPrice = await _validation.CheckAndUpdatePriceAsync(coId, coId.UnitPrice, update.UnitPrice, co => co.UnitPrice == update.UnitPrice);
            coId.Discount = update.Discount;
            coId.OrderId = await _validation.CheckAndUpdateQuantityAsync(coId, coId.OrderId, update.OrderId, co => co.OrderId == update.OrderId);
            coId.ProductDetailId = await _validation.CheckAndUpdateQuantityAsync(coId, coId.ProductDetailId, update.ProductDetailId, co => co.ProductDetailId == update.ProductDetailId);
            
            var result = _mapper.UpdateToEntity(update);
            
            coId.Status = result.Status;
        
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

