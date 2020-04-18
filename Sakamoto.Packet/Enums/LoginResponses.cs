namespace Sakamoto.Packet.Enums
{
	public enum LoginResponses
	{
		invalid_credentials = -1,
		outdated_client = -2,
		user_banned = -3,
		multiaccount_detected = -4,
		server_error = -5,
		cutting_edge_multiplayer = -6,
		account_password_rest = -7,
		verification_required = -8
	}
}
