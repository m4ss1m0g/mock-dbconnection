# Fake DbConnection

This project aim to mock the Dbconnection to test your query WITHOUT a database.

I wrote this tool because I found that on many occasions is not a good idea and also is not possible to test your SQL commands against an in-memory provider like SQLite.

Sometimes you want to test: transactions, stored procedures, a DB schema, or a SQL dialect not supported by SQLite, in these cases, is almost impossible or very hard to create a valid SQLite database that can fit your tests.

Another good point for not testing with in-memory DB came from Jimmy Bogard, I suggest you to read this article: [Avoid In-Memory Databases for Tests](https://jimmybogard.com/avoid-in-memory-databases-for-tests/)

In the situation where we cannot test with SQLite, what we can do, at least, is to check if the command is correctly created.

## How to

1. Implement the `IDbLogger` to pass to the instance of `MockedDbConnection` OR use the `ListLogger` that log on a List.
2. Create a new instance the `FakedDbConnection`
3. Call your code
4. Check the result

``` csharp

    // Arrange
    var lst = new ListLogger();
    IDbConnection conn = new MockedDbConnection(lst);

    var sql = "SELECT TITLE FROM CUSTOMERS WHERE ID = @Id AND EMAIL = @EMail";
    var parms = new { Id = 1, EMail = "guest@nobody.com" };

    // Act
    var result = conn.Query<string>(sql, parms);

    // Assert
    Assert.NotNull(result); // return an empty list since theres no DataTable passed to the DbConnection constructor

    // Verify the SQL Command
    var cmd = lst.Log.Where(p => p.Type == DbLogItem.LogType.LogCommand && p.Message == "ExecuteDbDataReader").First();
    Assert.Equal(sql, cmd.Value);

    // Verify the Parameters
    Assert.Equal(parms.EMail, lst.Parameters.First(p => p.ParameterName == nameof(parms.EMail)).Value);
    Assert.Equal(parms.Id, lst.Parameters.First(p => p.ParameterName == nameof(parms.Id)).Value);
```

### Optional

You can also pass a DataTable if you want to mock  the results.

## Logger

You can write your own IDbLogger implementation to retrive only what you want

``` csharp
    public interface IDbLogger
    {
        void LogParameter(DbParameter parameter);
        
        void LogConnection(string message, string? value = null);
        
        void LogTransaction(string message, string? value = null);

        void LogCommand(string message, string? value = null);
    }
```
