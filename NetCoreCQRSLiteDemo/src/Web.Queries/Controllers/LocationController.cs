using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Domain.ReadModel.Repositories;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Queries.Controllers
{
    [Route("location")]
    public class LocationController : Controller
    {
        private ILocationRepository _locationRepo;

        public LocationController(ILocationRepository locationRepo)
        {
            _locationRepo = locationRepo;
        }
        // TODO sostiutire Ok con ObjectResult
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetByID(int id)
        {
            var location = _locationRepo.GetByID(id);
            if (location == null)
            {
                return BadRequest("No location with ID " + id.ToString() + " was found.");
            }
            return Ok(location);
        }

        [HttpGet]
        [Route("all")]
        public IActionResult GetAll()
        {
            var locations = _locationRepo.GetAll();
            return Ok(locations);
        }

        [HttpGet]
        [Route("{id}/employees")]
        public IActionResult GetEmployees(int id)
        {
            var employees = _locationRepo.GetEmployees(id);
            return Ok(employees);
        }
    }
}
