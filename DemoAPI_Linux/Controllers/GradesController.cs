using DemoAPI_Linux.Models;
using DemoAPI_Linux.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace DemoAPI_Linux.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GradesController : ControllerBase
    {
        private readonly SchoolContext _context;
        public GradesController(SchoolContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public IEnumerable<GradeResponseGet> GetAll()
        {            
            var grades = _context.Grades.Where(x => x.Active == 1).ToList();
            var response = grades.Select(g => new GradeResponseGet
            {
                GradeId = g.GradeId,
                Name = g.Name,
                Description = g.Description,
            });
            return response;
        }
    }
}
