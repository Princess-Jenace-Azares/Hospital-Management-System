using System.Data.SqlClient;

namespace HospitalLogIn
{
    public class DatabaseConnection
    {
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(
                @"Data Source=JINIZ\SQLEXPRESS;Initial Catalog=HospitalManagementDB;Integrated Security=True"
            );
        }
    }
}