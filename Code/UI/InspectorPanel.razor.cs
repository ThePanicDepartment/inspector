using System;

namespace Inspector.UI;

public partial class InspectorPanel
{
	private InspectorService? _service;

	[Property]
	public bool OpenOnStart { get; set; } = true;

	public InspectorService? Service
	{
		get => _service;
		private set => _service = value;
	}

	protected InspectorService? CurrentService => EnsureService();

	protected override void OnAwake()
	{
		base.OnAwake();
		EnsureService();
	}

	protected override void OnStart()
	{
		base.OnStart();

		if ( OpenOnStart )
			EnsureService()?.SetOpen( true );
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

	private InspectorService? EnsureService()
	{
		if ( _service.IsValid() )
			return _service;

		_service = GameObject.Components.Get<InspectorService>( true ) ?? GameObject.AddComponent<InspectorService>();
		return _service;
	}
}
