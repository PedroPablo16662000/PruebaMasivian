using Backend.RuletaMasivian.Utilities;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.RuletaMasivian.Repositories
{
    public class EcoOracleContext
    {
        /// <summary>
        /// The connection string
        /// </summary>
        public readonly string ConnectionString;

        /// <summary>
        /// The telemetry exception
        /// </summary>
        private readonly Utilities.Telemetry.ITelemetryException TelemetryException;

        /// <summary>
        /// Initializes a new instance of the <see cref="EcoOracleContext" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="telemetryException">The telemetry exception.</param>
        public EcoOracleContext(IConfiguration configuration,
                                Utilities.Telemetry.ITelemetryException telemetryException)
        {
            ConnectionString = configuration.GetValue<string>(Entities.Constants.KeyVault.ORACLEConnection);
            TelemetryException = telemetryException;
        }

        /// <summary>
        /// Executes the procedure.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public async Task<Entities.Responses.ResponseBase<List<T>>> ExecuteProcedure<T>(string name, params OracleParameter[] parameters)
        {
            using OracleConnection oracleConnection = new OracleConnection(ConnectionString);
            try
            {
                using OracleCommand oracleCommand = new OracleCommand(name)
                {
                    Connection = oracleConnection,
                    CommandType = CommandType.StoredProcedure
                };

                await oracleConnection.OpenAsync();

                if (parameters != null && parameters.Any())
                {
                    oracleCommand.Parameters.AddRange(parameters);
                }

                using OracleDataAdapter oracleDataAdapter = new OracleDataAdapter
                {
                    SelectCommand = oracleCommand
                };

                DataTable dataTable = new DataTable();
                oracleDataAdapter.Fill(dataTable);

                var lstRespuesta = dataTable.ToDynamic<T>();

                return new Entities.Responses.ResponseBase<List<T>>(data: lstRespuesta);
            }
            catch (Exception exc)
            {
                TelemetryException.RegisterException(exc);
                return new Entities.Responses.ResponseBase<List<T>>(System.Net.HttpStatusCode.InternalServerError, exc.Message);
            }
            finally
            {
                if (oracleConnection != null)
                {
                    await oracleConnection.CloseAsync();
                    await oracleConnection.DisposeAsync();
                }
            }
        }

        public async Task<Entities.Responses.ResponseBase<List<T>>> ExecuteProcedureOut<T>(string name, string parameterOut, params OracleParameter[] parameters)
        {
            using OracleConnection oracleConnection = new OracleConnection(ConnectionString);
            try
            {
                using OracleCommand oracleCommand = new OracleCommand(name)
                {
                    Connection = oracleConnection,
                    CommandType = CommandType.StoredProcedure
                };

                await oracleConnection.OpenAsync();

                if (parameters != null && parameters.Any())
                {
                    oracleCommand.Parameters.AddRange(parameters);
                }

                using OracleDataAdapter oracleDataAdapter = new OracleDataAdapter
                {
                    SelectCommand = oracleCommand
                };

                DataTable dataTable = new DataTable();
                oracleDataAdapter.Fill(dataTable);

                var lstRespuesta = dataTable.ToDynamic<T>();

                var response = oracleCommand.Parameters[parameterOut].Value.ToString();

                return new Entities.Responses.ResponseBase<List<T>>(data: lstRespuesta, message: response);
            }
            catch (Exception exc)
            {
                TelemetryException.RegisterException(exc);
                return new Entities.Responses.ResponseBase<List<T>>(System.Net.HttpStatusCode.InternalServerError, exc.Message);
            }
            finally
            {
                if (oracleConnection != null)
                {
                    await oracleConnection.CloseAsync();
                    await oracleConnection.DisposeAsync();
                }
            }
        }
    }
}
