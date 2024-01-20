namespace Hyper.Core.Configuration
{
    public class SecuritySettings
    {
        public string EncryptionKey { get; set; }
        public bool AllowNonAsciiCharactersInHeaders { get; set; }
    }
}
