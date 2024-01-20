namespace Hyper.Core.Domain.ApplicationUsers
{
    public class ApplicationUserRole : BaseEntity<int>
    {
        public string Name { get; set; }

        public string SystemName { get; set; }

        public bool IsSystemRole { get; set; }

        public bool EnablePasswordLifetime { get; set; }
    }
}
