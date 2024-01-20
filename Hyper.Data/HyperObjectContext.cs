using Hyper.Core.Configuration;
using Hyper.Data.Mappings;
using Hyper.Data.Mappings.ApplicationUsers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace Hyper.Data
{
    /// <summary>
    /// Defines the <see cref="HyperObjectContext" />.
    /// </summary>
    public partial class HyperObjectContext : DbContext, IHyperDbProvider
    {
        /// <summary>
        /// Defines the _hyperAppSettings.
        /// </summary>
        private readonly HyperAppSettings _hyperAppSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="HyperObjectContext"/> class.
        /// </summary>
        /// <param name="hyperAppSettings">The hyperAppSettings<see cref="HyperAppSettings"/>.</param>
        public HyperObjectContext(HyperAppSettings hyperAppSettings)
        {
            _hyperAppSettings = hyperAppSettings;
        }

        /// <summary>
        /// Gets the DbContext.
        /// </summary>
        public DbContext DbContext { get => this; }

        /// <summary>
        /// The OnConfiguring.
        /// </summary>
        /// <param name="optionsBuilder">The optionsBuilder<see cref="DbContextOptionsBuilder"/>.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_hyperAppSettings.DatabaseConnectionString).UseLazyLoadingProxies();
        }

        /// <summary>
        /// The OnModelCreating.
        /// Apply configuration of entities
        /// </summary>
        /// <param name="modelBuilder">The modelBuilder<see cref="ModelBuilder"/>.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => !String.IsNullOrEmpty(type.Namespace))
                .Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
                type.BaseType.GetGenericTypeDefinition() == typeof(HyperEntityTypeConfigurator<>));

            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.ApplyConfiguration(configurationInstance);
            }
        }
    }
}
