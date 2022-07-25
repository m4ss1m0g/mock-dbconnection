using System.Data;
using System.Data.Common;
using Dapper;
using MockDBConnection.Kernel;
using MockDBConnection.Loggers;
using Moq;

namespace MockDbConnection.Test.Kernel
{
    public class MockDbConnectionTest
    {
        [Fact]
        public void Should_create_connection()
        {
            // Arrange
            var log = new Mock<IDbLogger>();

            // Act
            var conn = new MockedDbConnection(log.Object);

            // Assert
            Assert.NotNull(conn);
        }

        [Fact]
        public void Should_execute_command_with_dapper()
        {
            // Arrange
            var log = new Mock<IDbLogger>();
            IDbConnection conn = new MockedDbConnection(log.Object);
            var sql = "SELECT TITLE FROM CUSTOMERS WHERE ID = @Id AND EMAIL = @EMail";

            // Act
            var result = conn.Query<string>(sql, new { Id = 1, EMail = "guest@nobody.com" });

            // Assert
            Assert.NotNull(result);
            log.Verify(p => p.LogCommand(It.IsAny<string>(), It.IsAny<string?>()), Times.AtLeastOnce);
            log.Verify(p => p.LogConnection(It.IsAny<string>(), It.IsAny<string?>()), Times.AtLeastOnce);
            log.Verify(p => p.LogTransaction(It.IsAny<string>(), It.IsAny<string?>()), Times.Never);
            log.Verify(p => p.LogParameter(It.IsAny<DbParameter>()), Times.AtLeastOnce);
        }

        [Fact]
        public void Should_return_fake_datatable()
        {
            var log = new Mock<IDbLogger>();
            var dt = new DataTable("customers");
            dt.Columns.Add("Title", typeof(string));
            dt.Rows.Add("Customer 1");
            dt.Rows.Add("Customer 2");
            dt.Rows.Add("Customer 3");

            IDbConnection conn = new MockedDbConnection(log.Object, null, dt);
            var sql = "SELECT TITLE FROM CUSTOMERS WHERE ID = @Id AND EMAIL = @EMail";

            // Act
            var result = conn.Query<string>(sql, new { Id = 1, EMail = "guest@nobody.com" });

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(3, result.Count());

            log.Verify(p => p.LogCommand(It.IsAny<string>(), It.IsAny<string?>()), Times.AtLeastOnce);
            log.Verify(p => p.LogConnection(It.IsAny<string>(), It.IsAny<string?>()), Times.AtLeastOnce);
            log.Verify(p => p.LogTransaction(It.IsAny<string>(), It.IsAny<string?>()), Times.Never);
            log.Verify(p => p.LogParameter(It.IsAny<DbParameter>()), Times.AtLeastOnce);
        }


        [Fact]
        public void Should_check_the_fake_command_and_parameters()
        {
            var lst = new ListLogger();
            IDbConnection conn = new MockedDbConnection(lst);

            var sql = "SELECT TITLE FROM CUSTOMERS WHERE ID = @Id AND EMAIL = @EMail";
            var parms = new { Id = 1, EMail = "guest@nobody.com" };

            // Act
            var result = conn.Query<string>(sql, parms);

            // Assert
            Assert.NotNull(result);

            var cmd = lst.Log.Where(p => p.Type == DbLogItem.LogType.LogCommand && p.Message == "ExecuteDbDataReader").First();
            Assert.Equal(sql, cmd.Value);

            Assert.Equal(parms.EMail, lst.Parameters.First(p => p.ParameterName == nameof(parms.EMail)).Value);
            Assert.Equal(parms.Id, lst.Parameters.First(p => p.ParameterName == nameof(parms.Id)).Value);
        }
    }
}