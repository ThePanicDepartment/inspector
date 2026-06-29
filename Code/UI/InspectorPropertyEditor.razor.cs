using System;
using System.Globalization;
using Inspector.Properties;

namespace Inspector.UI;

public partial class InspectorPropertyEditor
{
	[Parameter, EditorRequired] public InspectorPropertyDescriptor Descriptor { get; set; } = null!;

	private bool IsInteger => Descriptor.Kind == InspectorPropertyKind.Integer;
	private bool IsGuid => Descriptor.Kind == InspectorPropertyKind.Guid;
	private bool IsRotation => Descriptor.Kind == InspectorPropertyKind.Rotation;
	private bool IsSupported => IsInteger || IsGuid || IsRotation;
	private string KindClass => IsRotation ? "is-rotation" : string.Empty;

	private int IntValue => Descriptor.Property.GetValue<int>();
	private string IntText => IntValue.ToString( CultureInfo.InvariantCulture );

	private Guid GuidValue => Descriptor.Property.GetValue<Guid>();
	private string GuidText => GuidValue.ToString();

	private Vector3 RotationAngles
	{
		get
		{
			var angles = Descriptor.Property.GetValue<Rotation>().Angles();
			return new Vector3( angles.pitch, angles.yaw, angles.roll );
		}
	}

	private void SetIntText( string text )
	{
		if ( !int.TryParse( text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value ) )
			return;

		SetValue( value );
	}

	private void SetGuidText( string text )
	{
		if ( !Guid.TryParse( text, out var value ) )
			return;

		SetValue( value );
	}

	private void SetRotationPitch( float value ) => SetRotationAngles( new Vector3( value, RotationAngles.y, RotationAngles.z ) );
	private void SetRotationYaw( float value ) => SetRotationAngles( new Vector3( RotationAngles.x, value, RotationAngles.z ) );
	private void SetRotationRoll( float value ) => SetRotationAngles( new Vector3( RotationAngles.x, RotationAngles.y, value ) );

	private void SetRotationAngles( Vector3 value )
	{
		SetValue<Rotation>( new Angles( value.x, value.y, value.z ) );
	}

	private void SetValue<T>( T value )
	{
		Descriptor.Property.SetValue( value );
		Descriptor.Property.Parent?.NoteChanged( Descriptor.Property );
	}
}
