using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace PlantServiceTest;

public class TestableDbContextOptionsBuilder<TContext> : DbContextOptionsBuilder<TContext> where TContext : DbContext
{
    public string ConnectionString;
    public ServerVersion ServerVersion;
    public Action<MySqlDbContextOptionsBuilder> MySqlOptionsAction;
    
    public DbContextOptionsBuilder<TUContext> UseMySql<TUContext>(
        string connectionString,
        ServerVersion serverVersion,
        Action<MySqlDbContextOptionsBuilder> mySqlOptionsAction = null)
        where TUContext : DbContext
    {
        ConnectionString = connectionString;
        ServerVersion = serverVersion;
        MySqlOptionsAction = mySqlOptionsAction;
        return new DbContextOptionsBuilder<TUContext>();
    }
}