using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sandbox;

namespace Inspector;

[TestClass]
public sealed class InspectorTransformEditingTests
{
	[TestMethod]
	public void GameObjectTransformCanBeEditedAtRuntime()
	{
		var scene = new Scene();
		using ( scene.Push() )
		{
			var gameObject = new GameObject();
			var position = new Vector3( 10f, 20f, 30f );
			var scale = new Vector3( 2f, 3f, 4f );

			gameObject.WorldPosition = position;
			gameObject.WorldRotation = new Angles( 5f, 15f, 25f );
			gameObject.LocalScale = scale;

			Assert.AreEqual( position, gameObject.WorldPosition );
			Assert.AreEqual( scale, gameObject.LocalScale );
			Assert.AreEqual( 15f, gameObject.WorldRotation.Angles().yaw, 0.001f );
		}
	}
}