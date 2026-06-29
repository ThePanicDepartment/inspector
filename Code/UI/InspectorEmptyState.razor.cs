namespace Inspector.UI;

public partial class InspectorEmptyState
{
	[Parameter] public string Icon { get; set; } = "info";
	[Parameter] public string Message { get; set; } = "Nothing to inspect.";
}
