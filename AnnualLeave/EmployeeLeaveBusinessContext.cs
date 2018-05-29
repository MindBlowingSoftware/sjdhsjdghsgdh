using System;
using log4net;

namespace AnnualLeave
{
    public class EmployeeLeaveBusinessContext : IEmployeeLeaveBusinessContext
    {
        private readonly ILog _log;
        private readonly IEmployeeLeaveDataContext _dataContext;

        public EmployeeLeaveBusinessContext(ILog log, IEmployeeLeaveDataContext dataContext)
        {
            _log = log;
            _dataContext = dataContext;
        }
        public void ProcessLeaveRequest(DateTime leaveStartDate, int days, string reason, int employeeId)
        {
            _dataContext.ProcessLeaveRequest(leaveStartDate, days, reason, employeeId);
        }

        public Employee FindEmployee(int employeeId)
        {
            return _dataContext.FindEmployee(employeeId);
        }
    }
}