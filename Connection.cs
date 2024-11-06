using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement
{
    class Connection
    {
        SqlConnection conn;
        
        public SqlConnection getCon()
        {
            conn = new SqlConnection("Data Source=DESKTOP-5RV2ANH;Initial Catalog=OfficeManagement;Integrated Security=True");
          
            return conn;
        }
    }
}
