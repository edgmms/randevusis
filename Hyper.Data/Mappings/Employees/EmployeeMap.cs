using Hyper.Core.Domain.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hyper.Data.Mappings.Employees
{
    public class EmployeeMap : HyperEntityTypeConfigurator<Employee>
    {
        public override void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable(nameof(Employee));
            builder.HasKey(e => e.Id);

            builder.Ignore(e => e.FullName);
        }
    }
}
