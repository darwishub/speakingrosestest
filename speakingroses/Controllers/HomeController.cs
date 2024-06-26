using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using speakingrosestest.Models;
using speakingrosestest.Repository;
using speakingrosestest.Data;
using Microsoft.EntityFrameworkCore;

namespace speakingrosestest.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRepositoryWrapper _repository;
    private readonly MyDbContext _context;

    public HomeController(ILogger<HomeController> logger, IRepositoryWrapper repository, MyDbContext context)
    {
        _logger = logger;
        _repository = repository;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetTasks(DateTime? dueDate = null, byte? priority = null, bool? status = null)
    {
        try
        {
            var tasks = await _repository.Task.GetTasks();
            var tasksQuery = tasks.AsQueryable();

            if (dueDate.HasValue)
            {
                tasksQuery = tasksQuery.Where(task => task.DueDate.Date == dueDate.Value.Date);
            }

            if (priority.HasValue)
            {
                tasksQuery = tasksQuery.Where(task => task.Priority == priority.Value);
            }

            if (status.HasValue)
            {
                tasksQuery = tasksQuery.Where(task => task.Status == status.Value);
            }

            var filteredTasks = tasksQuery.OrderBy(task => task.Status).ThenBy(task => task.DueDate).ToList();
            return Json(filteredTasks);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetTaskById(int id)
    {
        try
        {
            var task = await _repository.Task.GetTaskById(id);
            if (task == null)
            {
                return StatusCode(500, $"Task with ID {id} not found.");
            }
            return Json(task);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> InsertTask([FromBody] _Task task)
    {
        try
        {
            if (task == null)
            {
                return StatusCode(500, "Task is null.");
            }

            await _repository.Task.InsertTask(task);
            await _repository.Task.Save();
            return Json(task);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPatch]
    public async Task<IActionResult> CompleteTask(int id)
    {
        try
        {
            var task = await _repository.Task.GetTaskById(id);
            if (task == null)
            {
                return StatusCode(500, $"Task with ID {id} not found.");
            }

            task.Status = true;
            _repository.Task.UpdateTask(task);
            await _repository.Task.Save();
            return Json(task);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPatch]
    public async Task<IActionResult> IncompleteTask(int id)
    {
        try
        {
            var task = await _repository.Task.GetTaskById(id);
            if (task == null)
            {
                return StatusCode(500, $"Task with ID {id} not found.");
            }

            task.Status = false;
            _repository.Task.UpdateTask(task);
            await _repository.Task.Save();
            return Json(task);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteTask(int id)
    {
        try
        {
            var task = await _repository.Task.GetTaskById(id);
            if (task == null)
            {
                return StatusCode(500, $"Task with ID {id} not found.");
            }

            await _repository.Task.DeleteTask(id);
            await _repository.Task.Save();
            return Json(task);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTask([FromBody] _Task task)
    {
        try
        {
            if (task == null)
            {
                return StatusCode(500, "Task is null.");
            }

            var getTask = await _repository.Task.GetTaskById(task.TaskId);
            if (getTask == null)
            {
                return StatusCode(500, $"Task with ID {task.TaskId} not found.");
            }

            getTask.Title = task.Title;
            getTask.Description = task.Description;
            getTask.DueDate = task.DueDate;
            getTask.Priority = task.Priority;

            _repository.Task.UpdateTask(getTask);
            await _repository.Task.Save();
            return Json(task);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
