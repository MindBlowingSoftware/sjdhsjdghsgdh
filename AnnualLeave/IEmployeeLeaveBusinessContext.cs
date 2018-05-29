using System;

namespace AnnualLeave
{
    public interface IEmployeeLeaveBusinessContext
    {
        void ProcessLeaveRequest(DateTime leaveStartDate, int days, string reason, int employeeId);
        Employee FindEmployee(int employeeId);
    }
}