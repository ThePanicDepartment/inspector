namespace Inspector.UI;

public partial class InspectorSearchBox
{
	[Parameter, EditorRequired] public InspectorService Service { get; set; } = null!;
	[Parameter] public string Placeholder { get; set; } = "Search...";
}
