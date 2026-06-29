using System;

namespace Inspector.Selection;

/// <summary>
/// Small fixed-size history of recently inspected objects.
/// </summary>
public sealed class InspectorSelectionHistory
{
	private readonly int _capacity;
	private readonly List<GameObject> _items;

	public InspectorSelectionHistory( int capacity = 8 )
	{
		_capacity = Math.Max( 1, capacity );
		_items = new List<GameObject>( _capacity );
	}

	public IReadOnlyList<GameObject> Items => _items;

	public void Push( GameObject? gameObject )
	{
		if ( !gameObject.IsValid() )
			return;

		_items.Remove( gameObject );
		_items.Insert( 0, gameObject );

		if ( _items.Count > _capacity )
			_items.RemoveRange( _capacity, _items.Count - _capacity );
	}

	public void PruneInvalid()
	{
		for ( var i = _items.Count - 1; i >= 0; i-- )
		{
			if ( !_items[i].IsValid() )
				_items.RemoveAt( i );
		}
	}
}
