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

        public void ReadyGame(string message)
        {
            _logger.LogInformation(message);
            if (message.Equals("ready"))
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
                            player.Num = (i + 1);
                            room.players.Add(player);
                        }
                        GameCenter.dicGameRoom.Add(roomId, room);
                        Clients.Users(room.players.Select(p => p.UserId).ToList()).SendAsync("GameStart", roomId);
                        GameCenter.waitingUsers.Remove(UserId);
                    }
                    else
                        Clients.User(UserId).SendAsync("UserCount", GameCenter.waitingUsers.Count.ToString());
                }
            }
        }

        public void JoinGame(string roomId)
        {
            _logger.LogInformation("roomId:" + roomId);

            if (GameCenter.dicGameRoom.ContainsKey(roomId))
            {
                GameRoom room = GameCenter.dicGameRoom[roomId];
                var UserId = Context.UserIdentifier;
                Player gamePlayer = room.players.FirstOrDefault(p => p.UserId == UserId);
                gamePlayer.online = true;
                int onlineUserCount = 0;
                foreach (Player player in room.players)
                {
                    if (player.online)
                        onlineUserCount++;
                }
                if (onlineUserCount == 3)
                {
                    string gameInfo = room.InitGame();
                    Clients.Users(room.players.Select(p => p.UserId).ToList()).SendAsync("Dealing", gameInfo);
                }
                else
                {
                    Clients.Users(UserId).SendAsync("UserNum", gamePlayer.Num);
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
