using DomL.DataAccess;
using System;

public class UnitOfWork : IDisposable
{
    private readonly DomLContext _context;

    public AutoRepository AutoRepo { get; private set; }
    public BookRepository BookRepo { get; private set; }
    public ComicRepository ComicRepo { get; private set; }

    public UnitOfWork(DomLContext context)
    {
        _context = context;

        AutoRepo = new AutoRepository(_context);
        BookRepo = new BookRepository(_context);
        ComicRepo = new ComicRepository(_context);
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
