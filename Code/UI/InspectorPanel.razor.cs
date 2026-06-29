using System;

namespace Inspector.UI;

public partial class InspectorPanel
{
	public InspectorService? Service { get; private set; }

	protected override void OnAwake()
	{
		base.OnAwake();
		Service = GameObject.Components.Get<InspectorService>( true ) ?? GameObject.AddComponent<InspectorService>();
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		SetClass( "is-open", Service?.IsOpen == true );
		SetClass( "is-closed", Service?.IsOpen != true );
	}

	protected override int BuildHash()
	{
		return HashCode.Combine(
			Service?.IsOpen,
			Service?.ActiveRoute,
			Service?.SelectedGameObject?.Id,
			Service?.GizmoMode,
			Service?.SearchText,
			Time.Now.CeilToInt() );
	}
}
