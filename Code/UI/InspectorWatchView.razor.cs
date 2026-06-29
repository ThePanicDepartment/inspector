using Inspector.Watch;

namespace Inspector.UI;

public partial class InspectorWatchView
{
	[Parameter, EditorRequired] public InspectorService Service { get; set; } = null!;

	private IEnumerable<IGrouping<string, InspectorWatchMetric>> GroupedMetrics => Service.WatchMetrics.GroupBy( x => x.Group );

	private static string IconForGroup( string group ) => group switch
	{
		"Transform" => "open_with",
		"Network" => "lan",
		"Gameplay" => "sports_esports",
		_ => "monitoring"
	};

	protected override int BuildHash()
	{
		return HashCode.Combine( Service.WatchMetrics.Count, Time.Now.CeilToInt() );
	}
}
