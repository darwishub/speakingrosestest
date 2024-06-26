using speakingrosestest.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using speakingrosestest.Data;

namespace speakingrosestest.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private MyDbContext _context;
        public TaskRepository(MyDbContext context)
        {
            this._context = context;
        }
        public async Task<IEnumerable<_Task>> GetTasks()
        {
            return await _context.Tasks.ToListAsync();
        }

        public async Task<_Task> GetTaskById(int id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task InsertTask(_Task task)
        {
            await _context.Tasks.AddAsync(task);
        }

        public void UpdateTask(_Task task)
        {
            _context.Entry(task).State = EntityState.Modified;
        }

        public async Task DeleteTask(int id)
        {
            _Task task = await _context.Tasks.FindAsync(id);
            _context.Tasks.Remove(task);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}