using System.Collections.Generic;
using System.Linq;
using AnnualLeave.Shared.Interface;
using AnnualLeave.Shared.Model;
using NUnit.Framework;

namespace AnnualLeave.Test
{
    public class FakeEmployeeLeaveDataContext : IEmployeeLeaveDataContext
    {
        public List<EmployeeLeaveRequest> EmployeeLeaveStore { get; set; }
        public List<Employee> Employees { get; set; }
        public void ProcessLeaveRequest(EmployeeLeaveRequest leaveRequest)
        {
            EmployeeLeaveStore.Add(leaveRequest);
        }

        public Employee FindEmployee(int employeeId)
        {
            return Employees.Single(e => e.EmployeeId == employeeId);
        }
    }
}