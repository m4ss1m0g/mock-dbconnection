using System.Data.Common;
using MockDBConnection.Kernel;

namespace MockDBConnection.Loggers
{
    public class DbLogItem
    {
        public enum LogType
        {
            LogUnknow,
            LogConnection,
            LogCommand,
            LogParameter,
            LogTransaction
        }

        public DateTime DateTime { get; }

        public LogType Type { get; set; }

        public string Message { get; set; }

        public string? Value { get; set; }

        public DbLogItem(LogType logType, string message, string? value)
        {
            DateTime = DateTime.Now;
            Type = logType;
            Message = message;
            Value = value;
        }
    }

    public class ListLogger : IDbLogger
    {
        private readonly List<DbLogItem> _log = new();

        private readonly List<DbParameter> _parameters = new();

        public void LogCommand(string message, string? value = null)
        {
            _log.Add(new DbLogItem(DbLogItem.LogType.LogCommand, message, value));
        }

        public void LogConnection(string message, string? value = null)
        {
            _log.Add(new DbLogItem(DbLogItem.LogType.LogConnection, message, value));
        }

        public void LogParameter(DbParameter parameter)
        {
            var value = $"Name: {parameter.ParameterName} Value: {parameter.Value} IsNullable: {parameter.IsNullable}";
            _log.Add(new DbLogItem(DbLogItem.LogType.LogParameter, "DbParameter", value));
            _parameters.Add(parameter);
        }

        public void LogTransaction(string message, string? value = null)
        {
            _log.Add(new DbLogItem(DbLogItem.LogType.LogTransaction, message, value));
        }

        public IEnumerable<DbLogItem> Log => _log;

        public IEnumerable<DbParameter> Parameters => _parameters;
    }
}