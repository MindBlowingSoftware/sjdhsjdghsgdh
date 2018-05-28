using System;

namespace AnnualLeave
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime ContactStartDate { get; set; }
        public decimal? Salary { get; set; }
        public bool IsMarried { get; set; }
    }
}