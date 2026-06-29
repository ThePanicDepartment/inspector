using System.Globalization;

namespace Inspector.UI;

public partial class InspectorNumberInput
{
	[Parameter] public float Value { get; set; }
	[Parameter] public Action<float>? ValueChanged { get; set; }
	[Parameter] public string Format { get; set; } = "0.###";

	private string FormattedValue => Value.ToString( Format, CultureInfo.InvariantCulture );

	private void Commit( string text )
	{
		if ( !float.TryParse( text, NumberStyles.Float, CultureInfo.InvariantCulture, out var value ) )
			return;

		ValueChanged?.Invoke( value );
	}
}
