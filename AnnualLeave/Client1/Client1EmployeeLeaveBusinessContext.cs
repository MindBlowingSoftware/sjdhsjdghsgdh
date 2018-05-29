using System;
using AnnualLeave.Shared.Interface;
using AnnualLeave.Shared.Model;
using log4net;

namespace AnnualLeave.Client1
{
    public class Client1EmployeeLeaveBusinessContext : IEmployeeLeaveBusinessContext
    {
        private readonly ILog _log;
        private readonly IEmployeeLeaveDataContext _dataContext;

        public Client1EmployeeLeaveBusinessContext(ILog log, IEmployeeLeaveDataContext dataContext)
        {
            _log = log;
            _dataContext = dataContext;
        }
        public void ProcessLeaveRequest(DateTime leaveStartDate, int days, string reason, int employeeId)
        {
            var employee = FindEmployee(employeeId);

            if ((DateTime.Now - employee.ContactStartDate).TotalDays <= 90 && !employee.IsMarried)
            {
                var ex = new Exception("Invalid leave request.");
                _log.Error(ex.Message, ex);
                return;
            }

            if (days > 20)
            {
                var ex = new Exception("Invalid leave request.");
                _log.Error(ex.Message, ex);
                return;
            }

            var leaveRequest = new EmployeeLeaveRequest
            {
                EmployeeId = employeeId,
                LeaveStartDateTime = leaveStartDate,
                LeaveEndDateTime = leaveStartDate.AddDays(days),
                IsApproved = true
            };

            _dataContext.ProcessLeaveRequest(leaveRequest);
        }

        public Employee FindEmployee(int employeeId)
        {
            return _dataContext.FindEmployee(employeeId);
        }
    }
}