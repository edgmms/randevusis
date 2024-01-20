namespace Hyper.Core.Domain.ApplicationUsers
{
    public class ApplicationUserApplicationUserRoleMapping : BaseEntity<int>
    {
        public int ApplicationUserId { get; set; }
        public int ApplicationUserRoleId { get; set; }
    }
}
