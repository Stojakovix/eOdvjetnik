using System;

namespace eOdvjetnik.Model
{
    public class EmployeeItem
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string? EmployeeHWID { get; set; }
        public string? Initals { get; set; }
        public int Active { get; set; }
        public int Type { get; set; }

    }
}