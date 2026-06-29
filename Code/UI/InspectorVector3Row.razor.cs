namespace Inspector.UI;

public partial class InspectorVector3Row
{
	[Parameter] public string Label { get; set; } = string.Empty;
	[Parameter] public string XLabel { get; set; } = "X";
	[Parameter] public string YLabel { get; set; } = "Y";
	[Parameter] public string ZLabel { get; set; } = "Z";
	[Parameter] public Vector3 Value { get; set; }
	[Parameter] public Action<Vector3>? ValueChanged { get; set; }

	private void SetX( float value ) => ValueChanged?.Invoke( new Vector3( value, Value.y, Value.z ) );
	private void SetY( float value ) => ValueChanged?.Invoke( new Vector3( Value.x, value, Value.z ) );
	private void SetZ( float value ) => ValueChanged?.Invoke( new Vector3( Value.x, Value.y, value ) );
}
