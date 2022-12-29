namespace NBABiorhythm.Models
{
    public class ScheduledGames
    {
        public Meta Meta { get; set; }
        public LeagueSchedule LeagueSchedule { get; set; }
        
    }

    public class Meta
    {
        public string Version { get; set; }
        public string Request { get; set; }
        public DateTime Time { get; set; }
    }

   

    public class Team
    {
        public string TeamId { get; set; }
        public string TeamName { get; set; }
        public string TeamCity { get; set; }
        public bool IsHome { get; set; }
    }
    public class Game
    {
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
        public DateTime GameDateTimeUTC { get; set; }
        public DateTime GameDateTimeEst { get; set; }
    }

    public class GameDates
    {
        public DateTime GameDate { get; set; }
        public List<Game> Games { get; set; }
    }

    public class LeagueSchedule
    {
        public string SeasonYear { get; set; }
        public string LeagueId { get; set; }
        public List<GameDates> GameDates { get; set; }
    }
}
