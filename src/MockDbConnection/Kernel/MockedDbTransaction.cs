using System.Data;
using System.Data.Common;

namespace MockDBConnection.Kernel
{
    internal class MockedDbTransaction : DbTransaction
    {
        private readonly DbConnection _connection;
        private readonly IsolationLevel _isoltationLevel;
        private readonly IDbLogger _logger;

        public MockedDbTransaction(DbConnection connection, IsolationLevel isoltationLevel, IDbLogger logger)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _isoltationLevel = isoltationLevel;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogTransaction("new", isoltationLevel.ToString());
        }

        public override IsolationLevel IsolationLevel => _isoltationLevel;

        protected override DbConnection? DbConnection => _connection;

        public override void Commit()
        {
            _logger.LogTransaction(nameof(Commit));
        }

        public override void Rollback()
        {
            _logger.LogTransaction(nameof(Rollback));
        }
    }
}