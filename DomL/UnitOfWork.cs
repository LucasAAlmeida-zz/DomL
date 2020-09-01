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
    public CourseRepository CourseRepo { get; private set; }
    public DoomRepository DoomRepo { get; private set; }
    public EventRepository EventRepo { get; private set; }
    public GameRepository GameRepo { get; private set; }
    public GiftRepository GiftRepo { get; private set; }
    public HealthRepository HealthRepo { get; private set; }
    public MovieRepository MovieRepo { get; private set; }
    public PetRepository PetRepo { get; private set; }
    public MeetRepository MeetRepo { get; private set; }
    public PlayRepository PlayRepo { get; private set; }
    public PurchaseRepository PurchaseRepo { get; private set; }
    public ShowRepository ShowRepo { get; private set; }
    public TravelRepository TravelRepo { get; private set; }
    public WorkRepository WorkRepo { get; private set; }

    public PersonRepository PersonRepo { get; private set; }
    public CompanyRepository CompanyRepo { get; internal set; }
    public LocationRepository LocationRepo { get; internal set; }
    public TransportRepository TransportRepo { get; internal set; }
    public MediaTypeRepository MediaTypeRepo { get; internal set; }
    public SeriesRepository SeriesRepo { get; private set; }
    public FranchiseRepository FranchiseRepo { get; internal set; }
    public ScoreRepository ScoreRepo { get; internal set; }

    public UnitOfWork(DomLContext context)
    {
        _context = context;

        ActivityRepo = new ActivityRepository(_context);
        
        AutoRepo = new AutoRepository(_context);
        BookRepo = new BookRepository(_context);
        ComicRepo = new ComicRepository(_context);
        CourseRepo = new CourseRepository(_context);
        DoomRepo = new DoomRepository(_context);
        EventRepo = new EventRepository(_context);
        GameRepo = new GameRepository(_context);
        GiftRepo = new GiftRepository(_context);
        HealthRepo = new HealthRepository(_context);
        MovieRepo = new MovieRepository(_context);
        PetRepo = new PetRepository(_context);
        MeetRepo = new MeetRepository(_context);
        PlayRepo = new PlayRepository(_context);
        PurchaseRepo = new PurchaseRepository(_context);
        ShowRepo = new ShowRepository(_context);
        TravelRepo = new TravelRepository(_context);
        WorkRepo = new WorkRepository(_context);

        SeriesRepo = new SeriesRepository(_context);
        PersonRepo = new PersonRepository(_context);
        MediaTypeRepo = new MediaTypeRepository(_context);
        CompanyRepo = new CompanyRepository(_context);
        LocationRepo = new LocationRepository(_context);
        TransportRepo = new TransportRepository(_context);
        FranchiseRepo = new FranchiseRepository(_context);
        ScoreRepo = new ScoreRepository(_context);
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
