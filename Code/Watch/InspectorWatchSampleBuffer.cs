using System;

namespace Inspector.Watch;

/// <summary>
/// Allocation-stable ring buffer for watch sparklines.
/// </summary>
public sealed class InspectorWatchSampleBuffer
{
	private readonly float[] _values;
	private int _nextIndex;

	public InspectorWatchSampleBuffer( int capacity )
	{
		_values = new float[Math.Max( 1, capacity )];
	}

	public int Count { get; private set; }

	public int Capacity => _values.Length;

	public void Push( float value )
	{
		_values[_nextIndex] = value;
		_nextIndex = (_nextIndex + 1) % _values.Length;
		Count = Math.Min( Count + 1, _values.Length );
	}

	public float GetChronological( int index )
	{
		if ( index < 0 || index >= Count )
			throw new ArgumentOutOfRangeException( nameof( index ) );

		var start = Count == _values.Length ? _nextIndex : 0;
		return _values[(start + index) % _values.Length];
	}

	public string ToSparklinePath()
	{
		if ( Count == 0 )
			return string.Empty;

		var min = float.MaxValue;
		var max = float.MinValue;

		for ( var i = 0; i < Count; i++ )
		{
			var value = GetChronological( i );
			min = Math.Min( min, value );
			max = Math.Max( max, value );
		}

		var range = Math.Max( 0.001f, max - min );
		var parts = new string[Count];

		for ( var i = 0; i < Count; i++ )
		{
			var x = Count == 1 ? 0f : i / (Count - 1f) * 100f;
			var y = 100f - ((GetChronological( i ) - min) / range * 100f);
			parts[i] = $"{x:0.#},{y:0.#}";
		}

		return string.Join( " ", parts );
	}
}
