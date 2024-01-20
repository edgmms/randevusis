namespace Hyper.Core.Configuration
{
    public class DefaultEmailAccountSettings
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public bool EnableSsl { get; set; }
        public string Host { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public bool UseDefaultCredentials { get; set; }
    }
}
