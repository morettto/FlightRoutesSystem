using FlightRoutesSystem.Application.Services.Airports;
using FlightRoutesSystem.Domain.Entities.Airports;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FlightRoutesSystem.Api.Controllers.Airports
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportController : ControllerBase
    {
        private readonly AirportService _airportService;

        public AirportController(AirportService airportService)
        {
            _airportService = airportService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Airport>> GetAirports()
        {
            var airports = _airportService.GetAll();
            return Ok(airports);
        }

        [HttpGet("{id}")]
        public ActionResult<Airport> GetAirport(long id)
        {
            var airport = _airportService.GetById(id);
            if (airport == null) return NotFound();
            return Ok(airport);
        }

        [HttpPost]
        public ActionResult<Airport> CreateAirport(Airport airport)
        {
            var createdAirport = _airportService.Add(airport);
            return CreatedAtAction(nameof(GetAirport), new { id = createdAirport.Id }, createdAirport);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateAirport(Airport updatedAirport)
        {
            _airportService.Update(updatedAirport);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteAirport(long id)
        {
            var airport = _airportService.GetById(id);
            if (airport == null) return NotFound();

            _airportService.Remove(airport);
            return NoContent();
        }
    }
}
