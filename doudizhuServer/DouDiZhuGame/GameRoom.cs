namespace doudizhuServer
{
    public class GameRoom
    {
        public string RoomId { get; set; }
        public string RoomType { get; set; }
        
        public List<Player> players = new List<Player>();
    }
}
