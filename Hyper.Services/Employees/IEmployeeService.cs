using Hyper.Core;
using Hyper.Core.Domain.Employees;
using System;

namespace Hyper.Services.Employees
{
    public interface IEmployeeService : IBaseService<Employee>
    {
        IPagedList<Employee> SearchEmployees(string name = "", string surname = "", string turkishIdentityNumber = "",
            DateTime? dateOfBirth = null, string gender = "", string email = "", string phone = "",
            string address = "", bool loadOnlyEmailNotificationActive = false, bool loadOnlySmsNotificationActive = false, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
