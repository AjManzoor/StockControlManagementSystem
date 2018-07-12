using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSM.Data.Respositories
{
 
    public class BaseRespository
    {
        private string server;
        private string databases;
        private string uid;
        private string password;

        //connection string for all my different database classes. All database classes should inherit from this
        public string GenerateConnectionString()
        {
            server = "localhost";
            databases = "SCMS";
            uid = "root";
            password = "password";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            databases + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            return connectionString; ;
        }
    }
}
