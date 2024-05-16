using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalksAPI.Controllers

{
    // https://localhost: portnumber/api/students
    // [controller] in this format we can represent controller name in the api
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        // It has name Action Methon 
        // it is get action method
        // [HttpDelete] , [HttpGet] ,[HttpPost] and [HttpPut] these are called as action method 
        // GET: https://localhost:portnumber/api/students
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            string[] studentNames = new string[] { "John", "Jane", "Mark", "Emily", "David" };
            return Ok(studentNames);
        }
    }
}
