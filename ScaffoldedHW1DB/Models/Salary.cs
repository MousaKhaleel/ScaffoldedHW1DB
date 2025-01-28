using System;
using System.Collections.Generic;

namespace ScaffoldedHW1DB.Models;

public partial class Salary
{
    public int SalaryId { get; set; }

    public int? EmployeeId { get; set; }

    public decimal Salary1 { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public virtual Employee? Employee { get; set; }
}
