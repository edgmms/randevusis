using Hyper.Core;
using Hyper.Core.Domain.Patients;
using System;

namespace Hyper.Services.Patients
{
    public partial interface IPatientService : IBaseService<Patient>
    {
        IPagedList<Patient> SearchPatients(string name = "", string surname = "", string turkishIdentityNumber = "",
            DateTime? dateOfBirth = null, string gender = "", string email = "", string phone = "", string address = "",
            bool loadOnlyEmailNotificationActive = false, bool loadOnlySmsNotificationActive = false,
            int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
