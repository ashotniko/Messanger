using Microsoft.AspNetCore.SignalR;

namespace Messanger.Hubs
{
    public class ChatHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var userId = this.Context.GetHttpContext()!.Request.Query["userId"].ToString();

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("UserId cannot be null or empty.", nameof(userId));
            }

            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, $"user-{userId}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = this.Context.GetHttpContext()!.Request.Query["userId"].ToString();
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("UserId cannot be null or empty.", nameof(userId));
            }
            await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, $"user-{userId}");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
