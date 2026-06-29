using Inspector.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inspector;

[TestClass]
public sealed class InspectorRouterTests
{
	[TestMethod]
	public void NavigateUpdatesActiveRouteAndHistory()
	{
		var router = new InspectorRouter();

		Assert.IsTrue( router.Navigate( InspectorRoute.Hierarchy ) );
		Assert.AreEqual( InspectorRoute.Hierarchy, router.ActiveRoute );
		Assert.AreEqual( 1, router.History.Count );
		Assert.AreEqual( InspectorRoute.Inspect, router.History[0] );
	}

	[TestMethod]
	public void BackAndForwardRestoreRoutes()
	{
		var router = new InspectorRouter();
		router.Navigate( InspectorRoute.Watch );
		router.Navigate( InspectorRoute.Network );

		Assert.IsTrue( router.TryGoBack() );
		Assert.AreEqual( InspectorRoute.Watch, router.ActiveRoute );

		Assert.IsTrue( router.TryGoForward() );
		Assert.AreEqual( InspectorRoute.Network, router.ActiveRoute );
	}
}