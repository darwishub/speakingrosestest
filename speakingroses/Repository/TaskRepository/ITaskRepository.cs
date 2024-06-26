using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using speakingrosestest.Models;

namespace speakingrosestest.Repository
{
    public interface ITaskRepository : IDisposable
    {
        Task<IEnumerable<_Task>> GetTasks();
        Task<_Task> GetTaskById(int id);
        Task InsertTask(_Task task);
        void UpdateTask(_Task task);
        Task DeleteTask(int id);
        Task Save();
    }
}