using System.Collections.Concurrent;

namespace doudizhuServer
{
    public class GameCenter
    {
        public static List<string> waitingUsers = new List<string>();
        public static Dictionary<string, GameRoom> dicGameRoom = new Dictionary<string, GameRoom>();
    }
}
