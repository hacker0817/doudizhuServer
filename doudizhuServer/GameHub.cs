﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

namespace doudizhuServer
{
    [Authorize]
    public class GameHub : Hub
    {
        private readonly ILogger _logger;
        private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();

        public static object userLock = new object();

        public GameHub(ILogger<GameHub> logger)
        {
            _logger = logger;
        }

        public void SendMessageToServer(string message)
        {
            if (message.Equals("ready"))
            {
                var UserId = Context.UserIdentifier;
                lock (userLock)
                {
                    if (GameCenter.userList.Count >= 3)
                    {
                        GameRoom room = new GameRoom();
                        room.RoomId = Guid.NewGuid().ToString();
                        room.RoomType = "DouDiZhu";

                        for (int i = 0; i < 3; i++)
                        {
                            string userid = GameCenter.userList[i];
                            Player player = new Player(userid);
                            room.players.Add(player);
                        }
                        Clients.Users(room.players.Select(p => p.UserId).ToList()).SendAsync("ReceiveMessage", "", "GameStart!");
                    }
                    else
                        Clients.User(Context.UserIdentifier).SendAsync("ReceiveMessage", "", $"Now User Count {GameCenter.userList.Count} ,Please wait...");
                }
            }
            _logger.LogInformation(message);
        }

        public async Task SendMessageToUser(string user, string message)
        {
            await Clients.User(user).SendAsync("ReceiveMessage", user, message);
        }

        public override async Task OnConnectedAsync()
        {
            //var UserId = Context.User.Claims.Where(c => c.Type == "UserId").FirstOrDefault().Value;
            var UserId = Context.UserIdentifier;
            _logger.LogInformation(UserId + " Connected");
            lock (userLock)
            {
                if (!GameCenter.userList.Contains(UserId))
                    GameCenter.userList.Add(UserId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var UserId = Context.UserIdentifier;
            _logger.LogInformation(UserId + " DisConnected");
            lock (userLock)
            {
                GameCenter.userList.Remove(UserId);
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
