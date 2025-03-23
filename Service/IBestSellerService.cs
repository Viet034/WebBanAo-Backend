using WebBanAoo.Models.DTO.Response;

namespace WebBanAoo.Service;

public interface IBestSellerService
{
    Task<IEnumerable<BestSellerResponse>> GetBestSellersAsync();
}
