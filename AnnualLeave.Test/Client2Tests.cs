using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnnualLeave.Client1;
using AnnualLeave.Client2;
using AnnualLeave.Shared.Interface;
using AnnualLeave.Shared.Model;
using NUnit.Framework;
using Rhino.Mocks;

namespace AnnualLeave.Test
{
    [TestFixture, ExcludeFromCodeCoverage]
    public class Client2Tests
    {
        private Employee _testEmployee;
        private EmployeeLeaveRequest _testEmployeeLeaveRequest;
        private CustomLog _log;
        private DateTime _leaveStartDate;
        private int _days;
        private string _reason;
        private int _employeeId;
        [TestFixtureSetUp]
        public void Setup()
        {
            _testEmployee = new Employee
            {
                ContactStartDate = DateTime.Today.AddDays(-90),
                EmployeeId = 1,
                IsMarried = true,
                LastName = "lastName",
                Name = "Name",
                Salary = 10000
            };

            _testEmployeeLeaveRequest = new EmployeeLeaveRequest()
            {
                EmployeeId = _testEmployee.EmployeeId,
                IsApproved = true,
                LeaveEndDateTime = DateTime.Today.AddDays(3),
                LeaveStartDateTime = DateTime.Today
            };
            _leaveStartDate = _testEmployeeLeaveRequest.LeaveStartDateTime;
            _days = (_testEmployeeLeaveRequest.LeaveEndDateTime - _testEmployeeLeaveRequest.LeaveStartDateTime).Days;
            _reason = "reason";
            _employeeId = _testEmployee.EmployeeId;

            _log = new CustomLog();
        }

        [Test]
        public void Client2_Find_Employee_Function_Returns_Employee()
        {
            //Arrange
            
            var businessContext = MockRepository.GenerateStub<IEmployeeLeaveBusinessContext>();
            businessContext.Stub(b => b.FindEmployee(_testEmployee.EmployeeId)).Return(_testEmployee);
            var sut = MockRepository.GenerateMock<Client2EmployeeLeave>(_log,businessContext);
            //Act

            var actualResponse = sut.FindEmployee(_testEmployee.EmployeeId);
            //Assert

            Assert.AreEqual(_testEmployee.EmployeeId, actualResponse.EmployeeId);
            Assert.AreEqual(_testEmployee.ContactStartDate, actualResponse.ContactStartDate);
            Assert.AreEqual(_testEmployee.IsMarried, actualResponse.IsMarried);
            Assert.AreEqual(_testEmployee.LastName, actualResponse.LastName);
            Assert.AreEqual(_testEmployee.Name, actualResponse.Name);
            Assert.AreEqual(_testEmployee.Salary, actualResponse.Salary);

        }

        [Test]
        public void Client2_Process_Employee_Function_Saves_Leave_Request_To_Db()
        {
            //Arrange

            var datacontext = MockRepository.GenerateStub<FakeEmployeeLeaveDataContext>();
            datacontext.EmployeeLeaveStore = new List<EmployeeLeaveRequest>();
            var employee = _testEmployee;
            employee.ContactStartDate = DateTime.Today.AddDays(-90);
            employee.IsMarried = true;
            datacontext.Employees = new List<Employee>() { employee };
            
            var businessContext = new Client2EmployeeLeaveBusinessContext(_log,datacontext);
           
            var sut = MockRepository.GenerateMock<Client2EmployeeLeave>(_log, businessContext);
            //Act

            sut.ProcessLeaveRequest(_leaveStartDate,_days,_reason,_employeeId);
            //Assert
            Assert.AreEqual(1, datacontext.EmployeeLeaveStore.Count);
            Assert.AreEqual(_testEmployeeLeaveRequest.EmployeeId, datacontext.EmployeeLeaveStore[0].EmployeeId);
            Assert.AreEqual(_testEmployeeLeaveRequest.LeaveEndDateTime, datacontext.EmployeeLeaveStore[0].LeaveEndDateTime);
            Assert.AreEqual(_testEmployeeLeaveRequest.LeaveStartDateTime, datacontext.EmployeeLeaveStore[0].LeaveStartDateTime);
            Assert.AreEqual(_testEmployeeLeaveRequest.IsApproved, datacontext.EmployeeLeaveStore[0].IsApproved);
        }

        [Test]
        public void Client2_Process_Employee_Function_Doesnt_Save_When_Days_More_Than_20()
        {
            //Arrange

            var datacontext = MockRepository.GenerateStub<FakeEmployeeLeaveDataContext>();
            datacontext.EmployeeLeaveStore = new List<EmployeeLeaveRequest>();
            var employee = _testEmployee;
            employee.IsMarried = true;
            employee.ContactStartDate = DateTime.Today.AddDays(-90);
            datacontext.Employees = new List<Employee>() { employee
            var businessContext = new Client2EmployeeLeaveBusinessContext(_log, datacontext);

            var sut = MockRepository.GenerateMock<Client2EmployeeLeave>(_log, businessContext);

            var days = 25;

            //Act

            try
            {
                sut.ProcessLeaveRequest(_leaveStartDate, days, _reason, _employeeId);
                Assert.Fail("Should have failed but did not");
            }
            catch
            {
                //Assert
                Assert.AreEqual(0, datacontext.EmployeeLeaveStore.Count);
            }
        }

        [Test]
        public void Client2_Process_Employee_Function_Doesnt_Save_When_Contract_date_is_recent()
        {
            //Arrange

            var datacontext = MockRepository.GenerateStub<FakeEmployeeLeaveDataContext>();
            datacontext.EmployeeLeaveStore = new List<EmployeeLeaveRequest>();
            var employee = _testEmployee;
            employee.ContactStartDate = DateTime.Today;
            employee.IsMarried = false;
            datacontext.Employees = new List<Employee>() { employee };
            var businessContext = new Client2EmployeeLeaveBusinessContext(_log, datacontext);

            var sut = MockRepository.GenerateMock<Client2EmployeeLeave>(_log, businessContext);
            
            //Act

            try
            {
                sut.ProcessLeaveRequest(_leaveStartDate, _days, _reason, _employeeId);
                Assert.Fail("Should have failed but did not");
            }
            catch
            {
                //Assert
                Assert.AreEqual(0, datacontext.EmployeeLeaveStore.Count);
            }
        }
    }
}
