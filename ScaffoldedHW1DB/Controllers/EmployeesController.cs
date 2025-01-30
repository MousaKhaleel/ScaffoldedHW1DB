using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScaffoldedHW1DB.Models;

namespace ScaffoldedHW1DB.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class EmployeesController : ControllerBase
	{
		private readonly SkyHw1Context _context;

		public EmployeesController(SkyHw1Context context)
		{
			_context = context;
		}

		[HttpGet("GetEmployeesWithDepartmentNames")]
		public IActionResult GetEmployeesWithDepartments()
		{
			var result = _context.Employees
				.Include(x => x.Department)
				.Select(x => new
				{
					x.FirstName,
					x.LastName,
					DepartmentName = x.Department.DepartmentName,
					x.HireDate
				})
				.ToList();

			return Ok(result);
		}
		[HttpGet("GetDepartmentsWithEmployees")]
		public IActionResult GetDepartmentsWithEmployees()
		{
			var result = _context.Departments
				.Select(x => new
				{
					DepartmentName = x.DepartmentName,
					Employees = x.Employees.Select(y => new
					{
						y.FirstName,
						y.LastName,
						y.HireDate
					}).ToList()
				})
				.ToList();

			return Ok(result);
		}

		[HttpGet("GetEmployeeCountBydepartments")]
		public IActionResult GetDepartmentsWithEmployeeCount()
		{
			var result = _context.Departments
				.Where(x => x.Employees.Count > 1)
				.Select(x => new
				{
					DepartmentName = x.DepartmentName,
					EmployeeCount = x.Employees.Count
				})
				.ToList();

			return Ok(result);
		}
		[HttpGet("GetDepartmentsWithHighSalary")]
		public IActionResult GetDepartmentsWithHighEarners()
		{
			var result = _context.Departments
				.Select(x => new
				{
					DepartmentName = x.DepartmentName,
					HighEarnerCount = x.Employees
						.Join(_context.Salaries,
							  y => y.DepartmentId,
							  z => z.EmployeeId,
							  (y, z) => new { Employee = y, Salary = z })
						.Count(c => c.Salary.Salary1 > 5000)
				})
				.Where(x => x.HighEarnerCount > 1)
				.ToList();

			return Ok(result);
		}
	}
}
