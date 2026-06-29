using Inspector.Hierarchy;

namespace Inspector.UI;

public partial class InspectorHierarchyView
{
	private static readonly string[] Filters = [ "All", "Entities", "Lights", "Cameras", "Volumes", "UI" ];
	private readonly HashSet<string> _collapsedNodes = [];

	[Parameter, EditorRequired] public InspectorService Service { get; set; } = null!;

	private string ActiveFilter { get; set; } = "All";
	private bool IsSceneExpanded { get; set; } = true;

	private void ResetFilters()
	{
		ActiveFilter = "All";
	}

	private IReadOnlyList<InspectorHierarchyNode> Nodes => VisibleNodes().ToArray();

	private IEnumerable<InspectorHierarchyNode> VisibleNodes()
	{
		if ( !IsSceneExpanded )
			yield break;

		var hiddenDescendantDepth = int.MaxValue;
		foreach ( var node in Service.Hierarchy.Build( Game.ActiveScene, Service.SearchText ) )
		{
			if ( node.Depth > hiddenDescendantDepth )
				continue;

			hiddenDescendantDepth = int.MaxValue;

			if ( !MatchesFilter( node.GameObject ) )
				continue;

			yield return node;

			if ( node.HasChildren && IsCollapsed( node.GameObject ) )
				hiddenDescendantDepth = node.Depth;
		}
	}

	private bool IsCollapsed( GameObject gameObject ) => _collapsedNodes.Contains( NodeKey( gameObject ) );

	private void ToggleScene()
	{
		IsSceneExpanded = !IsSceneExpanded;
	}

	private void ToggleNode( InspectorHierarchyNode node )
	{
		var key = NodeKey( node.GameObject );

		if ( !_collapsedNodes.Add( key ) )
			_collapsedNodes.Remove( key );
	}

	private static string NodeKey( GameObject gameObject ) => gameObject.Id.ToString();

	private bool MatchesFilter( GameObject gameObject )
	{
		return ActiveFilter switch
		{
			"Lights" => gameObject.Components.GetAll().Any( x => x is Light ),
			"Cameras" => gameObject.Components.GetAll().Any( x => x is CameraComponent ),
			"UI" => gameObject.Components.GetAll().Any( x => x is PanelComponent ),
			_ => true
		};
	}

	private static string IconFor( GameObject gameObject )
	{
		if ( gameObject.Components.GetAll().Any( x => x is Light ) )
			return "light_mode";

		if ( gameObject.Components.GetAll().Any( x => x is CameraComponent ) )
			return "videocam";

		if ( gameObject.Components.GetAll().Any( x => x is PanelComponent ) )
			return "dashboard";

		return "deployed_code";
	}

	protected override int BuildHash()
	{
		return HashCode.Combine(
			Service.SearchText,
			ActiveFilter,
			Service.SelectedGameObject?.Id,
			IsSceneExpanded,
			_collapsedNodes.Count,
			Time.Now.CeilToInt() );
	}
}
