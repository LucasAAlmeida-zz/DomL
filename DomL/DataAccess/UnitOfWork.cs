using DomL.DataAccess;
using DomL.DataAccess.Repositories;
using System;

public class UnitOfWork : IDisposable
{
    private readonly DomLContext _context;

    public ActivityRepository ActivityRepo { get; private set; }

    public AutoRepository AutoRepo { get; private set; }
    public BookRepository BookRepo { get; private set; }
    public ComicRepository ComicRepo { get; private set; }
    public DoomRepository DoomRepo { get; private set; }
    public EventRepository EventRepo { get; private set; }

    public SeriesRepository SeriesRepo { get; private set; }
    public PersonRepository PersonRepo { get; private set; }
    public MediaTypeRepository MediaTypeRepo { get; internal set; }

    public UnitOfWork(DomLContext context)
    {
        _context = context;

        ActivityRepo = new ActivityRepository(_context);
        
        AutoRepo = new AutoRepository(_context);
        BookRepo = new BookRepository(_context);
        ComicRepo = new ComicRepository(_context);
        DoomRepo = new DoomRepository(_context);
        EventRepo = new EventRepository(_context);

        SeriesRepo = new SeriesRepository(_context);
        PersonRepo = new PersonRepository(_context);
        MediaTypeRepo = new MediaTypeRepository(_context);
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
