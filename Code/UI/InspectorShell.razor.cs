namespace Inspector.UI;

public partial class InspectorShell
{
	[Parameter, EditorRequired] public InspectorService Service { get; set; } = null!;

	private string SearchPlaceholder => Service.ActiveRoute switch
	{
		InspectorRoute.Hierarchy => "Search entities...",
		InspectorRoute.Watch => "Search watched values...",
		InspectorRoute.Network => "Search network state...",
		_ => "Search components & properties..."
	};

	private void Close() => Service.SetOpen( false );
}
