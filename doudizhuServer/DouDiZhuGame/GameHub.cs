using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

namespace doudizhuServer
{
    [Authorize]
    public class GameHub : Hub
    {
        private readonly ILogger _logger;

        public static object userLock = new object();

        public GameHub(ILogger<GameHub> logger)
        {
            _logger = logger;
        }

        public void ReadyGame()
        {
            var UserId = Context.UserIdentifier;
            lock (userLock)
            {
                if (!GameCenter.waitingUsers.Contains(UserId))
                    GameCenter.waitingUsers.Add(UserId);

                if (GameCenter.waitingUsers.Count >= 3)
                {
                    GameRoom room = new GameRoom();
                    string roomId = Guid.NewGuid().ToString();
                    room.RoomId = roomId;
                    room.RoomType = "DouDiZhu";

                    for (int i = 0; i < 3; i++)
                    {
                        string userid = GameCenter.waitingUsers[i];
                        Player player = new Player(userid);
                        player.Num = i + 1;
                        room.players.Add(player);
                        Clients.User(userid).SendAsync("UserNum", player.Num);
                    }

                    room.InitGame();
                    GameCenter.dicGameRoom.Add(roomId, room);
                    Clients.Users(room.players.Select(p => p.UserId).ToList()).SendAsync("GameStart", roomId);
                    GameCenter.waitingUsers.Remove(UserId);
                }
                else
                    Clients.User(UserId).SendAsync("UserCount", GameCenter.waitingUsers.Count.ToString());
            }
        }

        public void DealPoker(string roomId, string userNum)
        {
            var UserId = Context.UserIdentifier;
            _logger.LogInformation("roomId:" + roomId + ",UserId:" + UserId + ",UserNum:" + userNum);
            if (GameCenter.dicGameRoom.ContainsKey(roomId))
            {
                var room = GameCenter.dicGameRoom[roomId];
                if (Convert.ToInt32(userNum) == room.DealerNum)
                {
                    while (room.DealingCount < 51)
                    {
                        int dealerNum = room.DealerNum;
                        int r = room.DealPoker();
                        Clients.User(room.GetPlayerByNum(dealerNum).UserId).SendAsync("deal", r);
                    }
                }
            }
        }

        public override async Task OnConnectedAsync()
        {
            //var UserId = Context.User.Claims.Where(c => c.Type == "UserId").FirstOrDefault().Value;
            var UserId = Context.UserIdentifier;
            _logger.LogInformation(UserId + " Connected");

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var UserId = Context.UserIdentifier;
            _logger.LogInformation(UserId + " DisConnected");
            lock (userLock)
                GameCenter.waitingUsers.Remove(UserId);

            await base.OnDisconnectedAsync(exception);
        }
    }
}
