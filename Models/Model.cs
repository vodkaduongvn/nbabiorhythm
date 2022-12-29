namespace NBABiorhythm.Models
{
    public class GameDateModel
    {
        public Model Model { get; set; }
        public DateTime GameDateTimeUTC { get; set; }
    }
    public class Model
    {
        public string Team { get; set; }
        public List<Player> Players { get; set; }
    }
    public class Player
    {
        public string Name { get; set; }
        public string Birthday { get; set; }
        public double Value { get; set; }
        public bool IsActive { get; set; }
    }
}
