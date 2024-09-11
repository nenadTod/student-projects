namespace RentApp.Services
{
    public interface ISmtpService
    {
        void SendMail(string subject, string body, string mailTo);
    }
}