using Microsoft.AspNetCore.Mvc;
using Task4.models;

namespace Task4.Controllers
{
    [ApiController]
    [Route("api/animals")]
    public class AnimalsController : ControllerBase
    {
        //private static List<Animal> animals = new List<Animal>();

        private static List<Animal> animals = new List<Animal>
        {
            new Animal { Name = "Dog", Category = "Mammal", Weight = 20, FurColor = "Brown" },
            new Animal { Name = "Cat", Category = "Mammal", Weight = 10, FurColor = "White" },
            new Animal { Name = "Parrot", Category = "Bird", Weight = 1, FurColor = "Green" }
        };

        //Retrieve a list of animals
        [HttpGet]
        public IActionResult Get() => Ok(animals);

        //Retrieve an animal by its id
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var animal = animals.FirstOrDefault(a => a.Id == id);
            if (animal == null) return NotFound();
            return Ok(animal);
        }

        //Add animal
        [HttpPost]
        public IActionResult Post(Animal animal)
        {
            animals.Add(animal);
            return CreatedAtAction(nameof(Get), new { id = animal.Id }, animal);
        }

        //Edit animal
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, Animal updatedAnimal)
        {
            var index = animals.FindIndex(a => a.Id == id);
            if (index == -1) return NotFound();
            animals[index] = updatedAnimal;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var index = animals.FindIndex(a => a.Id == id);
            if (index == -1) return NotFound();
            animals.RemoveAt(index);
            return NoContent();
        }
    }
}