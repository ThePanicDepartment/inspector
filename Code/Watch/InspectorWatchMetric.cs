using System;

namespace Inspector.Watch;

/// <summary>
/// A live value shown in the Watch view.
/// </summary>
public sealed class InspectorWatchMetric
{
	public string Group { get; init; } = "Runtime";
	public string Name { get; init; } = string.Empty;
	public string Unit { get; init; } = string.Empty;
	public Color Accent { get; init; } = Color.White;
	public Func<float> ReadValue { get; init; } = () => 0f;
	public InspectorWatchSampleBuffer Samples { get; } = new( 32 );

	public float LatestValue { get; private set; }

	public void Sample()
	{
		LatestValue = ReadValue();
		Samples.Push( LatestValue );
	}

	public string FormatValue() => Unit.Length > 0
		? $"{LatestValue:0.###} {Unit}"
		: $"{LatestValue:0.###}";
}
