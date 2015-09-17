using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using EmployeeModelProject_Web;

namespace WebLogicToConnectDatabaseTable
{
    public class ConnectDataBase
    {
        public void InsertInto(List<EmployeeModelClass_Web> consoledata)
        {
            var connstring = ConfigurationSettings.AppSettings["ConnectionString"];
            try
            {
                using(var connopen = new SqlConnection(connstring))
                {
                    using(var cmd = new SqlCommand())
                    {
                        cmd.CommandText = ConfigurationSettings.AppSettings["StoredProcName"];
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = connopen;

                        connopen.Open();

                        foreach(var datalist in consoledata)
                        {
                            EmployeeModelClass_Web modelobj = new EmployeeModelClass_Web();
                            var parameters = new[]
                            {
                                new SqlParameter("@EmployeeID", modelobj.EmployeeID),
                                new SqlParameter("@FullName", modelobj.EmployeeName),
                                new SqlParameter("@EmployeeDOB", modelobj.EmployeeDOB),
                                new SqlParameter("@EmployeeCity", modelobj.EmployeeCity)
                            };

                            foreach(var param in parameters)
                            {
                                cmd.Parameters.Add(param);
                            }

                            cmd.ExecuteNonQuery();

                            cmd.Parameters.Clear();
                        }

                        



                    }
                }







            }

            catch(Exception ex)
            {
                throw ex;
            }

        }
    }
}
