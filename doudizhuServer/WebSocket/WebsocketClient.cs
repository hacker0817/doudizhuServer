using System.Net.WebSockets;
using System.Text;

namespace doudizhuServer
{
    public class WebsocketClient
    {
        public WebSocket WebSocket { get; set; }

        public string Id { get; set; }

        public string RoomNo { get; set; }

        public Task SendMessageAsync(string message)
        {
            var msg = Encoding.UTF8.GetBytes(message);
            return WebSocket.SendAsync(new ArraySegment<byte>(msg, 0, msg.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
