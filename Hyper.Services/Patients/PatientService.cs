﻿using Hyper.Core;
using Hyper.Core.Domain.Patients;
using Hyper.Core.Extensions;
using Hyper.Data;
using System;
using System.Linq;

namespace Hyper.Services.Patients
{
    public partial class PatientService : BaseService<Patient>, IPatientService
    {
        private readonly IRepository<Patient> _patientRepository;

        public PatientService(IRepository<Patient> patientRepository) : base(patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public IPagedList<Patient> SearchPatients(string name = "", string surname = "", string turkishIdentityNumber = "",
            DateTime? dateOfBirth = null, string gender = "", string email = "", string phone = "", string address = "",
            bool loadOnlyEmailNotificationActive = false, bool loadOnlySmsNotificationActive = false,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _patientRepository.Table;

            if (!name.IsNullOrEmpty())
                query = query.Where(x => x.Name.Contains(name));

            if (!surname.IsNullOrEmpty())
                query = query.Where(x => x.Surname.Contains(surname));

            if (!turkishIdentityNumber.IsNullOrEmpty())
                query = query.Where(x => x.TurkishIdentityNumber == turkishIdentityNumber);

            if (dateOfBirth is not null)
                query = query.Where(x => x.DateOfBirth == dateOfBirth);

            if (!gender.IsNullOrEmpty())
                query = query.Where(x => x.Gender == gender);

            if (!email.IsNullOrEmpty())
                query = query.Where(x => x.Email.Contains(email));

            if (!phone.IsNullOrEmpty())
                query = query.Where(x => x.Phone == phone);

            if (!address.IsNullOrEmpty())
                query = query.Where(x => x.Address == address);

            if (loadOnlyEmailNotificationActive)
                query = query.Where(x => x.SendEmailNotifications == loadOnlyEmailNotificationActive);

            if (loadOnlySmsNotificationActive)
                query = query.Where(x => x.SendSmsNotifications == loadOnlySmsNotificationActive);

            var data = new PagedList<Patient>(query, pageIndex, pageSize);
            return data;
        }
    }
}
