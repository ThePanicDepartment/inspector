using Inspector.Hierarchy;
using Inspector.Network;
using Inspector.Properties;
using Inspector.Routing;
using Inspector.Selection;
using Inspector.Watch;

namespace Inspector;

/// <summary>
/// Runtime owner for the in-game inspector state.
/// </summary>
public sealed partial class InspectorService : SingletonComponent<InspectorService>
{
	private readonly InspectorNetworkProvider _networkProvider = new();
	private float _nextWatchSampleTime;

	[Property, InputAction]
	public string ToggleAction { get; set; } = InspectorInput.ToggleAction;

	public InspectorRouter Router { get; } = new();
	public InspectorSelectionHistory RecentSelections { get; } = new();
	public InspectorPropertyDescriptorProvider PropertyDescriptors { get; } = new();
	public InspectorHierarchyProvider Hierarchy { get; } = new();
	public List<InspectorWatchMetric> WatchMetrics { get; } = [];

	public bool IsOpen { get; private set; }
	public bool IsPinned { get; private set; }
	public string SearchText { get; private set; } = string.Empty;
	public GameObject? SelectedGameObject { get; private set; }
	public InspectorGizmoMode GizmoMode { get; private set; } = InspectorGizmoMode.Move;
	public InspectorNetworkSnapshot NetworkSnapshot { get; private set; } = InspectorNetworkSnapshot.Empty;

	public InspectorRoute ActiveRoute => Router.ActiveRoute;

	public void ToggleOpen() => SetOpen( !IsOpen );

	public void SetOpen( bool open )
	{
		if ( IsOpen == open )
			return;

		IsOpen = open;
	}

	public void TogglePinned() => IsPinned = !IsPinned;

	public void Navigate( InspectorRoute route ) => Router.Navigate( route );

	public void SetSearchText( string? value ) => SearchText = value ?? string.Empty;

	public void Select( GameObject? gameObject )
	{
		if ( SelectedGameObject == gameObject )
			return;

		SelectedGameObject = gameObject;
		RecentSelections.Push( gameObject );
		PropertyDescriptors.Clear();
		EnsureDefaultWatchMetrics();
	}

	public void SetGizmoMode( InspectorGizmoMode mode ) => GizmoMode = mode;

	public IReadOnlyList<InspectorPropertyDescriptor> GetComponentDescriptors( Component component )
	{
		return PropertyDescriptors.GetDescriptors( component );
	}

	public InspectorNetworkSnapshot RefreshNetworkSnapshot()
	{
		return NetworkSnapshot = _networkProvider.ReadSnapshot();
	}

	protected override void OnStart()
	{
		base.OnStart();
		SelectFirstSceneObject();
		EnsureDefaultWatchMetrics();
	}

	protected override void OnUpdate()
	{
		if ( !string.IsNullOrWhiteSpace( ToggleAction ) && Input.Pressed( ToggleAction ) )
			ToggleOpen();

		RecentSelections.PruneInvalid();
		RefreshNetworkSnapshot();

		if ( Time.Now >= _nextWatchSampleTime )
		{
			_nextWatchSampleTime = Time.Now + 0.1f;
			SampleWatchMetrics();
		}
	}

	private void SelectFirstSceneObject()
	{
		if ( SelectedGameObject.IsValid() )
			return;

		var first = Game.ActiveScene?.Children.FirstOrDefault( x => x.IsValid() );
		Select( first );
	}

	private void EnsureDefaultWatchMetrics()
	{
		WatchMetrics.Clear();

		var selected = SelectedGameObject;
		if ( !selected.IsValid() )
			return;

		WatchMetrics.AddRange(
		[
			new()
			{
				Group = "Transform",
				Name = "Position X",
				Unit = "u",
				Accent = new Color( 0.31f, 0.55f, 1f ),
				ReadValue = () => selected.IsValid() ? selected.WorldPosition.x : 0f
			},
			new()
			{
				Group = "Transform",
				Name = "Position Y",
				Unit = "u",
				Accent = new Color( 0.39f, 0.8f, 0.45f ),
				ReadValue = () => selected.IsValid() ? selected.WorldPosition.y : 0f
			},
			new()
			{
				Group = "Transform",
				Name = "Position Z",
				Unit = "u",
				Accent = new Color( 0.58f, 0.43f, 0.94f ),
				ReadValue = () => selected.IsValid() ? selected.WorldPosition.z : 0f
			},
			new()
			{
				Group = "Network",
				Name = "KB In",
				Unit = "KB/s",
				Accent = new Color( 0.39f, 0.8f, 0.45f ),
				ReadValue = () => NetworkSnapshot.BytesInPerSecond / 1024f
			},
			new()
			{
				Group = "Network",
				Name = "KB Out",
				Unit = "KB/s",
				Accent = new Color( 0.58f, 0.43f, 0.94f ),
				ReadValue = () => NetworkSnapshot.BytesOutPerSecond / 1024f
			}
		] );
	}

	private void SampleWatchMetrics()
	{
		foreach ( var metric in WatchMetrics )
		{
			metric.Sample();
		}
	}
}
