using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

using Newtonsoft.Json;


namespace ReadFromTextFile_CallWebAPI_01
{
    class Program
    {
        //Entry point
        static void Main(string[] args)
        {
            try
            {
                Program pgmobj = new Program();

                //call method to get file information
                var FileInfo = pgmobj.GetFileInfo();

                //call method to parse file data into list object, pass file information as param
                var EmployeeList = pgmobj.ParseFileData(FileInfo);

                //call webservice to insert data into database table

                pgmobj.WebServiceCall(EmployeeList);

                //Once process is complete
                Console.WriteLine("Application run successful!");
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();
        }

        //Method to get the file information
        public string GetFileInfo()
        {
            try
            {
                return ConfigurationSettings.AppSettings["InputFilePath"] + "\\" + ConfigurationSettings.AppSettings["InputFileName"];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        //Method to parse the input file data into list object
        public List<EmployeeModel> ParseFileData(string FileInfo)
        {
            EmployeeModel EmpMdlObj = new EmployeeModel();
            var EmployeeModelList = new List<EmployeeModel>();

            try
            {
                var lines = File.ReadAllLines(FileInfo);
                foreach (var line in lines)
                {
                    var linedata = line.Split(',');
                    EmpMdlObj.EmployeeID = linedata[0];
                    EmpMdlObj.EmployeeName = linedata[1];
                    EmpMdlObj.EmployeeDOB = Convert.ToDateTime(linedata[2]);
                    EmpMdlObj.EmployeeCity = linedata[3];

                    EmployeeModelList.Add(EmpMdlObj);
                }

                return EmployeeModelList;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }

        }


        //Method to call the webservice to connect to Database
        public void WebServiceCall(List<EmployeeModel> data)
        {
            HttpClient clientobj = new HttpClient();

            clientobj.BaseAddress = new Uri(ConfigurationSettings.AppSettings["WebServiceURL"]);

            clientobj.DefaultRequestHeaders.Accept.Clear();
            clientobj.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

          

            var jsonObj = JsonConvert.SerializeObject(data);

            var response = clientobj.PostAsync("api/DataTable", new StringContent(jsonObj, Encoding.UTF8, "application/json")).Result;

            Console.ReadKey();

        }


    }
}
