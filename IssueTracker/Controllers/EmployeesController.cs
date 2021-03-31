using AutoMapper;
using AutoMapper.QueryableExtensions;
using IssueTracker.Domain;
using IssueTracker.Models.Employees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueTracker.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IssueTrackerDataContext _context;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _config;

        public EmployeesController(IssueTrackerDataContext context, IMapper mapper, MapperConfiguration config)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
        }



        // GET /employees  (or /employees/index)
        public async Task<IActionResult> Index()
        {
            var response = new GetEmployeesResponse();

            var data = await _context.Employees
                .OrderBy(e => e.LastName)
                .ProjectTo<GetEmployeeSummaryItem>(_config)
                .ToListAsync();

            response.NumberOfFullTimeEmployees = data.Count();
            response.Employees = data;
            return View(response);
        }

        public IActionResult New()
        {
            var model = new PostEmployeeRequest();
            model.StartingSalary = 35000;
            return View(model);
        }

        public async Task<IActionResult> CreateAsync(PostEmployeeRequest request)
        {
            if(!ModelState.IsValid)
            {
                return View("New", request);
            } else
            {
                // uh, we should save it to the database...
                var employee = new Employee
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = $"{request.FirstName.ToLower()}@aol.com",
                    Salary = request.StartingSalary
                };

                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
        }
    }
}
