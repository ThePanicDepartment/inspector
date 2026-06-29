namespace Inspector.UI;

public partial class InspectorSection
{
	[Parameter] public string Title { get; set; } = string.Empty;
	[Parameter] public string Icon { get; set; } = string.Empty;
	[Parameter] public new RenderFragment? ChildContent { get; set; }

	private bool Collapsed { get; set; }

	private void Toggle() => Collapsed = !Collapsed;
}
