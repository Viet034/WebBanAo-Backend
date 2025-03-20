using Quartz;
using System.Threading.Tasks;
using WebBanAoo.Service;
namespace WebBanAoo.Ultility;

public class MyCronJob : IJob
{
    private readonly IVoucherService _voucher;
    private readonly ILogger<MyCronJob> _logger;

    public MyCronJob(IVoucherService voucher, ILogger<MyCronJob> logger)
    {
        _voucher = voucher;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var ss = await _voucher.ScanAndUpdateStatusAsync();
    }
}
