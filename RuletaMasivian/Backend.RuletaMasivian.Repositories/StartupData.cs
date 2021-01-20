namespace Backend.RuletaMasivian
{
    using Backend.RuletaMasivian.Entities.Interface.Repositories;
    using Backend.RuletaMasivian.Entities.Interface.Repository;
    using Backend.RuletaMasivian.Entities.Interface.RepositoryAdmin;
    using Backend.RuletaMasivian.Repositories.DataBase;
    using Backend.RuletaMasivian.Utilities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Startup Data.
    /// </summary>
    public static class StartupData
    {
        /// <summary>
        /// Configure data.
        /// </summary>
        /// <param name="services">Service Collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="isDev">if set to <c>true</c> [is dev].</param>
        public static void AddData(this IServiceCollection services, IConfiguration configuration, bool isDev)
        {
            services.AddDbContext<Repositories.RuletaMasivianContext>(options =>
            {
                options.UseSqlServer(HelperConnection.GetConnectionSQL(configuration, Entities.Constants.KeyVault.SQLDataBaseREC, isDev), sqlOptions => { sqlOptions.EnableRetryOnFailure(3, System.TimeSpan.FromSeconds(30), null); sqlOptions.CommandTimeout(60); });
            });

            services.AddDbContext<Repositories.AdministracionContext>(options =>
            {
                options.UseSqlServer(HelperConnection.GetConnectionSQL(configuration, Entities.Constants.KeyVault.SQLDataBaseADM, isDev), sqlOptions => { sqlOptions.EnableRetryOnFailure(3, System.TimeSpan.FromSeconds(30), null); sqlOptions.CommandTimeout(60); });
            });

            services.AddScoped<Repositories.EcoOracleContext>();

            services.AddTransient<ICampoRepository, CampoRepository>();
            services.AddTransient<ICommonsRepository, CommonsRepository>();
            services.AddTransient<ICampoContratoRepository, CampoRepository>();
            services.AddTransient<ICompaniaRepository, CompaniaRepository>();
            services.AddTransient<IContratoRepository, ContratoRepository>();
            services.AddTransient<IFacilidadRepository, FacilidadRepository>();
            services.AddTransient<IPozoRepository, PozoRepository>();
            services.AddTransient<IYacimientoRepository, YacimientoRepository>();
            services.AddTransient<IPozoYacimientoRepository, PozoYacimientoRepository>();
            services.AddTransient<ISistemaTransporteRepository, SistemaTransporteRepository>();
            services.AddTransient<IAprobacionRuletaMasivianRepository, AprobacionRuletaMasivianRepository>();
            services.AddTransient<IReglasRepository, ReglasRepository>();
            services.AddTransient<IInterestRepository, InterestRepository>();
            services.AddTransient<ISartaRepository, SartaRepository>();
            services.AddTransient<IRolRepository, RolRepository>();
        }
    }
}
