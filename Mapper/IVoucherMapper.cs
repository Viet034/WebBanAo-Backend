using WebBanAoo.Models.DTO.Response;
using WebBanAoo.Models.DTO.Request.Voucher;
using WebBanAoo.Models;

namespace WebBanAoo.Mapper
{
    public interface IVoucherMapper
    {
        // request => Entity(DTO)
        Voucher CreateToEntity(VoucherCreate create);
        Voucher UpdateToEntity(VoucherUpdate update);
        Voucher DeleteToEntity(VoucherDelete delete);

        // Entity(DTO) => Response
        VoucherResponse EntityToResponse(Voucher entity);
        IEnumerable<VoucherResponse> ListEntityToResponse(IEnumerable<Voucher> entities);
    }
}
