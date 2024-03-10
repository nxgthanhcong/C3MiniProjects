namespace Translation.Models.DataModels
{
    public class PostgreDatabaseConfiguration
    {
        public const string Position = "Database:Postgre";

        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string SSLMode { get; set; }
        public string Database { get; set; }

        public override string ToString()
        {
            return $"Host={Host};Username={Username};Password={Password};Database={Database};sslmode={SSLMode}";
        }
    }
}
