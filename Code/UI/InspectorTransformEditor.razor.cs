namespace Inspector.UI;

public partial class InspectorTransformEditor
{
	[Parameter, EditorRequired] public GameObject? Target { get; set; }

	private Vector3 Position => Target.IsValid() ? Target!.WorldPosition : Vector3.Zero;
	private Vector3 Scale => Target.IsValid() ? Target!.LocalScale : Vector3.One;

	private Vector3 RotationAngles
	{
		get
		{
			if ( !Target.IsValid() )
				return Vector3.Zero;

			var angles = Target!.WorldRotation.Angles();
			return new( angles.pitch, angles.yaw, angles.roll );
		}
	}

	private void SetPosition( Vector3 value )
	{
		if ( Target.IsValid() )
			Target!.WorldPosition = value;
	}

	private void SetRotationAngles( Vector3 value )
	{
		if ( Target.IsValid() )
			Target!.WorldRotation = new Angles( value.x, value.y, value.z );
	}

	private void SetScale( Vector3 value )
	{
		if ( Target.IsValid() )
			Target!.LocalScale = value;
	}

	protected override int BuildHash()
	{
		return HashCode.Combine( Position, RotationAngles, Scale );
	}
}
