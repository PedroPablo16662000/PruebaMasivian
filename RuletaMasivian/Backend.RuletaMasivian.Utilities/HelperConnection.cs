using Microsoft.Extensions.Configuration;

namespace Backend.RuletaMasivian.Utilities
{
    /// <summary>
    /// Helper Connection
    /// </summary>
    public static class HelperConnection
    {
        /// <summary>
        /// Gets the connection SQL.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="isDevelopment">if set to <c>true</c> [is development].</param>
        /// <returns></returns>
        public static string GetConnectionSQL(IConfiguration configuration, string databaseName, bool isDevelopment = false)
        {
            var database = configuration.GetValue<string>(databaseName);

            if (database.Contains("Server="))
            {
                return database;
            }
            isDevelopment = false;
            var server = configuration.GetValue<string>($"{Entities.Constants.KeyVault.SQLServer}{(isDevelopment ? "DEV" : string.Empty)}");
            var user = configuration.GetValue<string>(Entities.Constants.KeyVault.SQLUser);
            var pwd = configuration.GetValue<string>(Entities.Constants.KeyVault.SQLSecure);

            return string.Format("Server={0};Database={1};User ID={2};Password={3};Trusted_Connection=False;Encrypt=True;",
                server,
                database,
                user,
                pwd);
        }
    }
}