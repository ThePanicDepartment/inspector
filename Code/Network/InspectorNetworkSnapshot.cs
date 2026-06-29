namespace Inspector.Network;

/// <summary>
/// Read-only network metrics available to the runtime inspector.
/// </summary>
public sealed record InspectorNetworkSnapshot(
	int PeerCount,
	float BytesInPerSecond,
	float BytesOutPerSecond,
	float PacketsInPerSecond,
	float PacketsOutPerSecond,
	bool HasHost,
	bool HasLocalConnection )
{
	public static InspectorNetworkSnapshot Empty { get; } = new( 0, 0f, 0f, 0f, 0f, false, false );
}
