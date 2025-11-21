using DemoAPI_Linux.Models;
using DemoAPI_Linux.Request;
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
        
        [HttpGet("{id}")]
        public ActionResult<GradeResponseGet> GetById(int id)
        {
            var grade = _context.Grades.FirstOrDefault(x => x.GradeId == id && x.Active == 1);
            if (grade == null)
            {
                return NotFound(new { message = "Grade not found" });
            }
            
            var response = new GradeResponseGet
            {
                GradeId = grade.GradeId,
                Name = grade.Name,
                Description = grade.Description,
            };
            
            return Ok(response);
        }
        
        [HttpPost]
        public ActionResult<GradeResponseGet> Insert([FromBody] GradeRequestInsert request)
        {
            var grade = new Grade
            {
                Name = request.Name,
                Description = request.Description,
                Active = 1
            };
            
            _context.Grades.Add(grade);
            _context.SaveChanges();
            
            var response = new GradeResponseGet
            {
                GradeId = grade.GradeId,
                Name = grade.Name,
                Description = grade.Description,
            };
            
            return CreatedAtAction(nameof(GetById), new { id = grade.GradeId }, response);
        }
        
        [HttpPut("{id}")]
        public ActionResult<GradeResponseGet> Update(int id, [FromBody] GradeRequestInsert request)
        {
            var grade = _context.Grades.FirstOrDefault(x => x.GradeId == id && x.Active == 1);
            if (grade == null)
            {
                return NotFound(new { message = "Grade not found" });
            }
            
            grade.Name = request.Name;
            grade.Description = request.Description;
            
            _context.SaveChanges();
            
            var response = new GradeResponseGet
            {
                GradeId = grade.GradeId,
                Name = grade.Name,
                Description = grade.Description,
            };
            
            return Ok(response);
        }
        
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var grade = _context.Grades.FirstOrDefault(x => x.GradeId == id && x.Active == 1);
            if (grade == null)
            {
                return NotFound(new { message = "Grade not found" });
            }
            
            // Soft delete: cambiar Active a 0
            grade.Active = 0;
            _context.SaveChanges();
            
            return Ok(new { message = "Grade deleted successfully" });
        }
    }
}
