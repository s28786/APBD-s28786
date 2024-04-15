using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using Task4.models;

namespace Task4.Controllers
{
    [ApiController]
    [Route("api/visits")]
    public class VisitsController : ControllerBase
    {
        // List of visits
        //private static List<Visit> visits = new List<Visit>();
        private static List<Visit> visits = new List<Visit>
        {
            new Visit { DateOfVisit = new DateTime(2021, 10, 10), AnimalId = Guid.NewGuid(), Description = "Dog visit", Price = 100 },
            new Visit { DateOfVisit = new DateTime(2021, 10, 11), AnimalId = Guid.NewGuid(), Description = "Cat visit", Price = 50 },
            new Visit { DateOfVisit = new DateTime(2021, 10, 12), AnimalId = Guid.NewGuid(), Description = "Parrot visit", Price = 10 }
        };

        //Retrieve all visits
        [HttpGet]
        public IActionResult Get() => Ok(visits);

        //Retrieve visists with specific animal by the Id
        [HttpGet("{animalId}")]
        public IActionResult Get(Guid animalId)
        {
            var visit = visits.FirstOrDefault(v => v.AnimalId == animalId);
            if (visit == null) return NotFound();
            return Ok(visit);
        }

        //Add a new visit
        [HttpPost]
        public IActionResult Post(Visit visit)
        {
            visits.Add(visit);
            return CreatedAtAction(nameof(Get), new { id = visit.Id }, visit);
        }
    }
}