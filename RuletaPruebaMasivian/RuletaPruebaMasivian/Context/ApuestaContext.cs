using Microsoft.Extensions.Configuration;
using RuletaPruebaMasivian.Interface.IContext;
using RuletaPruebaMasivian.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ApuestaPruebaMasivian.Context
{
    public class ApuestaContext : IApuestaContext
    {
        private IConfiguration _Configuration;
        private string _connectionSQL;
        public ApuestaContext(IConfiguration configuration)
        {
            _Configuration = configuration;
            _connectionSQL = Microsoft
               .Extensions
               .Configuration
               .ConfigurationExtensions
               .GetConnectionString(this._Configuration, "DefaultConnection");
        }
        public int Add(Apuesta apuesta)
        {
            int id = -1;
            SqlConnection conn = new SqlConnection(_connectionSQL);
            SqlCommand command = new SqlCommand($"insert into Apuestas(numero,color,valorApostado,idRuleta,idUsuario,fechaApuesta) OUTPUT INSERTED.idApuesta values ('{apuesta.numero}','{apuesta.color}','{apuesta.valorApostado}','{apuesta.idRuleta}','{apuesta.idUsuario}','{DateTime.Now}')", conn);
            conn.Open();
            command.CommandType = System.Data.CommandType.Text;
            id = (int)command.ExecuteScalar();
            conn.Close();

            return id;
        }
        public bool HasUserEnoughtMoney(Apuesta apuesta)
        {
            SqlConnection conn = new SqlConnection(_connectionSQL);
            SqlCommand command = new SqlCommand($"select case when r.dineroPorGanar <= u.dineroAFavor then 'Y' else 'N' end from Usuarios u inner join Ruletas r on .idRuleta = '{apuesta.idRuleta}' where idUsuario = '{apuesta.idUsuario}'')", conn);
            conn.Open();
            command.CommandType = System.Data.CommandType.Text;
            bool enought = command.ExecuteScalar().ToString().Equals("Y");
            conn.Close();

            return enought;
        }
    }
}
