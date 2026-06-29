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

	private static string AccentClass( InspectorWatchMetric metric ) => metric.Name switch
	{
		"Position X" => "accent-blue",
		"Position Y" or "KB In" => "accent-green",
		"Position Z" or "KB Out" => "accent-purple",
		_ => "accent-blue"
	};

	private static IEnumerable<string> SparklineBars( InspectorWatchMetric metric )
	{
		var samples = metric.Samples;

		if ( samples.Count == 0 )
		{
			yield return "height-1";
			yield break;
		}

		var min = float.MaxValue;
		var max = float.MinValue;

		for ( var i = 0; i < samples.Count; i++ )
		{
			var value = samples.GetChronological( i );
			min = Math.Min( min, value );
			max = Math.Max( max, value );
		}

		var range = Math.Max( 0.001f, max - min );

		for ( var i = 0; i < samples.Count; i++ )
		{
			var normalized = (samples.GetChronological( i ) - min) / range;
			var height = Math.Clamp( (int)MathF.Round( normalized * 7f ) + 1, 1, 8 );
			yield return $"height-{height}";
		}
	}

	protected override int BuildHash()
	{
		return HashCode.Combine( Service.WatchMetrics.Count, Time.Now.CeilToInt() );
	}
}
