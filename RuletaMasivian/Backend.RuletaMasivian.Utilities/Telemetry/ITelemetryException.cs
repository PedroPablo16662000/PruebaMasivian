using System;

namespace Backend.RuletaMasivian.Utilities.Telemetry
{
    public interface ITelemetryException
    {
        void RegisterException(Exception exception);
    }
}