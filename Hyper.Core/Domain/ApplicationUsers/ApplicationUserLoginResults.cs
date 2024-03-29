﻿namespace Hyper.Core.Domain.ApplicationUsers
{
    /// <summary>
    /// Represents the application user login result enumeration
    /// </summary>
    public enum ApplicationUserLoginResults
    {
        /// <summary>
        /// Login successful
        /// </summary>
        Successful = 1,

        /// <summary>
        /// ApplicationUser does not exist (email or username)
        /// </summary>
        ApplicationUserNotExist = 2,

        /// <summary>
        /// Wrong password
        /// </summary>
        WrongPassword = 3,

        /// <summary>
        /// Account have not been activated
        /// </summary>
        NotActive = 4,

        /// <summary>
        /// ApplicationUser has been deleted 
        /// </summary>
        Deleted = 5,

        /// <summary>
        /// Locked out
        /// </summary>
        LockedOut = 7,
    }
}
