namespace Inspector.UI;

public partial class InspectorTabs : Panel
{
	private static readonly InspectorRoute[] Routes =
	[
		InspectorRoute.Inspect,
		InspectorRoute.Hierarchy,
		InspectorRoute.Watch,
		InspectorRoute.Network
	];

	[Parameter, EditorRequired] public InspectorService Service { get; set; } = null!;

	private static string LabelFor( InspectorRoute route ) => route switch
	{
		InspectorRoute.Inspect => "Inspect",
		InspectorRoute.Hierarchy => "Hierarchy",
		InspectorRoute.Watch => "Watch",
		InspectorRoute.Network => "Network",
		_ => route.ToString()
	};
}
