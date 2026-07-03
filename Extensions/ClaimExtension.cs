using System.Security.Claims;

namespace CeramicsShopMasterApi.Extensions
{
	public static class ClaimExtension
	{
		public static string? GetUsername(this ClaimsPrincipal user) => user.FindFirst("username")?.Value;
		public static string? GetUuid(this ClaimsPrincipal user) => user.FindFirst("uuid")?.Value;
		public static string? GetShopUuid(this ClaimsPrincipal user) => user.FindFirst("shop_uuid")?.Value;
	}
}
