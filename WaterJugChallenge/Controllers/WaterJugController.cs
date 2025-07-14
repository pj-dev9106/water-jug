using Microsoft.AspNetCore.Mvc;
using WaterJugChallenge.Models;
using WaterJugChallenge.Services;

namespace WaterJugChallenge.Controllers
{
    [Route("api/water-jug")]
    [ApiController]
    public class WaterJugController : ControllerBase
    {
        private readonly WaterJugService _service;

        public WaterJugController(WaterJugService service)
        {
            _service = service;
        }

        /// <summary>
        /// Solves the water jug problem using BFS algorithm
        /// </summary>
        /// <param name="request">The water jug problem parameters containing jug capacities and target amount</param>
        /// <returns>A sequence of steps to reach the target amount</returns>
        /// <response code="200">Returns the solution steps when a valid solution is found</response>
        /// <response code="400">If the input parameters are invalid or the problem is unsolvable</response>
        
        [HttpPost]
        [ProducesResponseType(typeof(WaterJugResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public IActionResult Solve([FromBody] WaterJugRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ErrorResponse { Error = "Invalid input: Values must be positive integers." });

            var (steps, error) = _service.Solution(request.XCapacity, request.YCapacity, request.TargetAmount);

            if (error != null)
                return BadRequest(new ErrorResponse { Error = error });

            return Ok(new WaterJugResponse { Steps = steps });
        }
    }
}