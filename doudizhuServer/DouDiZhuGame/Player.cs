namespace doudizhuServer
{
    public class Player
    {
        public Player(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; set; }

        public int Num { get; set; }

        public bool online = false;
    }
}
