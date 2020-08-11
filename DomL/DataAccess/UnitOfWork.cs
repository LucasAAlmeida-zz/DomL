using DomL.DataAccess;
using System;

public class UnitOfWork : IDisposable
{
    private readonly DomLContext _context;
    public BookRepository BookRepo { get; private set; }
    public ComicRepository ComicRepo { get; private set; }
    public GameRepository GameRepo { get; private set; }
    public SeriesRepository SeriesRepo { get; private set; }
    public WatchRepository WatchRepo { get; private set; }
    public AutoRepository AutoRepo { get; private set; }
    public DoomRepository DoomRepo { get; private set; }
    public GiftRepository GiftRepo { get; private set; }
    public HealthRepository HealthRepo { get; private set; }
    public MovieRepository MovieRepo { get; private set; }
    public PersonRepository PersonRepo { get; private set; }
    public PetRepository PetRepo { get; private set; }
    public PlayRepository PlayRepo { get; private set; }
    public PurchaseRepository PurchaseRepo { get; private set; }
    public TravelRepository TravelRepo { get; private set; }
    public WorkRepository WorkRepo { get; private set; }
    public EventRepository EventRepo { get; private set; }
    public ActivityBlockRepository ActivityBlockRepo { get; private set; }

    public UnitOfWork(DomLContext context)
    {
        _context = context;
        BookRepo = new BookRepository(_context);
        ComicRepo = new ComicRepository(_context);
        GameRepo = new GameRepository(_context);
        SeriesRepo = new SeriesRepository(_context);
        WatchRepo = new WatchRepository(_context);
        AutoRepo = new AutoRepository(_context);
        DoomRepo = new DoomRepository(_context);
        GiftRepo = new GiftRepository(_context);
        HealthRepo = new HealthRepository(_context);
        MovieRepo = new MovieRepository(_context);
        PersonRepo = new PersonRepository(_context);
        PetRepo = new PetRepository(_context);
        PlayRepo = new PlayRepository(_context);
        PurchaseRepo = new PurchaseRepository(_context);
        TravelRepo = new TravelRepository(_context);
        WorkRepo = new WorkRepository(_context);
        EventRepo = new EventRepository(_context);
        ActivityBlockRepo = new ActivityBlockRepository(_context);
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
