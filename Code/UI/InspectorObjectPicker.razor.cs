namespace Inspector.UI;

public partial class InspectorObjectPicker : Panel
{
	[Parameter, EditorRequired] public InspectorService Service { get; set; } = null!;

	private string SelectedName => Service.SelectedGameObject.IsValid()
		? Service.SelectedGameObject!.Name
		: "No Selection";

	private void SelectFirstObject()
	{
		var first = Game.ActiveScene?.Children.FirstOrDefault( x => x.IsValid() );
		Service.Select( first );
	}
}
