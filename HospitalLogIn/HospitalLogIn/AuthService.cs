using System;

namespace HospitalBillingSystem
{
    public class AuthService
    {
        public bool Login(string username, string password, string role)
        {
            if (role == "Admin")
            {
                return username == "admin" && password == "admin123";
            }
            else if (role == "Billing Staff")
            {
                return username == "billing" && password == "billing123";
            }
            else if (role == "Attendant")
            {
                return username == "attendant" && password == "attendant123";
            }

            return false;
        }
    }
}