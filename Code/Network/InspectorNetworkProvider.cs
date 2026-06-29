namespace Inspector.Network;

/// <summary>
/// Isolates S&amp;Box connection metrics from the Network view.
/// </summary>
public sealed class InspectorNetworkProvider
{
	public InspectorNetworkSnapshot ReadSnapshot()
	{
		var hostConnection = Connection.Host;
		var localConnection = Connection.Local;

		var bytesIn = 0f;
		var bytesOut = 0f;
		var packetsIn = 0f;
		var packetsOut = 0f;
		var peerCount = 0;

		foreach ( var connection in Connection.All )
		{
			if ( connection == localConnection )
				continue;

			var stats = connection.Stats;
			bytesIn += stats.InBytesPerSecond;
			bytesOut += stats.OutBytesPerSecond;
			packetsIn += stats.InPacketsPerSecond;
			packetsOut += stats.OutPacketsPerSecond;
			peerCount++;
		}

		return new(
			peerCount,
			bytesIn,
			bytesOut,
			packetsIn,
			packetsOut,
			hostConnection is not null,
			localConnection is not null );
	}
}
