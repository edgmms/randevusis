using System;

namespace Hyper.Core.Domain.ApplicationUsers
{
    /// <summary>
    /// Represents a applicationUser
    /// </summary>
    public partial class ApplicationUser : BaseEntity<int>, IStoreEntity
    {
        public ApplicationUser()
        {
            ApplicationUserGuid = Guid.NewGuid();
        }

        public int RegisteredInStoreId { get; set; }

        /// <summary>
        /// Gets or sets the applicationUser GUID
        /// </summary>
        public Guid ApplicationUserGuid { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the full name
        /// </summary>
        public string FullName { get { return $"{FirstName} {LastName}"; } }

        /// <summary>
        /// Gets or sets the gender
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the date of birth
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the street address
        /// </summary>
        public string StreetAddress { get; set; }

        /// <summary>
        /// Gets or sets the street address 2
        /// </summary>
        public string StreetAddress2 { get; set; }

        /// <summary>
        /// Gets or sets the zip
        /// </summary>
        public string ZipPostalCode { get; set; }

        /// <summary>
        /// Gets or sets the city
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the county
        /// </summary>
        public string County { get; set; }

        /// <summary>
        /// Gets or sets the country id
        /// </summary>
        public int CountryId { get; set; }

        /// <summary>
        /// Gets or sets the state province id
        /// </summary>
        public int StateProvinceId { get; set; }

        /// <summary>
        /// Gets or sets the phone number
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the fax
        /// </summary>
        public string Fax { get; set; }
         
        /// <summary>
        /// Gets or sets the email that should be re-validated. 
        /// Used in scenarios when a applicationUser is already registered and wants to change an email address.
        /// </summary>
        public string EmailToRevalidate { get; set; }

        /// <summary>
        /// Gets or sets the admin comment
        /// </summary>
        public string AdminComment { get; set; }
         
        /// <summary>
        /// Gets or sets a value indicating whether the applicationUser is required to re-login
        /// </summary>
        public bool RequireReLogin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating number of failed login attempts (wrong password)
        /// </summary>
        public int FailedLoginAttempts { get; set; }

        /// <summary>
        /// Gets or sets the date and time until which a applicationUser cannot login (locked out)
        /// </summary>
        public DateTime? CannotLoginUntilDateUtc { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the applicationUser account is system
        /// </summary>
        public bool IsSystemAccount { get; set; }

        /// <summary>
        /// Gets or sets the applicationUser system name
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// Gets or sets the last IP address
        /// </summary>
        public string LastIpAddress { get; set; }
         
        /// <summary>
        /// Gets or sets the date and time of last login
        /// </summary>
        public DateTime? LastLoginDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of last activity
        /// </summary>
        public DateTime LastActivityDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the PasswordRecoveryToken
        /// </summary>
        public string PasswordRecoveryToken { get; set; }

        /// <summary>
        /// Gets or sets the PasswordRecoveryToken
        /// </summary>
        public string PasswordRecoveryHashedToken { get; set; }

        /// <summary>
        /// Gets or sets the PasswordRecoveryTokenSaltKey
        /// </summary>
        public string PasswordRecoveryTokenSaltKey { get; set; }

        /// <summary>
        /// Gets or sets the PasswordRecoveryTokenCreatedOn
        /// </summary>
        public DateTime? PasswordRecoveryTokenCreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets deleted
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets active
        /// </summary>
        public bool Active { get; set; }
    }
}