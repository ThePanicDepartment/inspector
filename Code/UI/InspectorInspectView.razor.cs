using Inspector.Properties;

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

	private IEnumerable<InspectorPropertyDescriptor> CustomDescriptors( Component component )
	{
		return Service.GetComponentDescriptors( component ).Where( IsCustomEditorDescriptor );
	}

	private static bool ShouldShowControlSheetProperty( SerializedProperty property )
	{
		if ( !InspectorPropertyDescriptorProvider.IsInspectableProperty( property ) )
			return false;

		return !IsCustomEditorType( property.PropertyType );
	}

	private static bool IsCustomEditorDescriptor( InspectorPropertyDescriptor descriptor )
	{
		return IsCustomEditorType( descriptor.PropertyType );
	}

	private static bool IsCustomEditorType( Type type )
	{
		return type == typeof( int ) ||
			type == typeof( Guid ) ||
			type == typeof( Rotation );
	}

	protected override int BuildHash()
	{
		return HashCode.Combine( Selected?.Id, Time.Now.CeilToInt() );
	}
}
