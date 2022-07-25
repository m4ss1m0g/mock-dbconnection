using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace MockDBConnection.Kernel
{
    public class MockedDbConnection : DbConnection
    {
        private string _databaseName = "FakeDB";
        private ConnectionState _state;
        private readonly IDbLogger _logger;
        private readonly DataTable _dataTable;

        public MockedDbConnection(IDbLogger logger, string? connectionString = null, DataTable? dataTable = null)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dataTable = dataTable ?? new DataTable();
            ConnectionString = connectionString;
            _state = ConnectionState.Closed;
            _logger.LogConnection("new");
        }

        [AllowNull]
        public override string ConnectionString { get; set; }

        public override string Database => _databaseName;

        public override string DataSource => "FakeDataSource";

        public override string ServerVersion => "0.0.1";

        public override ConnectionState State => _state;

        public override void ChangeDatabase(string databaseName)
        {
            _logger.LogConnection(nameof(ChangeDatabase), databaseName);
            _databaseName = databaseName;
        }

        public override void Close()
        {
            _logger.LogConnection(nameof(Close));
            _state = ConnectionState.Closed;
        }

        public override void Open()
        {
            _logger.LogConnection(nameof(Open));
            _state = ConnectionState.Open;
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            _logger.LogConnection(nameof(BeginDbTransaction));
            return new MockedDbTransaction(this, isolationLevel, _logger);
        }

        protected override DbCommand CreateDbCommand()
        {
            _logger.LogConnection(nameof(CreateDbCommand));
            return new MockedDbCommand(_dataTable, _logger);
        }

    }

}