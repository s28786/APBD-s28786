using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task8_New.Context;
using Task8_New.Models;

namespace Task8_New.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly maksousDbContext _context;

        public TripsController(maksousDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (_context.Trips == null)
            {
                return NotFound();
            }

            var trips = await _context.Trips
            .OrderByDescending(t => t.DateFrom)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TripDTO
            {
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom,
                DateTo = t.DateTo,
                MaxPeople = t.MaxPeople,

                Countries = t.IdCountries.Select(ct => new CountryDTO
                {
                    Name = ct.Name
                }).ToList(),

                Clients = t.ClientTrips.Select(ct => new ClientDTO
                {
                    FirstName = ct.IdClientNavigation.FirstName,
                    LastName = ct.IdClientNavigation.LastName
                }).ToList()
            }).ToListAsync();

            var totalTrips = await _context.Trips.CountAsync();
            var totalPages = (int)Math.Ceiling(totalTrips / (double)pageSize);

            return Ok(new
            {
                pageNum = page,
                pageSize,
                allPages = totalPages,
                trips
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTrip(int id)
        {
            var trip = await _context.Trips.FindAsync(id);
            if (trip == null)
            {
                return NotFound();
            }

            if (trip.ClientTrips.Count > 0)
            {
                return BadRequest("The trip has assigned clients. Remove them first.");
            }

            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /*Prepare an endpoint that allows assigning a client to a trip
        1. Requests should be accepted at the HTTP POST address
        /api/trips/{idTrip}/clients
        2. The server should, during the request processing:
        1. Check if a client with the given PESEL number already
        exists - if not so, return an error.
        2. Check if a client with the given PESEL number is
        already registered for the given trip - if so, return an
        error.
        3. Check if the given trip exists and if DateFrom is in the
        future. We cannot register for a trip that has already
        occurred.
        4. PaymentDate can be NULL for clients who have not
        yet paid for the trip. Additionally, RegisteredAt in the
        Client_Trip table should match the time the request
        was received by the server.
        */

        [HttpPost("{idTrip}/clients")]
        public async Task<ActionResult> AddClientToTrip(ClientTripRequestTDO clientTripRequestTDO, int idTrip)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Pesel == clientTripRequestTDO.Pesel);
            if (client == null)
            {
                return BadRequest("Client with the given PESEL number does not exist.");
            }

            var trip_client = await _context.ClientTrips.FirstOrDefaultAsync(ct => ct.IdClient == client.IdClient && ct.IdTrip == idTrip);
            if (trip_client != null)
            {
                return BadRequest("The client is already registered for the trip.");
            }
            var trip = await _context.Trips.FirstOrDefaultAsync(t => t.IdTrip == idTrip);
            if (trip == null)
            {
                return BadRequest("Trip with the given ID does not exist.");
            }

            if (trip.DateFrom < DateTime.Now)
            {
                return BadRequest("The trip has already occurred.");
            }

            var clientTrip = await _context.ClientTrips.FirstOrDefaultAsync(ct => ct.IdClient == client.IdClient && ct.IdTrip == trip.IdTrip);
            if (clientTrip != null)
            {
                return BadRequest("The client is already registered for the trip.");
            }

            var newClientTrip = new ClientTrip
            {
                IdClient = client.IdClient,
                IdTrip = trip.IdTrip,
                RegisteredAt = DateTime.Now,
                PaymentDate = clientTripRequestTDO.PaymentDate
            };

            _context.ClientTrips.Add(newClientTrip);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}