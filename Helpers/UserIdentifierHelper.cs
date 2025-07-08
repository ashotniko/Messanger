using System.Security.Claims;

namespace Messanger.Helpers;

public class UserIdentifierHelper
{
    public static bool IsSenderReceiverOrAdmin(ClaimsPrincipal user, int? senderId, int? receiverId)
    {
        var myId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        var isAdmin = user.IsInRole("Admin");

        return isAdmin ||
            myId == senderId?.ToString() ||
            myId == receiverId?.ToString();
    }

    public static bool IsSelfOrAdmin(ClaimsPrincipal user, int? userId)
    {
        var myId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        var isAdmin = user.IsInRole("Admin");

        return isAdmin || myId == userId?.ToString();
    }
}
