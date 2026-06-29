using Inspector.Hierarchy;

namespace Inspector.UI;

public partial class InspectorHierarchyView
{
	private static readonly string[] Filters = [ "All", "Entities", "Lights", "Cameras", "Volumes", "UI" ];

	[Parameter, EditorRequired] public InspectorService Service { get; set; } = null!;

	private string ActiveFilter { get; set; } = "All";

	private void ResetFilters()
	{
		ActiveFilter = "All";
	}

	private IReadOnlyList<InspectorHierarchyNode> Nodes => Service.Hierarchy.Build( Game.ActiveScene, Service.SearchText )
		.Where( x => MatchesFilter( x.GameObject ) )
		.ToArray();

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
		return HashCode.Combine( Service.SearchText, ActiveFilter, Service.SelectedGameObject?.Id, Time.Now.CeilToInt() );
	}
}
