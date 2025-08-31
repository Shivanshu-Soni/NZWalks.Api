### repository pattern

# it is a design patern which seperates the data access layer from the application

# provides interface without exposing elements

# helps create abstraction (hidden implementation , dont know which db is using)

    ## Example: Repository Pattern in C#

    ```csharp
    // IRepository.cs
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        void Remove(int id);
    }

    // UserRepository.cs
    public class UserRepository : IRepository<User>
    {
        private readonly DbContext _context;

        public UserRepository(DbContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetAll() => _context.Users.ToList();

        public User GetById(int id) => _context.Users.Find(id);

        public void Add(User entity) => _context.Users.Add(entity);

        public void Remove(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
                _context.Users.Remove(user);
        }
    }
    ```

    **Benefits:**
    - Decouples business logic from data access logic.
    - Makes unit testing easier by allowing mocking of repositories.
    - Supports multiple data sources (SQL, NoSQL, etc.) with minimal changes.
