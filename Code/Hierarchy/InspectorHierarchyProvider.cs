using System;

namespace Inspector.Hierarchy;

/// <summary>
/// Builds a flattened scene hierarchy for the inspector tree.
/// </summary>
public sealed class InspectorHierarchyProvider
{
	private readonly List<InspectorHierarchyNode> _nodes = [];

	public IReadOnlyList<InspectorHierarchyNode> Build( Scene? scene, string? search )
	{
		_nodes.Clear();

		if ( scene is null )
			return _nodes;

		foreach ( var child in scene.Children )
		{
			AddNode( child, 0, search );
		}

		return _nodes;
	}

	private void AddNode( GameObject gameObject, int depth, string? search )
	{
		if ( !gameObject.IsValid() )
			return;

		var matches = string.IsNullOrWhiteSpace( search )
			|| gameObject.Name.Contains( search, StringComparison.OrdinalIgnoreCase );

		if ( matches )
			_nodes.Add( new( gameObject, depth ) );

		foreach ( var child in gameObject.Children )
		{
			AddNode( child, depth + 1, search );
		}
	}
}
