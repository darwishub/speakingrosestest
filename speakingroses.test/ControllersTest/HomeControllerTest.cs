using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using speakingrosestest.Controllers;
using speakingrosestest.Models;
using speakingrosestest.Repository;
using speakingrosestest.Data;
using Xunit;

namespace speakingrosestest.ControllersTest
{
    public class HomeControllerTests
    {
        private readonly Mock<ILogger<HomeController>> _mockLogger;
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<MyDbContext> _mockContext;
        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            _mockLogger = new Mock<ILogger<HomeController>>();
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockContext = new Mock<MyDbContext>();
            _controller = new HomeController(_mockLogger.Object, _mockRepository.Object, _mockContext.Object);
        }

        [Fact]
        public async Task GetTasks_ReturnsJsonResult_WithFilteredTasks()
        {
            // Arrange
            var tasks = new List<_Task>
            {
                new _Task { TaskId = 1, DueDate = DateTime.Now, Priority = 1, Status = false },
                new _Task { TaskId = 2, DueDate = DateTime.Now.AddDays(1), Priority = 2, Status = true }
            }.AsQueryable();

            _mockRepository.Setup(repo => repo.Task.GetTasks()).ReturnsAsync(tasks);

            // Act
            var result = await _controller.GetTasks();

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var returnedTasks = Assert.IsAssignableFrom<IEnumerable<_Task>>(jsonResult.Value);
            Assert.Equal(2, returnedTasks.Count());
        }

        [Fact]
        public async Task GetTaskById_ReturnsJsonResult_WithTask()
        {
            // Arrange
            var task = new _Task { TaskId = 1, Title = "Test Task" };
            _mockRepository.Setup(repo => repo.Task.GetTaskById(1)).ReturnsAsync(task);

            // Act
            var result = await _controller.GetTaskById(1);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var returnedTask = Assert.IsType<_Task>(jsonResult.Value);
            Assert.Equal("Test Task", returnedTask.Title);
        }

        [Fact]
        public async Task InsertTask_ReturnsJsonResult_WithInsertedTask()
        {
            // Arrange
            var task = new _Task { TaskId = 1, Title = "New Task" };
            _mockRepository.Setup(repo => repo.Task.InsertTask(task)).Returns(Task.CompletedTask);
            _mockRepository.Setup(repo => repo.Task.Save()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.InsertTask(task);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var returnedTask = Assert.IsType<_Task>(jsonResult.Value);
            Assert.Equal("New Task", returnedTask.Title);
        }

        [Fact]
        public async Task CompleteTask_ReturnsJsonResult_WithCompletedTask()
        {
            // Arrange
            var taskId = 1;
            var task = new _Task { TaskId = taskId, Status = false };
            _mockRepository.Setup(repo => repo.Task.GetTaskById(taskId)).ReturnsAsync(task);
            _mockRepository.Setup(repo => repo.Task.UpdateTask(task));
            _mockRepository.Setup(repo => repo.Task.Save()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CompleteTask(taskId);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var returnedTask = Assert.IsType<_Task>(jsonResult.Value);
            Assert.True(returnedTask.Status);
        }

        [Fact]
        public async Task IncompleteTask_ReturnsJsonResult_WithIncompleteTask()
        {
            // Arrange
            var taskId = 1;
            var task = new _Task { TaskId = taskId, Status = true };
            _mockRepository.Setup(repo => repo.Task.GetTaskById(taskId)).ReturnsAsync(task);
            _mockRepository.Setup(repo => repo.Task.UpdateTask(task));
            _mockRepository.Setup(repo => repo.Task.Save()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.IncompleteTask(taskId);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var returnedTask = Assert.IsType<_Task>(jsonResult.Value);
            Assert.False(returnedTask.Status);
        }

        [Fact]
        public async Task DeleteTask_ReturnsJsonResult_WithDeletedTask()
        {
            // Arrange
            var taskId = 1;
            var task = new _Task { TaskId = taskId };
            _mockRepository.Setup(repo => repo.Task.GetTaskById(taskId)).ReturnsAsync(task);
            _mockRepository.Setup(repo => repo.Task.DeleteTask(taskId)).Returns(Task.CompletedTask);
            _mockRepository.Setup(repo => repo.Task.Save()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteTask(taskId);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var returnedTask = Assert.IsType<_Task>(jsonResult.Value);
            Assert.Equal(taskId, returnedTask.TaskId);
        }

        [Fact]
        public async Task UpdateTask_ReturnsJsonResult_WithUpdatedTask()
        {
            // Arrange
            var task = new _Task { TaskId = 1, Title = "Old Title" };
            var updatedTask = new _Task { TaskId = 1, Title = "New Title", Description = "New Description", DueDate = DateTime.Now, Priority = 1 };

            _mockRepository.Setup(repo => repo.Task.GetTaskById(task.TaskId)).ReturnsAsync(task);
            _mockRepository.Setup(repo => repo.Task.UpdateTask(task));
            _mockRepository.Setup(repo => repo.Task.Save()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateTask(updatedTask);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var returnedTask = Assert.IsType<_Task>(jsonResult.Value);
            Assert.Equal("New Title", returnedTask.Title);
        }

    }
}