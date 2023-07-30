using System.Collections.Generic;

namespace Core.ServiceMetadata.Common;

public class Database : ServiceMetadataBase, IDatabase
{
    public string Dsn {
        get
        {
            var dsn = Get<string>("DATABASE_DSN");
            if (!string.IsNullOrEmpty(dsn))
            {
                return dsn;
            }
            return string.Join(";", new List<string>
            {
                "server=" + Get<string>("DB_SERVER"),
                "port=" + Get<int>("DB_PORT"),
                "user=" + Get<string>("DB_USER"),
                "password=" + Get<string>("DB_PASS"),
                "database=" + Get<string>("DB_NAME"),
            });
        }
    }
}