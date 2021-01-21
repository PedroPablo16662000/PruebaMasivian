using Microsoft.Extensions.Configuration;
using RuletaPruebaMasivian.Interface.IContext;
using RuletaPruebaMasivian.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RuletaPruebaMasivian.Context
{
    public class RuletaContext : IRuletaContext
    {
        private IConfiguration _Configuration;
        private string _connectionSQL;
        public RuletaContext(IConfiguration configuration)
        {
            _Configuration = configuration;
            _connectionSQL = Microsoft
               .Extensions
               .Configuration
               .ConfigurationExtensions
               .GetConnectionString(this._Configuration, "DefaultConnection");
        }
        public bool Exists(int ruleta)
        {

            return true;
        }
        public int Add(Ruleta ruleta)
        {
            int id = -1;
            SqlConnection conn = new SqlConnection(_connectionSQL);
            SqlCommand command = new SqlCommand($"insert into Ruletas(fechaInicial,fechaFinal,marca,observacion,dineroPorGanar, estadoActual) OUTPUT INSERTED.idRuleta values (null,null,'{ruleta.marca}','{ruleta.observacion}','{ruleta.dineroPorGanar}','0')", conn);
            conn.Open();
            command.CommandType = System.Data.CommandType.Text;
            id = (int)command.ExecuteScalar();
            conn.Close();

            return id;
        }
        public bool Open(int ruleta)
        {
            bool estado = false;
            SqlConnection conn = new SqlConnection(_connectionSQL);
            SqlCommand command = new SqlCommand($"update Ruletas set fechaInicial = '{DateTime.Now}', estadoActual=1 where idRuleta={ruleta}", conn);
            conn.Open();
            command.CommandType = System.Data.CommandType.Text;
            estado = command.ExecuteNonQuery() == 1;
            conn.Close();

            return estado;
        }
        public bool Close(int ruleta)
        {
            bool estado = false;
            SqlConnection conn = new SqlConnection(_connectionSQL);
            SqlCommand command = new SqlCommand($"update Ruletas set fechaFinal = '{DateTime.Now}', estadoActual=0 where idRuleta={ruleta}", conn);
            conn.Open();
            command.CommandType = System.Data.CommandType.Text;
            estado = command.ExecuteNonQuery() == 1;
            conn.Close();

            return estado;
        }
    }
}
