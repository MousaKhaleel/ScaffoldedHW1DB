using System;
using System.Collections.Generic;

namespace ScaffoldedHW1DB.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int? DepartmentId { get; set; }

    public DateOnly? HireDate { get; set; }

    public virtual Department? Department { get; set; }

    public virtual ICollection<Salary> Salaries { get; set; } = new List<Salary>();
}
