using DomL.DataAccess;

public class UnitOfWork
{
    private readonly DomLContext _context;
    public BookRepository BookRepo { get; private set; }

    public UnitOfWork(DomLContext context)
    {
        _context = context;
        BookRepo = new BookRepository(_context);
    }

    public int Complete()
    {
        return _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
