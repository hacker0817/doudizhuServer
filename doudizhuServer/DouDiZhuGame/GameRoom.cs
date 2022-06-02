using doudizhuServer.Models;
using Microsoft.AspNetCore.SignalR;

namespace doudizhuServer
{
    public class GameRoom
    {
        public string RoomId { get; set; }
        public string RoomType { get; set; }
        
        public List<Player> players = new List<Player>();

        public string InitGame()
        {
            return "";
        }
    }
}
