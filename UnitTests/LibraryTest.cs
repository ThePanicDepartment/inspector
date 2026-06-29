using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sandbox;

namespace Inspector;

[TestClass]
public class LibraryTests
{
	[TestMethod]
	public void SceneTest()
	{
		var scene = new Scene();
		
		using ( scene.Push() )
		{
			Assert.AreEqual( 1, scene.Directory.GameObjectCount );
		}
	}
}
