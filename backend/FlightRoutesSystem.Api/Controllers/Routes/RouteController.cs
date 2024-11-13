using FlightRoutesSystem.Application.Services.Routes;
using FlightRoutesSystem.Domain.Entities.Routes;
using FlightRoutesSystem.Domain.Entities.Routes.dto;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FlightRoutesSystem.Api.Controllers.Routes
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouteController : ControllerBase
    {
        private readonly RouteService _routeService;

        public RouteController(RouteService routeService)
        {
            _routeService = routeService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Route>> GetRoutes()
        {
            var routes = _routeService.GetAll();
            return Ok(routes);
        }

        [HttpGet("cheapest-route/{originId}/{destinyId}")]
        public ActionResult<IEnumerable<Route>> GetCheapestRoute(long originId, long destinyId)
        {
            var routes = _routeService.GetCheapestRoute(originId, destinyId);
            return Ok(routes);
        }

        [HttpGet("{id}")]
        public ActionResult<Route> GetRoute(long id)
        {
            var route = _routeService.GetById(id);
            if (route == null) return NotFound();
            return Ok(route);
        }

        [HttpPost]
        public ActionResult<Route> CreateRoute(RouteDTO route)
        {
            var createdRoute = _routeService.MapAndAdd(route);
            return Ok(new { id = createdRoute.Id });
        }

        [HttpPut("{id}")]
        public ActionResult UpdateRoute(long id, RouteUpdateDTO updatedRoute)
        {
            _routeService.MapAndUpdate(id, updatedRoute);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteRoute(long id)
        {
            var route = _routeService.GetById(id);
            if (route == null) return NotFound();

            _routeService.Remove(route);
            return NoContent();
        }
    }
}
