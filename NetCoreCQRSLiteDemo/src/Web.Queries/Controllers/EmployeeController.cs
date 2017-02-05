using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Domain.ReadModel.Repositories;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Queries.Controllers
{
    //[Route("api/[controller]")]
    [Route("employees")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepo;

        public EmployeeController(IEmployeeRepository employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        //[HttpGet]
        //[Route("{id}")]
        [HttpGet("{id}")]

        public IActionResult GetByID(int id)
        {
            var employee = _employeeRepo.GetByID(id);

            //It is possible for GetByID() to return null.
            //If it does, we return HTTP 400 Bad Request
            if (employee == null)
            {
                return BadRequest("No Employee with ID " + id.ToString() + " was found.");
            }

            //Otherwise, we return the employee
            return new ObjectResult(employee);
        }

        [HttpGet]
        [Route("all")]
        public IActionResult GetAll()
        {
            var employees = _employeeRepo.GetAll();
            return new ObjectResult(employees);
        }
    }
}
