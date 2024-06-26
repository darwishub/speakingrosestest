using speakingrosestest.Data;
using speakingrosestest.Models;

namespace speakingrosestest.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly MyDbContext _context;
        private ITaskRepository? _task;
       
        public ITaskRepository Task
        {
            get
            {
                if (_task == null)
                {
                    _task = new TaskRepository(_context);
                }
                return _task;
            }
        }

        public RepositoryWrapper(MyDbContext context)
        {
            _context = context;
        }
    }
}