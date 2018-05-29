using AnnualLeave.Shared.Model;

namespace AnnualLeave.Shared.Interface
{
    public interface IEmployeeLeaveDataContext
    {
        void ProcessLeaveRequest(EmployeeLeaveRequest leaveRequest);
        Employee FindEmployee(int employeeId);
    }
}