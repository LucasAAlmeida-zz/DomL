using DomL.Business.Entities;
using DomL.Business.Utils;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;

namespace DomL.DataAccess
{
    public class GameRepository : DomLRepository<GameActivity>
    {
        public GameRepository(DomLContext context) : base(context) { }

        public DomLContext DomLContext
        {
            get { return Context as DomLContext; }
        }

        public void CreateGameActivity(GameActivity gameActivity)
        {
            DomLContext.GameActivity.Add(gameActivity);
        }

        //TODO add year to search
        public Game GetGameByTitle(string title)
        {
            var cleanTitle = Util.CleanString(title);
            return DomLContext.Game
                .Include(u => u.Platform)
                .Include(u => u.Series)
                .Include(u => u.Person)
                .Include(u => u.Company)
                .SingleOrDefault(u =>
                    u.Title.Replace(":", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").Replace("'", "").Replace(",", "").ToLower().Replace("the", "")
                    == cleanTitle
                );
        }

        public void CreateGame(Game game)
        {
            DomLContext.Game.Add(game);
        }

        public List<Game> GetAllGames()
        {
            return DomLContext.Game.ToList();
        }
    }
}
