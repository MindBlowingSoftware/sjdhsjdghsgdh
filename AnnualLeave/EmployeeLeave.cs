using System;
using log4net;

namespace AnnualLeave
{
    public class EmployeeLeave : IEmployeeLeave
    {
        private readonly ILog _log;
        private readonly IEmployeeLeaveBusinessContext _businessContext;

        public EmployeeLeave(ILog log, IEmployeeLeaveBusinessContext businessContext)
        {
            _log = log;
            _businessContext = businessContext;
        }
        public void ProcessLeaveRequest(DateTime leaveStartDate, int days, string reason, int employeeId)
        {
            _businessContext.ProcessLeaveRequest(leaveStartDate,days,reason,employeeId);
        }

        public Employee FindEmployee(int employeeId)
        {
            return _businessContext.FindEmployee(employeeId);
        }
    }
}