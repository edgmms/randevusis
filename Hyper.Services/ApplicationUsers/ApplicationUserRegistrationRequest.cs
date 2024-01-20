using Hyper.Core.Domain.ApplicationUsers;

namespace Hyper.Services.ApplicationUsers
{
    public class ApplicationUserRegistrationRequest
    {
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }
        public PasswordFormat PasswordFormat { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string SystemName { get; set; }
        public bool RequireReLogin { get; set; }
        public bool IsSystemAccount { get; set; }
        public int RegisteredInStoreId { get; set; }
    }
}