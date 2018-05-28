using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnnualLeave;

namespace AnnualLeave
{
    public class EmployeeLeave
    {
        public void ProcessLeaveRequest(DateTime leaveStartDate, int days, string reason, int employeeId)
        {
            var employee = FindEmployee(employeeId);
            
            if((DateTime.Now - employee.ContactStartDate).TotalDays <= 90 && !employee.IsMarried)
                throw new Exception("Invalid leave request.");

            if(days > 20)
                throw new Exception("Invalid leave request.");

            var leaveRequest = new EmployeeLeaveRequest();
            leaveRequest.EmployeeId = employeeId;
            leaveRequest.LeaveStartDateTime = leaveStartDate;
            leaveRequest.LeaveEndDateTime = leaveStartDate.AddDays(days);
        }

        public void SaveLeaveRequest(EmployeeLeaveRequest leaveRequest)
        {
            var sqlConnection = new SqlConnection("Data Source=local;Initial Catalog=Employee;Integrated Security=True");
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Insert into EmployeeLeave (EmployeeId, StartDateTime, EndDateTime) values ('@EmployeeId','@StartDateTime', '@EndDateTime')";
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
