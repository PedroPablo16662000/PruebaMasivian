namespace Backend.RuletaMasivian.Utilities
{
    public static class ManejadorExcepciones
    {
        public static void ConversionMensaje(ref string mensaje)
        {
            switch (mensaje)
            {
                case string a when a.Contains("sarta") || a.Contains("SARTA"):
                    mensaje = "No se puede crear un recurso con la misma Sarta";
                    break;
                case string a when a.Contains("unique") || a.Contains("UNIQUE") || a.Contains("única") || a.Contains("duplicate") && (!a.Contains("sarta") || !a.Contains("SARTA")):
                    mensaje = "El registro que intenta insertar ya existe";
                    break;
                case string b when b.Contains("FOREIGN KEY") || b.Contains("foreign"):
                    mensaje = "El registro foráneo al que se refiere no existe";
                    break;
                default:
                    break;
            }
        }
    }
}
