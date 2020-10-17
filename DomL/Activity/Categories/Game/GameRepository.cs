using DomL.Business.Entities;
using DomL.Business.Utils;
using System.Linq;
using System.Data.Entity;
using System.Windows.Documents;
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

        public Game GetGameByTitleAndPlatformName(string title, string platformName)
        {
            var cleanTitle = Util.CleanString(title);
            var cleanPlatformName = Util.CleanString(platformName);
            return DomLContext.Game
                .Include(u => u.Platform)
                .Include(u => u.Series)
                .Include(u => u.Director)
                .Include(u => u.Publisher)
                .SingleOrDefault(u =>
                    u.Title.Replace(":", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").Replace("'", "").Replace(",", "").ToLower().Replace("the", "")
                    == cleanTitle
                    && u.Platform.Name.Replace(":", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").Replace("'", "").Replace(",", "").ToLower().Replace("the", "")
                    == cleanPlatformName
                );
        }

        public void CreateGameActivity(GameActivity gameActivity)
        {
            DomLContext.GameActivity.Add(gameActivity);
        }

        public Game GetGameByTitle(string title)
        {
            var cleanTitle = Util.CleanString(title);
            return DomLContext.Game
                .Include(u => u.Platform)
                .Include(u => u.Series)
                .Include(u => u.Director)
                .Include(u => u.Publisher)
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
