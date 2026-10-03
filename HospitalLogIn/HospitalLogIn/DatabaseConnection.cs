using System.Data.SqlClient;

namespace HospitalBillingSystem
{
    public class DatabaseConnection
    {
        private readonly string connectionString =
            @"Server=.\SQLEXPRESS;Database=HospitalBillingDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}