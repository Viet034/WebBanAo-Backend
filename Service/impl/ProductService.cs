using Microsoft.EntityFrameworkCore;
using WebBanAoo.Data;
using WebBanAoo.Mapper;
using WebBanAoo.Models;
using WebBanAoo.Models.Status;
using WebBanAoo.Models.DTO.Request.Product;
using WebBanAoo.Models.DTO.Response;
using static WebBanAoo.Models.Status.Status;
using WebBanAoo.Ultility;

namespace WebBanAoo.Service.impl
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private IProductMapper _mapper;
        private readonly Validation<Product> _validation;

        public ProductService(ApplicationDbContext context, IProductMapper mapper, Validation<Product> validation)
        {
            _context = context;
            _mapper = mapper;
            _validation = validation;
        }

        public async Task<ProductResponse> CreateProductAsync(ProductCreate create)
        {
            Product entity = _mapper.CreateToEntity(create);

            if (!string.IsNullOrEmpty(create.Code) && create.Code != "string")
            {
                entity.Code = create.Code;
            }
            else
            {
                entity.Code = await CheckUniqueCodeAsync();
            }

            while (await _context.Products.AnyAsync(p => p.Code == entity.Code))
            {
                entity.Code = await CheckUniqueCodeAsync();
            }

            await _context.Products.AddAsync(entity);
            
            await _context.SaveChangesAsync();
            var response = _mapper.EntityToResponse(entity);
            return response;
        }

        public async Task<string> CheckUniqueCodeAsync()
        {
            string newCode;
            bool isExist;

            do
            {
                newCode = GenerateCode.GenerateProductCode();
                _context.ChangeTracker.Clear();
                isExist = await _context.Products.AnyAsync(p => p.Code == newCode);
            }
            while (isExist);

            return newCode;
        }

        public async Task<bool> HardDeleteProductAsync(int id)
        {
            var proId = _context.Products.FirstOrDefault(pro => pro.Id == id);
            if (proId == null)
            {
                throw new Exception($" Khong co Id {id} ton tai");
            }
             _context.Products.Remove(proId);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ProductResponse> FindProductByIdAsync(int id)
        {
            var proId =  _context.Products.FirstOrDefault(pro => pro.Id == id);
            if (proId == null)
            {
                throw new Exception($" Khong co Id {id} ton tai");
            }
            var response =  _mapper.EntityToResponse(proId);
            return response;
        }

        public async Task<IEnumerable<ProductResponse>> GetAllProductsAsync()
        {
            var pro = await _context.Products.ToListAsync();
            if (pro == null) throw new Exception($"Khong co ban ghi nao");

            var response =  _mapper.ListEntityToResponse(pro);

            return response;
        }

        public async Task<IEnumerable<ProductResponse>> SearchByKeyAsync(string key)
        {
            var proKey = await _context.Products
                .FromSqlRaw("Select * from Products where ProductName like {0}","%" + key + "%").ToListAsync();
            if (proKey == null) throw new Exception($"Khong co san pham ten {key} nao");
            var response = _mapper.ListEntityToResponse(proKey);
            return response;
        }

        public async Task<ProductResponse> UpdateProductAsync(int id, ProductUpdate update)
        {
            var proId = await  _context.Products.FindAsync(id);
            if (proId == null)
            {
                throw new KeyNotFoundException($" Khong co Id {id} ton tai");
            }
            proId.Code = await _validation.CheckAndUpdateAPIAsync(proId, proId.Code, update.Code, co => co.Code == update.Code);
            proId.ProductName = await _validation.CheckAndUpdateAPIAsync(proId, proId.ProductName, update.ProductName, co => co.ProductName == update.ProductName);
            proId.Description = await _validation.CheckAndUpdateAPIAsync(proId, proId.Description, update.Description, co => co.Description == update.Description);
            proId.CategoryId = await _validation.CheckAndUpdateQuantityAsync(proId, proId.CategoryId, update.CategoryId, co => co.CategoryId == update.CategoryId);
            proId.BrandId = await _validation.CheckAndUpdateQuantityAsync(proId, proId.BrandId, update.BrandId, co => co.BrandId == update.BrandId);

            var result = _mapper.UpdateToEntity(update);
            
            
            
            proId.CreateDate = result.CreateDate;
            proId.UpdateDate = result.UpdateDate;
            proId.CreatedBy = result.CreatedBy;
            proId.UpdateBy = result.UpdateBy;
            proId.Status = result.Status;
            
            await _context.SaveChangesAsync();

            var response = _mapper.EntityToResponse(proId);
            return response;
            
        }

        public async Task<ProductResponse> SoftDeleteProductAsync(int id, ProductStatus newStatus)
        {
           var proid = await _context.Products.FindAsync(id);
            if (proid == null) throw new Exception($"Khong co Id {id} ton tai");

            proid.Status = newStatus;
 
            await _context.SaveChangesAsync();

            var response = _mapper.EntityToResponse(proid);
            return response;
        }

        public async Task<IEnumerable<ProductResponse>> GetProductByCategoryIdAsync(int id)
        {
            var tId = await _context.Products.Where(pro => pro.CategoryId == id).ToListAsync();
            if (!tId.Any())
            {
                throw new Exception($"Không có sản phẩm nào chứa CategoryId {id}");
            }
            var response = _mapper.ListEntityToResponse(tId);
            return response;
        }

        public async Task<IEnumerable<ProductResponse>> GetProductByBrandIdAsync(int id)
        {
            var tId = await _context.Products.Where(pro => pro.BrandId == id).ToListAsync();
            if (!tId.Any())
            {
                throw new Exception($"Không có sản phẩm nào chứa BrandId {id}");
            }
            var response = _mapper.ListEntityToResponse(tId);
            return response;
        }
    }
}
