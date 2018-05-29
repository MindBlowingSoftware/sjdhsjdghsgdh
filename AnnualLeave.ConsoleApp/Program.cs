using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnnualLeave.Client1;
using AnnualLeave.Shared.Interface;
using AnnualLeave.Shared.Model;
using log4net;
using Newtonsoft.Json;
using StructureMap;

namespace AnnualLeave.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new Container(x =>
                {
                    x.For<ILog>().Use<CustomLog>();
                    x.For<IEmployeeLeaveDataContext>().Use<Client1EmployeeLeaveDataContext>();
                    x.For<IEmployeeLeaveBusinessContext>().Use<Client1EmployeeLeaveBusinessContext>();
                    x.For<IEmployeeLeave>().Use<FakeClient1EmployeeLeave>();
                }
            );

            var employee = container.GetInstance<IEmployeeLeave>().FindEmployee(new Random().Next(100));
            Console.WriteLine(JsonConvert.SerializeObject(employee));
            Console.ReadLine();
        }
    }

    internal class FakeClient1EmployeeLeave : IEmployeeLeave
    {
        public void ProcessLeaveRequest(DateTime leaveStartDate, int days, string reason, int employeeId)
        {
            throw new NotImplementedException();
        }

        public Employee FindEmployee(int employeeId)
        {
            return new Employee()
            {
                EmployeeId = employeeId,
                IsMarried = true,
                LastName = "LastName",
                ContactStartDate = DateTime.Today.AddDays(-100),
                Salary = 200000,
                Name = "Name"
            };
        }
    }
}
