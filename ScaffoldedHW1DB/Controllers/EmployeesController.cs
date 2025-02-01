using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScaffoldedHW1DB.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace ScaffoldedHW1DB.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class EmployeesController : ControllerBase
	{
		private readonly SkyHw1Context _context;
		public EmployeesController(SkyHw1Context context)
		{
			_context = context;
		}

		//		Write LINQ query to list all employees along with their department name.Include
		//the employee&#39;s first and last name, department name, and hire date.

		[HttpGet("GetEmployeesWithDepartmentNames")]
		public async Task<IActionResult> GetEmployeesWithDepartments()
		{
			var result = await _context.Employees
				.Include(x => x.Department)
				.Select(x => new
				{
					x.FirstName,
					x.LastName,
					DepartmentName = x.Department.DepartmentName,
					x.HireDate
				})
				.ToListAsync();

			return Ok(result);
		}

		//2. Write LINQ query to list all departments along with their employees.If a
		//department has no employees, still list the department but display null for employee
		//		details.

		[HttpGet("GetDepartmentsWithEmployees")]
		public async Task<IActionResult> GetDepartmentsWithEmployees()
		{
			var result = await _context.Departments
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
				.ToListAsync();

			return Ok(result);
		}

		//3. Write LINQ query to find the total number of employees in each department and
		//display only departments with more than 1 employee.

		[HttpGet("GetEmployeeCountBydepartments")]
		public async Task<IActionResult> GetDepartmentsWithEmployeeCount()
		{
			var result = await _context.Departments
				.Where(x => x.Employees.Count > 1)
				.Select(x => new
				{
					DepartmentName = x.DepartmentName,
					EmployeeCount = x.Employees.Count
				})
				.ToListAsync();

			return Ok(result);
		}

		//4. Write LINQ query to find the count of employees who have a salary greater than
		//$5000. Group the result by department and display only departments where the count
		//of such employees is greater than 1

		[HttpGet("GetDepartmentsWithHighSalary")]
		public async Task<IActionResult> GetDepartmentsWithHighEarners()
		{
			var result = await _context.Departments
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
				.ToListAsync();

			return Ok(result);
		}
	}
}
