using System;
using Inspector.Watch;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inspector;

[TestClass]
public sealed class InspectorWatchSampleBufferTests
{
	[TestMethod]
	public void PushKeepsFixedCapacity()
	{
		var buffer = new InspectorWatchSampleBuffer( 3 );

		buffer.Push( 1f );
		buffer.Push( 2f );
		buffer.Push( 3f );
		buffer.Push( 4f );

		Assert.AreEqual( 3, buffer.Count );
		Assert.AreEqual( 2f, buffer.GetChronological( 0 ) );
		Assert.AreEqual( 4f, buffer.GetChronological( 2 ) );
	}

	[TestMethod]
	public void SparklinePathHasOnePointPerSample()
	{
		var buffer = new InspectorWatchSampleBuffer( 4 );
		buffer.Push( 10f );
		buffer.Push( 20f );
		buffer.Push( 30f );

		var points = buffer.ToSparklinePath().Split( ' ', StringSplitOptions.RemoveEmptyEntries );

		Assert.AreEqual( 3, points.Length );
	}
}