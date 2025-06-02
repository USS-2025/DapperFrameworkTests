namespace OracleTests.Utils
{
    public class DatabaseOptions
    {
        public const string Database = nameof(Database);

        public string DbName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string ServiceName { get; set; } = string.Empty;
    }
}
