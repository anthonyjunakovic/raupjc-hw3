using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TodoApp.Models;
using TodoRepository;
using TodoRepository.Interfaces;
using Microsoft.AspNetCore.Identity;
using TodoApp.Data;
using Microsoft.Extensions.Logging;

namespace TodoApp.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class TodoController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITodoRepository _repository;
        private readonly ILogger<TodoController> _errorLogs;

        public TodoController(UserManager<ApplicationUser> userManager, ITodoRepository repository, ILogger<TodoController> errorLogs)
        {
            _userManager = userManager;
            _repository = repository;
            _errorLogs = errorLogs;
        }

        [Route("")]
        [Route("[action]")]
        public IActionResult Index()
        {
            Guid userId = new Guid(_userManager.GetUserId(User));
            IndexViewModel models = new IndexViewModel();
            List<TodoItem> items = _repository.GetActive(userId).OrderByDescending(i => i.DateCreated).ToList();
            foreach (TodoItem item in items.Where(i => i.DateDue.HasValue))
            {
                TodoViewModel model = new TodoViewModel(item.Id, item.Text, item.DateDue, item.IsCompleted);
                model.Labels = TodoViewModel.GetLabelsRaw(item.Labels);
                models.Add(model);
            }
            foreach (TodoItem item in items.Where(i => !i.DateDue.HasValue))
            {
                TodoViewModel model = new TodoViewModel(item.Id, item.Text, item.DateDue, item.IsCompleted);
                model.Labels = TodoViewModel.GetLabelsRaw(item.Labels);
                models.Add(model);
            }
            return View(models);
        }

        [Route("[action]")]
        public IActionResult Completed()
        {
            Guid userId = new Guid(_userManager.GetUserId(User));
            CompletedViewModel models = new CompletedViewModel();
            List<TodoItem> items = _repository.GetCompleted(userId).OrderByDescending(i => i.DateCompleted).ToList();
            foreach (TodoItem item in items)
            {
                TodoViewModel model = new TodoViewModel(item.Id, item.Text, item.DateCompleted, item.IsCompleted);
                model.Labels = TodoViewModel.GetLabelsRaw(item.Labels);
                models.Add(model);
            }
            return View(models);
        }

        [Route("[action]")]
        public IActionResult Todo()
        {
            return NotFound();
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Mark(Guid? id)
        {
            if (id.HasValue)
            {
                try
                {
                    Guid userId = new Guid(_userManager.GetUserId(User));
                    _repository.MarkAsCompleted(id.Value, userId);
                    return RedirectToAction("Completed");
                }
                catch (Exception ex)
                {
                    _errorLogs.LogWarning(ex, ex.Message, new object[] { });
                    return Unauthorized();
                }
            }
            return NotFound();
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Remove(Guid? id)
        {
            if (id.HasValue)
            {
                try
                {
                    Guid userId = new Guid(_userManager.GetUserId(User));
                    _repository.Remove(id.Value, userId);
                    return RedirectToAction("Completed");
                }
                catch (Exception ex)
                {
                    _errorLogs.LogWarning(ex, ex.Message, new object[] { });
                    return Unauthorized();
                }
            }
            return NotFound();
        }

        [Route("[action]")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Add(AddViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Text == null)
                {
                    model.Text = "";
                }
                if (model.Labels == null)
                {
                    model.Labels = "";
                }
                if ((!model.Time.HasValue) || ((DateTime.UtcNow - model.Time.Value).TotalSeconds > 0))
                {
                    model.Time = null;
                }
                Guid userId = new Guid(_userManager.GetUserId(User));
                TodoItem item = new TodoItem(model.Text, userId);
                item.DateDue = model.Time;
                foreach (string label in model.Labels.Trim().Split(','))
                {
                    if (label != "")
                    {
                        item.Labels.Add(_repository.GenerateLabel(label));
                    }
                }
                _repository.Add(item);
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}