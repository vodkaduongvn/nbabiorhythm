using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NBABiorhythm.Models;
using Newtonsoft.Json;

namespace NBABiorhythm.Pages
{
    public class IndexModel : PageModel
    {
        public List<GameDateModel> Results { get; set; }

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task OnGet()
        {
            var scheduledGames = await GetScheduledGames();
            Results = CalPlayersBiorhythm(ReadFileData(), scheduledGames);
        }

        private List<GameDateModel> CalPlayersBiorhythm(List<Model> models, ScheduledGames scheduledGames)
        {
            //var birthday = new DateTime(1983, 5, 27);

            var result = new List<GameDateModel>();

            var todayGames = scheduledGames.LeagueSchedule.GameDates.Where(x => x.GameDate.ToShortDateString() == DateTime.Now.AddDays(-1).ToShortDateString()).FirstOrDefault()?.Games;

            foreach (var game in todayGames)
            {
                foreach (var team in models)
                {
                    if (team.Team.Contains($"{game.HomeTeam.TeamCity} {game.HomeTeam.TeamName}")
                        || team.Team.Contains($"{game.AwayTeam.TeamCity} {game.AwayTeam.TeamName}"))
                    {
                        foreach (var player in team.Players.Where(x => x.IsActive == true))
                        {
                            foreach (var date in CreateDates())
                            {
                                var daysDiff = Math.Round((date - DateTime.Parse(player.Birthday)).TotalDays);
                                var cycle = daysDiff / 23;
                                var sinus = Math.Sin((2 * Math.PI) * cycle);
                                var percent = Math.Round(((sinus + 1) * 100) / 2);
                                player.Value = percent;
                            }
                        }
                        result.Add(new GameDateModel { Model = team, GameDateTimeUTC = game.GameDateTimeUTC });
                    }
                }
            }

            return result;
        }

        private List<DateTime> CreateDates()
        {
            var range = 0;
            var datesRange = new List<DateTime>();

            var startDate = DateTime.UtcNow;
            datesRange.Add(startDate);

            for (var before = 1; before <= range; before++)
            {
                var b = startDate.AddDays(-before);
                datesRange.Add(b);
            }

            for (var after = 1; after <= range; after++)
            {
                var a = startDate.AddDays(+after);
                datesRange.Add(a);
            }

            return datesRange.OrderBy(x => x).ToList();
        }

        private List<Model> ReadFileData()
        {
            return JsonConvert.DeserializeObject<List<Model>>(System.IO.File.ReadAllText(@"Data\\data.json"));
        }

        private async Task<ScheduledGames> GetScheduledGames()
        {
            var httpClient = new HttpClient();
            var scheduledGames = new ScheduledGames();
            using (httpClient)
            {
                var res = await httpClient.GetAsync("https://cdn.nba.com/static/json/staticData/scheduleLeagueV2_9.json");
                scheduledGames = JsonConvert.DeserializeObject<ScheduledGames>(await res.Content.ReadAsStringAsync());
            }

            return scheduledGames;
        }
    }
}