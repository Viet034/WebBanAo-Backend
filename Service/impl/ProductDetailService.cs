using Microsoft.EntityFrameworkCore;
using WebBanAoo.Data;
using WebBanAoo.Mapper;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.ProductDetail;
using WebBanAoo.Models.DTO.Response;
using WebBanAoo.Models.Status;
using WebBanAoo.Ultility;

namespace WebBanAoo.Service.impl
{
    public class ProductDetailService : IProductDetailService
    {
        private readonly ApplicationDbContext _context;
        private IProductDetailMapper _mapper;
        private readonly Validation<ProductDetail> _validation;


        public ProductDetailService(ApplicationDbContext context, IProductDetailMapper mapper, Validation<ProductDetail> validation)
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
                isExist = await _context.ProductDetail.AnyAsync(p => p.Code == newCode);
            }
            while (isExist);

            return newCode;
        }

        public async Task<ProductDetailResponse> CreateProductDetailAsync(ProductDetailCreate create)
        {
            ProductDetail entity = _mapper.CreateToEntity(create);

            if (!string.IsNullOrEmpty(create.Code) && create.Code != "string")
            {
                entity.Code = create.Code;
            }
            else
            {
                entity.Code = await CheckUniqueCodeAsync();
            }

            while (await _context.ProductDetail.AnyAsync(p => p.Code == entity.Code))
            {
                entity.Code = await CheckUniqueCodeAsync();
            }

            await _context.ProductDetail.AddAsync(entity);

            await _context.SaveChangesAsync();
            var response = _mapper.EntityToResponse(entity);
            return response;
        }

        public async Task<ProductDetailResponse> FindProductDetailByIdAsync(int id)
        {
            var coId = await _context.ProductDetail.FindAsync( id);
            if (coId == null)
            {
                throw new KeyNotFoundException($" Khong co Id {id} ton tai");
            }
            var response = _mapper.EntityToResponse(coId);
            return response;
        }

        public async Task<IEnumerable<ProductDetailResponse>> GetAllProductDetailAsync()
        {
            var co = await _context.ProductDetail.OrderByDescending(x => x.CreateDate).ToListAsync();
            if (co == null) throw new Exception($"Khong co ban ghi nao");

            var response = _mapper.ListEntityToResponse(co);

            return response;
            
        }

        public async Task<bool> HardDeleteProductDetailAsync(int id)
        {
            var co = await _context.ProductDetail.FindAsync( id);
            if (co == null)
            {
                throw new KeyNotFoundException($" Khong co Id {id} ton tai");
            }
            _context.ProductDetail.Remove(co);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ProductDetailResponse>> SearchProductDetailByKeyAsync(string key)
        {
            var coKey = await _context.ProductDetail
               .FromSqlRaw("Select * from ProductDetails where Name like {0}", "%" + key + "%").ToListAsync();

            if (coKey == null) throw new Exception($"Khong co Code {key} nao");
            var response = _mapper.ListEntityToResponse(coKey);
            return response;
        }



        public async Task<ProductDetailResponse> SoftDeleteProductDetailAsync(int id, Status.ProductDetailStatus newStatus)
        {
            var coId = await _context.ProductDetail.FindAsync(id);
            if (coId == null) throw new KeyNotFoundException($"Khong co Id {id} ton tai");

            coId.Status = newStatus;

            await _context.SaveChangesAsync();

            var response = _mapper.EntityToResponse(coId);
            return response;
        }

        public async Task<ProductDetailResponse> UpdateProductDetailAsync(int id, ProductDetailUpdate update)
        {
            var coId = await _context.ProductDetail.FindAsync(id);
            if (coId == null)
            {
                throw new KeyNotFoundException($" Khong co Id {id} ton tai");
            }
            coId.Code = await _validation.CheckAndUpdateAPIAsync(coId, coId.Code, update.Code, co => co.Code == update.Code);
            coId.Name = await _validation.CheckAndUpdateAPIAsync(coId, coId.Name, update.Name, co => co.Name == update.Name);
            coId.Price = await _validation.CheckAndUpdatePriceAsync(coId, coId.Price, update.Price, co => co.Price == update.Price);
            coId.ProductId = await _validation.CheckAndUpdateQuantityAsync(coId, coId.ProductId, update.ProductId, co => co.ProductId == update.ProductId);
            coId.ColorId = await _validation.CheckAndUpdateQuantityAsync(coId, coId.ColorId, update.ColorId, co => co.ColorId == update.ColorId);
            coId.SizeId = await _validation.CheckAndUpdateQuantityAsync(coId, coId.SizeId, update.SizeId, co => co.SizeId == update.SizeId);
            coId.Quantity = await _validation.CheckAndUpdateQuantityAsync(coId, coId.Quantity, update.Quantity, co => co.Quantity == update.Quantity);

            var result = _mapper.UpdateToEntity(coId, update);
            
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
