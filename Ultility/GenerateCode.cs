namespace WebBanAoo.Ultility;

public class GenerateCode
{
    private static readonly Random random = new Random();
    public static string GenerateProductCode()
    {
        
        int proRand = random.Next(100000, 999999);
        return "PRO" + proRand;
    }
    public static string GenerateEmployeeCode()
    {
        
        int empRand = random.Next(100000, 999999);
        return "EMP" + empRand;
    }
    public static string GenerateBrandCode()
    {
        
        int brandRand = random.Next(100000, 999999);
        return "BRA" + brandRand;
    }
    public static string GenerateColorCode()
    {

        int coRand = random.Next(100000, 999999);
        return "CO" + coRand;
    }
    public static string GenerateCategoryCode()
    {

        int catRand = random.Next(100000, 999999);
        return "CAT" + catRand;
    }
    public static string GenerateCustomerCode()
    {

        int cusRand = random.Next(100000, 999999);
        return "CUS" + cusRand;
    }
    public static string GenerateOrderCode()
    {

        int ordRand = random.Next(100000, 999999);
        return "ORD" + ordRand;
    }
    public static string GenerateOrderDetailCode()
    {

        int orddRand = random.Next(100000, 999999);
        return "ORDD" + orddRand;
    }
    public static string GenerateProductDetailCode()
    {

        int proddRand = random.Next(100000, 999999);
        return "PROD" + proddRand;
    }

    public static string GenerateRoleCode()
    {

        int roleRand = random.Next(100000, 999999);
        return "ROL" + roleRand;
    }
    public static string GenerateSaleCode()
    {

        int saRand = random.Next(100000, 999999);
        return "SA" + saRand;
    }

    public static string GenerateSizeCode()
    {

        int siRand = random.Next(100000, 999999);
        return "SI" + siRand;
    }
    public static string GenerateVoucherCode()
    {

        int vouRand = random.Next(100000, 999999);
        return "VOU" + vouRand;
    }
    public static string GenerateProductImageCode()
    {

        int proImage = random.Next(100000, 999999);
        return "PRDI" + proImage;
    }
}
