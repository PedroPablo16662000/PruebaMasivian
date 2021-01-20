using System;
using System.Collections.Generic;

namespace Backend.RuletaMasivian.Utilities
{
    public static class SessionHelper
    {
        public static List<T> AddUserAndDate<T>(ref T entidadAGuardar, string usuario)
        {
            List<T> salida = new List<T>();
            entidadAGuardar.GetType().GetProperty("RowCreatedBy").SetValue(entidadAGuardar, usuario);
            entidadAGuardar.GetType().GetProperty("RowCreatedDate").SetValue(entidadAGuardar, DateTime.Now);
            entidadAGuardar.GetType().GetProperty("RowChangedBy").SetValue(entidadAGuardar, usuario);
            entidadAGuardar.GetType().GetProperty("RowChangedDate").SetValue(entidadAGuardar, DateTime.Now);
            salida.Add(entidadAGuardar);
            return salida;
        }

        public static List<T> AddUserAndDateUpdate<T>(ref T entidadAGuardar, string usuario)
        {
            List<T> salida = new List<T>();
            entidadAGuardar.GetType().GetProperty("RowCreatedBy").SetValue(entidadAGuardar, usuario);
            entidadAGuardar.GetType().GetProperty("RowCreatedDate").SetValue(entidadAGuardar, DateTime.Now);
            entidadAGuardar.GetType().GetProperty("RowChangedBy").SetValue(entidadAGuardar, usuario);
            entidadAGuardar.GetType().GetProperty("RowChangedDate").SetValue(entidadAGuardar, DateTime.Now);
            salida.Add(entidadAGuardar);
            return salida;
        }
    }
}