using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace MockDBConnection.Kernel
{
    internal class MockedDbCommand : DbCommand
    {
        private readonly DataTable _dataTable;
        private readonly IDbLogger _logger;
        private readonly MockedDataParameterCollection _collection;

        public MockedDbCommand(DataTable dataTable, IDbLogger logger)
        {
            _dataTable = dataTable ?? throw new ArgumentNullException(nameof(dataTable));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _collection = new MockedDataParameterCollection();
            CommandText = string.Empty;
            _logger.LogCommand("new");
        }

        [AllowNull]
        public override string CommandText { get; set; }
        public override int CommandTimeout { get; set; }
        public override CommandType CommandType { get; set; }
        public override bool DesignTimeVisible { get; set; }
        public override UpdateRowSource UpdatedRowSource { get; set; }
        protected override DbConnection? DbConnection { get; set; }
        protected override DbParameterCollection DbParameterCollection => _collection;
        protected override DbTransaction? DbTransaction { get; set; }

        public override void Cancel()
        {
            _logger.LogCommand(nameof(Cancel));
        }

        public override int ExecuteNonQuery()
        {
            _logger.LogCommand(nameof(ExecuteNonQuery), CommandText);
            return 0;
        }

        public override object? ExecuteScalar()
        {
            _logger.LogCommand(nameof(ExecuteScalar), CommandText);
            return null;
        }

        public override void Prepare()
        {
            _logger.LogCommand(nameof(Prepare));
        }

        protected override DbParameter CreateDbParameter()
        {
            _logger.LogCommand(nameof(CreateDbParameter));
            return new MockedDbParameter();
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            _logger.LogCommand(nameof(ExecuteDbDataReader), CommandText);
            
            foreach (MockedDbParameter item in _collection)
            {
                _logger.LogParameter(item);
            }

            return _dataTable.CreateDataReader();
        }
    }
}