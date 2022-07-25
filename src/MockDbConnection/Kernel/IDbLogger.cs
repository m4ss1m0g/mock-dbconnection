using System.Data.Common;

namespace MockDBConnection.Kernel
{
	public interface IDbLogger
	{
        void LogParameter(DbParameter parameter);
        
        void LogConnection(string message, string? value = null);
        
        void LogTransaction(string message, string? value = null);

        void LogCommand(string message, string? value = null);
    }
}