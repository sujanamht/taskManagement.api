using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.api.Data;
using TaskManagement.api.Models;

namespace TaskManagement.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly TaskDbContext _context;

        public ProjectsController(TaskDbContext context)
        {
            _context = context;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectGetDto>>> GetProjects()
        {
            var projects = await _context.Projects
                .Select(p => new ProjectGetDto
                {
                    ProjectId = p.ProjectId,
                    ProjTitle = p.ProjTitle,
                    Description = p.Description,
                    UserId = p.UserId, //who created this
                    Status = p.Status,
                    CreatedAt = p.CreatedAt,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate
                })
                .ToListAsync();

            return Ok(projects);
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectGetDto>> GetProject(int id)
        {
            var project = await _context.Projects
                .Where(p => p.ProjectId == id)
                .Select(p => new ProjectGetDto
                {
                    ProjectId = p.ProjectId,
                    ProjTitle = p.ProjTitle,
                    Description = p.Description,
                    UserId = p.UserId,
                    UserName = p.User.FirstName + " " + p.User.LastName,  //EF Core translates that into a SQL JOIN automatically because User is a navigation property.
                    Status = p.Status,
                    CreatedAt = p.CreatedAt,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate
                })
                .FirstOrDefaultAsync();

            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }

        // POST: api/Projects
        [HttpPost]
        public async Task<ActionResult<ProjectGetDto>> PostProject(ProjectCreateDto dto)
        {
            if (dto.EndDate < dto.StartDate)
            {
                return BadRequest("EndDate cannot be earlier than StartDate.");
            }

            var userExists = await _context.Users.AnyAsync(u => u.UserId == dto.UserId);
            if (!userExists)
            {
                return BadRequest("Invalid UserId.");
            }

            //convert the DTO into the database entity.
            //this step prepares the object that will be saved to the database.
            var project = new Project
            {
                ProjTitle = dto.ProjTitle,
                Description = dto.Description,
                UserId = dto.UserId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,

            };

            _context.Projects.Add(project);         //Marks the project for insertion.
            await _context.SaveChangesAsync();      //save


            // convert the database entity back to a DTO.
            // Because you should not return database entities directly. 
            // DTO controls what fields the API exposes.

            var projectDto = new ProjectGetDto
            {
                ProjectId = project.ProjectId,
                ProjTitle = project.ProjTitle,
                Description = project.Description,
                UserId = project.UserId,
                Status = project.Status,
                CreatedAt = project.CreatedAt,
                StartDate = project.StartDate,
                EndDate = project.EndDate
            };

            return CreatedAtAction(nameof(GetProject), new { id = project.ProjectId }, projectDto); //Return the created resource
        }

        // PUT: api/Projects/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, ProjectUpdateDto dto)
        {
            if (dto.EndDate < dto.StartDate)
            {
                return BadRequest("EndDate cannot be earlier than StartDate.");
            }

            var userExists = await _context.Users.AnyAsync(u => u.UserId == dto.UserId);
            if (!userExists)
            {
                return BadRequest("Invalid UserId.");
            }

            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            //copying values from the DTO into the existing database entity.
            //DTO → existing entity
            project.ProjTitle = dto.ProjTitle;
            project.Description = dto.Description;
            project.UserId = dto.UserId;
            project.StartDate = dto.StartDate;
            project.EndDate = dto.EndDate;
            project.Status = dto.Status;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // Sort projects by a single selected field from the query string.
        [HttpGet("Sort")]
        public async Task<ActionResult<IEnumerable<ProjectGetDto>>> GetSortedProjects(string sortBy = "username", string order = "asc")
        {
            IQueryable<Project> query = _context.Projects;      //starts with the Projects table, but does not execute yet.

            switch (sortBy.ToLower())
            {
                case "username":
                    query = order.ToLower() == "desc"
                        ? query.OrderByDescending(p => p.User.FirstName).ThenByDescending(p => p.User.LastName)
                        : query.OrderBy(p => p.User.FirstName).ThenBy(p => p.User.LastName);
                    break;

                case "startdate":
                    query = order.ToLower() == "desc"
                        ? query.OrderByDescending(p => p.StartDate)
                        : query.OrderBy(p => p.StartDate);
                    break;

                case "status":
                    query = order.ToLower() == "desc"
                        ? query.OrderByDescending(p => p.Status)
                        : query.OrderBy(p => p.Status);
                    break;

                default:
                    return BadRequest("Invalid sort field. Use username, startdate, or status.");
            }

            var projects = await query
                .Select(p => new ProjectGetDto
                {
                    ProjectId = p.ProjectId,
                    ProjTitle = p.ProjTitle,
                    Description = p.Description,
                    UserId = p.UserId,
                    UserName = p.User.FirstName + " " + p.User.LastName,
                    Status = p.Status,
                    CreatedAt = p.CreatedAt,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate
                })
                .ToListAsync();

            return Ok(projects);
        }

    }
}