using Microsoft.AspNetCore.Mvc;
using FDWS.Services;
using System.Threading.Tasks;

namespace FDWS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly IBusinessLogic _businessLogic;

        public ApiController(IBusinessLogic businessLogic)
        {
            _businessLogic = businessLogic;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _businessLogic.GetByIdAsync(id);
            if (result == null)
                return NotFound();
            
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] object data)
        {
            var result = await _businessLogic.CreateAsync(data);
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }
    }
}