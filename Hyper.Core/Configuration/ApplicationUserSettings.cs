using Hyper.Core.Domain.ApplicationUsers;

namespace Hyper.Core.Configuration
{
    /// <summary>
    /// ApplicationUser settings
    /// </summary>
    public partial class ApplicationUserSettings
    {
        public int PasswordMinLength { get; set; }
        public bool PasswordRequireLowercase { get; set; }
        public bool PasswordRequireUppercase { get; set; }
        public bool PasswordRequireNonAlphanumeric { get; set; }
        public bool PasswordRequireDigit { get; set; }
        public int UnduplicatedPasswordsNumber { get; set; }
        public int PasswordRecoveryLinkDaysValid { get; set; }
        public int PasswordLifetime { get; set; }
        public string HashedPasswordFormat { get; set; }
        public int PasswordSaltKeySize { get; set; }
        public int FailedPasswordAllowedAttempts { get; set; }
        public int FailedPasswordLockoutMinutes { get; set; }
        public bool LastVisitedPage { get; set; }
        public bool IpAddresses { get; set; }
        public bool SuffixDeletedApplicationUsers { get; set; }
    }
}
