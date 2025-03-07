using Microsoft.EntityFrameworkCore;
using WebBanAoo.Data;
using WebBanAoo.Mapper;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Voucher;
using WebBanAoo.Models.DTO.Response;
using WebBanAoo.Models.Status;
using WebBanAoo.Ultility;

namespace WebBanAoo.Service.impl
{
    public class VoucherService : IVoucherService
    {
        private readonly ApplicationDbContext _context;
        private IVoucherMapper _mapper;
        private readonly Validation<Voucher> _validation;

        public VoucherService(ApplicationDbContext context, IVoucherMapper mapper, Validation<Voucher> validation)
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
                newCode = GenerateCode.GenerateVoucherCode();
                _context.ChangeTracker.Clear();
                isExist = await _context.Vouchers.AnyAsync(p => p.Code == newCode);
            }
            while (isExist);

            return newCode;
        }

        public async Task<VoucherResponse> CreateVoucherAsync(VoucherCreate create)
        {
            Voucher entity = _mapper.CreateToEntity(create);

            if (!string.IsNullOrEmpty(create.Code) && create.Code != "string")
            {
                entity.Code = create.Code;
            }
            else
            {
                entity.Code = await CheckUniqueCodeAsync();
            }

            while (await _context.Vouchers.AnyAsync(p => p.Code == entity.Code))
            {
                entity.Code = await CheckUniqueCodeAsync();
            }

            await _context.Vouchers.AddAsync(entity);

            await _context.SaveChangesAsync();
            var response = _mapper.EntityToResponse(entity);
            return response;
        }

        public async Task<VoucherResponse> FindVoucherByIdAsync(int id)
        {
            var coId = await _context.Vouchers.FindAsync( id);
            if (coId == null)
            {
                throw new Exception($" Khong co Id {id} ton tai");
            }
            var response = _mapper.EntityToResponse(coId);
            return response;
        }

        public async Task<IEnumerable<VoucherResponse>> GetAllVoucherAsync()
        {
            var co = await _context.Vouchers.ToListAsync();
            if (co == null) throw new Exception($"Khong co ban ghi nao");

            var response = _mapper.ListEntityToResponse(co);

            return response;
        }

        public async Task<bool> HardDeleteVoucherAsync(int id)
        {
            var co = await _context.Vouchers.FindAsync( id);
            if (co == null)
            {
                throw new KeyNotFoundException($" Khong co Id {id} ton tai");
            }
            _context.Vouchers.Remove(co);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<VoucherResponse>> SearchVoucherByKeyAsync(string key)
        {
            var coKey = await _context.Vouchers
               .FromSqlRaw("Select * from Vouchers where Name like {0}", "%" + key + "%").ToListAsync();

            if (coKey == null) throw new Exception($"Khong co Code {key} nao");
            var response = _mapper.ListEntityToResponse(coKey);
            return response;
        }

        public async Task<VoucherResponse> SoftDeleteVoucherAsync(int id, Status.VoucherStatus newStatus)
        {
            var coId = await _context.Vouchers.FindAsync(id);
            if (coId == null) throw new KeyNotFoundException($"Không có Id {id} tồn tại");

            coId.Status = newStatus;

            // Cập nhật trạng thái của tất cả Customer_Voucher liên quan
            var relatedCustomerVouchers = await _context.Customer_Vouchers
                .Where(cv => cv.VoucherId == id)
                .ToListAsync();

            foreach (var customerVoucher in relatedCustomerVouchers)
            {
                // Nếu Voucher không còn active, set Customer_Voucher thành Inactive
                if (newStatus != Status.VoucherStatus.Active)
                {
                    customerVoucher.Status = Status.CustomerVoucherStatus.Expired;
                }
            }

            await _context.SaveChangesAsync();

            var response = _mapper.EntityToResponse(coId);
            return response;
        }

        public async Task<VoucherResponse> UpdateVoucherAsync(int id, VoucherUpdate update)
        {
            var coId = await _context.Vouchers.FindAsync(id);
            if (coId == null)
            {
                throw new KeyNotFoundException($" Khong co Id {id} ton tai");
            }
            coId.Code = await _validation.CheckAndUpdateAPIAsync(coId, coId.Code, update.Code, co => co.Code == update.Code);
            //coId.Name = await _validation.CheckAndUpdateAPIAsync(coId, coId.Name, update.Name, co => co.Name == update.Name);
            coId.Name = update.Name;
            coId.Description = await _validation.CheckAndUpdateAPIAsync(coId, coId.Description, update.Description, co => co.Description == update.Description);
            coId.StartDate = await _validation.CheckAndUpdateDateGeneralAsync(coId, coId.StartDate, update.StartDate, coId.EndDate, true);
            coId.EndDate = await _validation.CheckAndUpdateDateGeneralAsync(coId, coId.EndDate, update.EndDate, coId.StartDate, false);
            coId.DiscountValue = await _validation.CheckAndUpdateQuantityAsync(coId, coId.DiscountValue, update.DiscountValue, co => co.DiscountValue == update.DiscountValue);
            coId.MinimumOrderValue = await _validation.CheckAndUpdatePriceAsync(coId, coId.MinimumOrderValue, update.MinimumOrderValue, co => co.MinimumOrderValue == update.MinimumOrderValue);
            coId.MaxDiscount = await _validation.CheckAndUpdatePriceAsync(coId, coId.MaxDiscount, update.MaxDiscount, co => co.MaxDiscount == update.MaxDiscount);
            coId.Quantity = await _validation.CheckAndUpdateQuantityAsync(coId, coId.Quantity, update.Quantity, co => co.Quantity == update.Quantity);

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
