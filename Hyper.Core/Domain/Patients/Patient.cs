using System;

namespace Hyper.Core.Domain.Patients
{
    public partial class Patient : FullAuditedEntity<int>, IStoreEntity
    {
        public int RegisteredInStoreId { get; set; }
        public string TurkishIdentityNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public bool SendEmailNotifications { get; set; }
        public string Phone { get; set; }
        public string SecondPhone { get; set; }
        public bool SendSmsNotifications { get; set; }
        public string Address { get; set; }

        public string FullName { get { return $"{Name} {Surname}"; } }

    }
}