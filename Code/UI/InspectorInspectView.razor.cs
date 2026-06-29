namespace Inspector.UI;

public partial class InspectorInspectView
{
	[Parameter, EditorRequired] public InspectorService Service { get; set; } = null!;

	private GameObject? Selected => Service.SelectedGameObject;

	private IEnumerable<Component> Components => Selected.IsValid()
		? Selected!.Components.GetAll().Where( x => x.IsValid() )
		: [];

	private static string ComponentTitle( Component component )
	{
		var title = Game.TypeLibrary.GetType( component.GetType() )?.Title;
		return string.IsNullOrWhiteSpace( title ) ? component.GetType().Name : title;
	}

	private static string ComponentIcon( Component component ) => component switch
	{
		ModelRenderer => "view_in_ar",
		Collider => "deployed_code",
		CameraComponent => "videocam",
		Light => "light_mode",
		_ => "extension"
	};

	protected override int BuildHash()
	{
		return HashCode.Combine( Selected?.Id, Time.Now.CeilToInt() );
	}
}
