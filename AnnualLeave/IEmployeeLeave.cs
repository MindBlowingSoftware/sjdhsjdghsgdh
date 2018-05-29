using System;

namespace AnnualLeave
{
    public interface IEmployeeLeave
    {
        void ProcessLeaveRequest(DateTime leaveStartDate, int days, string reason, int employeeId);
        Employee FindEmployee(int employeeId);
    }
}