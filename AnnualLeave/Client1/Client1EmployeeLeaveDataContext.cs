using System;
using System.Data.SqlClient;
using AnnualLeave.Shared.Interface;
using AnnualLeave.Shared.Model;
using log4net;

namespace AnnualLeave.Client1
{
    public class Client1EmployeeLeaveDataContext : IEmployeeLeaveDataContext
    {
        private readonly ILog _log;

        public Client1EmployeeLeaveDataContext(ILog log)
        {
            _log = log;
        }
        public void ProcessLeaveRequest(EmployeeLeaveRequest leaveRequest)
        {
            SaveLeaveRequest(leaveRequest);
        }

        private static void SaveLeaveRequest(EmployeeLeaveRequest leaveRequest)
        {
            var sqlConnection = new SqlConnection("Data Source=local;Initial Catalog=Employee;Integrated Security=True");
            sqlConnection.Open();
            var cmd = new SqlCommand
            {
                CommandText =
                    "Insert into EmployeeLeave (EmployeeId, StartDateTime, EndDateTime) values ('@EmployeeId','@StartDateTime', '@EndDateTime')"
            };
            cmd.Parameters.AddWithValue("EmployeeId", leaveRequest.EmployeeId);
            cmd.Parameters.AddWithValue("StartDateTime", leaveRequest.LeaveStartDateTime);
            cmd.Parameters.AddWithValue("EndDateTime", leaveRequest.LeaveEndDateTime);
            cmd.ExecuteNonQuery();
        }

        public Employee FindEmployee(int employeeId)
        {
            var sqlConnection = new SqlConnection("Data Source=local;Initial Catalog=Employee;Integrated Security=True");
            var sql = "SELECT * from Employee WHERE EmployeeID=" + employeeId;
            var sqlCommand = new SqlCommand(sql, sqlConnection);
            sqlConnection.Open();
            var sqlReader = sqlCommand.ExecuteReader();

            Employee employee = null;
            if (sqlReader.Read())
            {
                employee = new Employee();

                employee.EmployeeId = int.Parse(sqlReader["EmployeeId"].ToString());
                employee.Name = sqlReader["Name"].ToString();
                employee.LastName = sqlReader["LastName"].ToString();
                employee.ContactStartDate = DateTime.Parse(sqlReader["StartDate"].ToString());
                employee.Salary = Decimal.Parse(sqlReader["Salary"].ToString());    
            }
            return employee;
        }
    }
}
