using Microsoft.AspNetCore.Http.HttpResults;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Voucher;
using WebBanAoo.Models.DTO.Response;

namespace WebBanAoo.Mapper.impl
{
    public class VoucherMapper : IVoucherMapper
    {
        
        public Voucher CreateToEntity(VoucherCreate create)
        {
            Voucher voucher = new Voucher();
            voucher.Code = create.Code;
            voucher.Name = create.Name;
            voucher.Description = create.Description;
            voucher.Status = create.Status;
            voucher.StartDate = TimeZoneInfo.ConvertTimeToUtc(create.StartDate).AddHours(7);
            voucher.EndDate = TimeZoneInfo.ConvertTimeToUtc(create.EndDate).AddHours(7);
            voucher.Quantity = create.Quantity;
            voucher.DiscountValue = create.DiscountValue;
            voucher.MinimumOrderValue = create.MinimumOrderValue;
            voucher.MaxDiscount = create.MaxDiscount;
            voucher.CreatedBy = "System";
            voucher.CreateDate = DateTime.Now.AddHours(7);
            voucher.UpdateDate = DateTime.Now.AddHours(7);
            voucher.UpdateBy = "System";
            return voucher;
        }

        public Voucher DeleteToEntity(VoucherDelete delete)
        {
            Voucher voucher = new Voucher();
            voucher.Id = delete.Id;
            voucher.Code = delete.Code;
            voucher.Name = delete.Name;
            voucher.Description = delete.Description;
            voucher.Status = delete.Status;
            voucher.StartDate = delete.StartDate;
            voucher.EndDate = delete.EndDate;
            voucher.Quantity = delete.Quantity;
            voucher.DiscountValue = delete.DiscountValue;
            voucher.MinimumOrderValue = delete.MinimumOrderValue;
            voucher.MaxDiscount = delete.MaxDiscount;
            voucher.CreatedBy = "System";
            voucher.CreateDate = DateTime.Now.AddHours(7);
            voucher.UpdateDate = DateTime.Now.AddHours(7);
            voucher.UpdateBy = "System";
            return voucher;
        }

        public VoucherResponse EntityToResponse(Voucher entity)
        {
            VoucherResponse response = new VoucherResponse();
            response.Id = entity.Id;
            response.Code = entity.Code;
            response.Name = entity.Name;
            response.Description = entity.Description;
            response.Status = entity.Status;
            response.StartDate = entity.StartDate;
            response.EndDate = entity.EndDate;
            response.Quantity = entity.Quantity;
            response.DiscountValue = entity.DiscountValue;
            response.MinimumOrderValue = entity.MinimumOrderValue;
            response.MaxDiscount = entity.MaxDiscount;
            
            return response;
        }

        public IEnumerable<VoucherResponse> ListEntityToResponse(IEnumerable<Voucher> entities)
        {
            return entities.Select(x => EntityToResponse(x)).ToList();
        }

        public Voucher UpdateToEntity(VoucherUpdate update)
        {
            TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

            Voucher voucher = new Voucher();
            voucher.Code = update.Code;
            voucher.Name = update.Name;
            voucher.Description = update.Description;
            voucher.Status = update.Status;


            //voucher.StartDate = TimeZoneInfo.ConvertTimeToUtc(update.StartDate).AddHours(7);
            //voucher.EndDate = TimeZoneInfo.ConvertTimeToUtc(update.EndDate).AddHours(7);
            voucher.StartDate = TimeZoneInfo.ConvertTime(update.StartDate, vnTimeZone);
            voucher.EndDate = TimeZoneInfo.ConvertTime(update.EndDate, vnTimeZone);



            voucher.Quantity = update.Quantity;
            voucher.DiscountValue = update.DiscountValue;
            voucher.MinimumOrderValue = update.MinimumOrderValue;
            voucher.MaxDiscount = update.MaxDiscount;
            voucher.CreatedBy = "System";
            voucher.CreateDate = DateTime.Now.AddHours(7);
            voucher.UpdateDate = DateTime.Now.AddHours(7);
            voucher.UpdateBy = "System";
            return voucher;
        }
    }
}
