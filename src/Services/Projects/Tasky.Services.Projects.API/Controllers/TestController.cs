using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tasky.BuildingBlocks.Constants;

namespace Tasky.Services.Projects.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles =Permissions.Roles.Administrators)] 
    
    public class TestController :ControllerBase
    {
        [Authorize(Policy=Permissions.Permission.FullAccess)] 
        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("test-api")]
        public IActionResult Get()
        {
            return Ok("Test endpoint is working!");
        }
    }
}