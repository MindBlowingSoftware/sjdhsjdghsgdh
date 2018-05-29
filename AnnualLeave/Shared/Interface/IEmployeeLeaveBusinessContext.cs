using System;
using AnnualLeave.Shared.Model;

namespace AnnualLeave.Shared.Interface
{
    public interface IEmployeeLeaveBusinessContext
    {
        void ProcessLeaveRequest(DateTime leaveStartDate, int days, string reason, int employeeId);
        Employee FindEmployee(int employeeId);
    }
}