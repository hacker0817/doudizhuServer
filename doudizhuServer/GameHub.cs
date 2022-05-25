using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

namespace doudizhuServer
{
    [Authorize]
    public class GameHub : Hub
    {
        private readonly ILogger _logger;
        private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();

        public GameHub(ILogger<GameHub> logger)
        {
            _logger = logger;
        }

        public void SendMessageToServer(string message)
        {
            _logger.LogInformation(message);
        }

        public async Task SendMessageToUser(string user, string message)
        {
            await Clients.User(user).SendAsync("ReceiveMessage", user, message);
        }

        public override async Task OnConnectedAsync()
        {
            //var UserId = Context.User.Claims.Where(c => c.Type == "UserId").FirstOrDefault().Value;
            _logger.LogInformation(Context.UserIdentifier);
            _connections.Add(Context.UserIdentifier, Context.ConnectionId);
            //await Groups.AddToGroupAsync(Context.ConnectionId, "Waiting Users");

            if (_connections.Count >= 2)
                _logger.LogInformation("游戏开始");
            
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _connections.Remove(Context.UserIdentifier, Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
