namespace Inspector.UI;

public partial class InspectorGizmoToolbar
{
	private static readonly InspectorGizmoMode[] Modes =
	[
		InspectorGizmoMode.Select,
		InspectorGizmoMode.Move,
		InspectorGizmoMode.Rotate,
		InspectorGizmoMode.Scale
	];

	[Parameter, EditorRequired] public InspectorService Service { get; set; } = null!;

	private static string LabelFor( InspectorGizmoMode mode ) => mode.ToString();

	private static string IconFor( InspectorGizmoMode mode ) => mode switch
	{
		InspectorGizmoMode.Select => "near_me",
		InspectorGizmoMode.Move => "open_with",
		InspectorGizmoMode.Rotate => "rotate_right",
		InspectorGizmoMode.Scale => "select_all",
		_ => "radio_button_unchecked"
	};
}
