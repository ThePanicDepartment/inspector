namespace Inspector.Routing;

/// <summary>
/// Owns inspector view navigation without coupling UI components to route state.
/// </summary>
public sealed class InspectorRouter
{
	private readonly List<InspectorRoute> _history = [];
	private readonly List<InspectorRoute> _forward = [];

	public InspectorRoute ActiveRoute { get; private set; } = InspectorRoute.Inspect;

	public IReadOnlyList<InspectorRoute> History => _history;

	public bool Navigate( InspectorRoute route )
	{
		if ( route == ActiveRoute )
			return false;

		_history.Add( ActiveRoute );
		_forward.Clear();
		ActiveRoute = route;
		return true;
	}

	public bool TryGoBack()
	{
		if ( _history.Count == 0 )
			return false;

		var previous = _history[^1];
		_history.RemoveAt( _history.Count - 1 );
		_forward.Add( ActiveRoute );
		ActiveRoute = previous;
		return true;
	}

	public bool TryGoForward()
	{
		if ( _forward.Count == 0 )
			return false;

		var next = _forward[^1];
		_forward.RemoveAt( _forward.Count - 1 );
		_history.Add( ActiveRoute );
		ActiveRoute = next;
		return true;
	}

	public void Reset( InspectorRoute route = InspectorRoute.Inspect )
	{
		_history.Clear();
		_forward.Clear();
		ActiveRoute = route;
	}
}
