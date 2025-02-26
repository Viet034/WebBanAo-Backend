namespace WebBanAoo.Service
{
    public interface ICustomPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
} 