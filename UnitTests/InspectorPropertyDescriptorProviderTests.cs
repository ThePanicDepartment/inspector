using System.Linq;
using Inspector.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sandbox;

namespace Inspector;

[TestClass]
public sealed class InspectorPropertyDescriptorProviderTests
{
	[TestMethod]
	public void GetKindMapsSupportedTypes()
	{
		Assert.AreEqual( InspectorPropertyKind.Boolean, InspectorPropertyDescriptorProvider.GetKind( typeof( bool ) ) );
		Assert.AreEqual( InspectorPropertyKind.Integer, InspectorPropertyDescriptorProvider.GetKind( typeof( int ) ) );
		Assert.AreEqual( InspectorPropertyKind.Guid, InspectorPropertyDescriptorProvider.GetKind( typeof( System.Guid ) ) );
		Assert.AreEqual( InspectorPropertyKind.Float, InspectorPropertyDescriptorProvider.GetKind( typeof( float ) ) );
		Assert.AreEqual( InspectorPropertyKind.Vector3, InspectorPropertyDescriptorProvider.GetKind( typeof( Vector3 ) ) );
		Assert.AreEqual( InspectorPropertyKind.Rotation, InspectorPropertyDescriptorProvider.GetKind( typeof( Rotation ) ) );
		Assert.AreEqual( InspectorPropertyKind.GameObject, InspectorPropertyDescriptorProvider.GetKind( typeof( GameObject ) ) );
	}

	[TestMethod]
	public void GetDescriptorsReturnsCachedDescriptorList()
	{
		var provider = new InspectorPropertyDescriptorProvider();
		var scene = new Scene();

		using ( scene.Push() )
		{
			var target = new GameObject();
			var first = provider.GetDescriptors( target );
			var second = provider.GetDescriptors( target );

			Assert.AreSame( first, second );
			Assert.IsTrue( first.Count > 0 );
			Assert.IsTrue( first.Any( x => x.Name == nameof( GameObject.Name ) ) );
		}
	}

	[TestMethod]
	public void GetDescriptorsOnlyReturnsPropertyAttributeMembers()
	{
		var provider = new InspectorPropertyDescriptorProvider();
		var target = new PropertyAttributeTestComponent();

		var descriptors = provider.GetDescriptors( target );

		Assert.IsTrue( descriptors.Any( x => x.Name == nameof( PropertyAttributeTestComponent.VisibleValue ) ) );
		Assert.IsFalse( descriptors.Any( x => x.Name == nameof( PropertyAttributeTestComponent.HiddenValue ) ) );
	}

	private sealed class PropertyAttributeTestComponent : Component
	{
		[Property]
		public float VisibleValue { get; set; }

		public float HiddenValue { get; set; }
	}
}
