using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.api.Data;
using TaskManagement.api.Models;

namespace TaskManagement.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskItemsController : ControllerBase
    {
        private readonly TaskDbContext _context;

        public TaskItemsController(TaskDbContext context)
        {
            _context = context;
        }

        // GET: api/TaskItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItemGetDto>>> GetTaskItems()
        {
            
            var tasks = await _context.TaskItems
                .Select(t => new TaskItemGetDto
                {
                    TaskId = t.TaskItemId,
                    TaskTitle = t.TaskTitle,
                    Description = t.Description,
                    ProjectId = t.ProjectId,
                    UserId = t.UserId, //assigned 
                    UserName =t.User.FirstName + " " + t.User.LastName,
                    CreatedDate = t.CreatedDate,
                    DueDate = t.DueDate,
                    Status = t.Status,
                    Priority = t.Priority
                })
                .ToListAsync();

            return Ok(tasks);
        }

        // GET: api/TaskItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItemGetDto>> GetTaskItem(int id)
        {
            var taskItem = await _context.TaskItems
                .Where(t => t.TaskItemId == id)
                .Select(t => new TaskItemGetDto
                {
                    TaskId = t.TaskItemId,
                    TaskTitle = t.TaskTitle,
                    Description = t.Description,
                    ProjectId = t.ProjectId,
                    UserId = t.UserId,
                    UserName = t.User.FirstName + " " + t.User.LastName,
                    CreatedDate = t.CreatedDate,
                    DueDate = t.DueDate,
                    Status = t.Status,
                    Priority = t.Priority
                })
                .FirstOrDefaultAsync();

            if (taskItem == null)
            {
                return NotFound();
            }

            return Ok(taskItem);
        }

        // POST: api/TaskItems
        [HttpPost]
        public async Task<ActionResult<TaskItemGetDto>> PostTaskItem(TaskItemCreateDto dto)
        {
            var projectExists = await _context.Projects.AnyAsync(p => p.ProjectId == dto.ProjectId);
            if (!projectExists)
            {
                return BadRequest("Invalid ProjectId.");
            }

            var userExists = await _context.Users.AnyAsync(u => u.UserId == dto.UserId);
            if (!userExists)
            {
                return BadRequest("Invalid UserId.");
            }

            var taskItem = new TaskItem
            {
                TaskTitle = dto.TaskTitle,
                Description = dto.Description,
                ProjectId = dto.ProjectId,
                UserId = dto.UserId,
                DueDate = dto.DueDate,
                Status = dto.Status,
                Priority = dto.Priority
            };

            _context.TaskItems.Add(taskItem);
            await _context.SaveChangesAsync();

            var taskDto = new TaskItemGetDto
            {
                TaskId = taskItem.TaskItemId,
                TaskTitle = taskItem.TaskTitle,
                Description = taskItem.Description,
                ProjectId = taskItem.ProjectId,
                UserId = taskItem.UserId,
                CreatedDate = taskItem.CreatedDate,
                DueDate = taskItem.DueDate,
                Status = taskItem.Status,
                Priority = taskItem.Priority
            };

            return CreatedAtAction(nameof(GetTaskItem), new { id = taskItem.TaskItemId }, taskDto);
        }

        // PUT: api/TaskItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskItem(int id, TaskItemUpdateDto dto)
        {
            var projectExists = await _context.Projects.AnyAsync(p => p.ProjectId == dto.ProjectId);
            if (!projectExists)
            {
                return BadRequest("Invalid ProjectId.");
            }

            var userExists = await _context.Users.AnyAsync(u => u.UserId == dto.UserId);
            if (!userExists)
            {
                return BadRequest("Invalid UserId.");
            }

            var taskItem = await _context.TaskItems.FindAsync(id);

            if (taskItem == null)
            {
                return NotFound();
            }

            taskItem.TaskTitle = dto.TaskTitle;
            taskItem.Description = dto.Description;
            taskItem.ProjectId = dto.ProjectId;
            taskItem.UserId = dto.UserId;
            taskItem.DueDate = dto.DueDate;
            taskItem.Status = dto.Status;
            taskItem.Priority = dto.Priority;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/TaskItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskItem(int id)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);

            if (taskItem == null)
            {
                return NotFound();
            }

            _context.TaskItems.Remove(taskItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //// Sort tasks by a single selected field from the query string.
        // GET: api/TaskItems/Sort?sortBy=user
        [HttpGet("Sort")]
        public async Task<ActionResult<IEnumerable<TaskItemGetDto>>> GetSortedTaskItems(string sortBy = "user", string order ="asc")
        {
            IQueryable<TaskItem> query = _context.TaskItems;

            switch (sortBy.ToLower())
            {
                case "user":
                    query = order.ToLower() == "desc" 
                        ? query.OrderByDescending(p => p.User.FirstName).ThenByDescending(p => p.User.LastName)
                        :query.OrderBy(t => t.User.FirstName)
                                 .ThenBy(t => t.User.LastName);
                    break;

                case "createdate":
                    query = order.ToLower() == "desc"
                        ? query.OrderByDescending(p => p.CreatedDate)
                        : query.OrderBy(p => p.CreatedDate);
                    break;

                case "due":
                    query = order.ToLower() == "desc"
                        ? query.OrderByDescending(p => p.DueDate)
                        : query.OrderBy(t => t.DueDate);
                    break;

                case "status":
                    query = order.ToLower() == "desc"
                        ? query.OrderByDescending(p => p.Status)
                        : query.OrderBy(p => p.Status);
                    break;

                case "priority":
                    query = order.ToLower() == "desc"
                        ? query.OrderByDescending(p => p.Priority)
                        : query.OrderBy(p => p.Priority);
                    break;

                default:
                    return BadRequest("Invalid sort field. Use user, create, due, status, or priority.");
            }

            var tasks = await query
                .Select(t => new TaskItemGetDto
                {
                    TaskId = t.TaskItemId,
                    TaskTitle = t.TaskTitle,
                    Description = t.Description,
                    ProjectId = t.ProjectId,
                    UserId = t.UserId,
                    UserName = t.User.FirstName + " " + t.User.LastName,
                    ProjectTitle = t.Project.ProjTitle,
                    CreatedDate = t.CreatedDate,
                    DueDate = t.DueDate,
                    Status = t.Status,
                    Priority = t.Priority
                })
                .ToListAsync();

            return Ok(tasks);
        }
    }
}