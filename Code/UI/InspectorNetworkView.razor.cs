using Inspector.Network;

namespace Inspector.UI;

public partial class InspectorNetworkView
{
	[Parameter, EditorRequired] public InspectorService Service { get; set; } = null!;

	private InspectorNetworkSnapshot Snapshot => Service.NetworkSnapshot;

	private string PositionText => Service.SelectedGameObject.IsValid()
		? FormatVector( Service.SelectedGameObject!.WorldPosition )
		: "n/a";

	private string RotationText
	{
		get
		{
			if ( !Service.SelectedGameObject.IsValid() )
				return "n/a";

			var angles = Service.SelectedGameObject!.WorldRotation.Angles();
			return $"{angles.pitch:0.###}, {angles.yaw:0.###}, {angles.roll:0.###}";
		}
	}

	private string AuthorityText => Service.SelectedGameObject?.IsProxy == true ? "Proxy" : "Local";

	private static string FormatVector( Vector3 value ) => $"{value.x:0.###}, {value.y:0.###}, {value.z:0.###}";

	private static string BarClass( int index ) => index % 17 == 0
		? "dropped"
		: index % 5 == 0 ? "sent" : "acked";

	protected override int BuildHash()
	{
		return HashCode.Combine( Snapshot, Service.SelectedGameObject?.Id, Time.Now.CeilToInt() );
	}
}
