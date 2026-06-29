namespace Inspector;

/// <summary>
/// A base class for singletons.
/// </summary>
/// <remarks>
/// Ensures that only one instance of the singleton is created.
/// </remarks>
/// <typeparam name="T">The type of the singleton.</typeparam>
public abstract class SingletonComponent<T> : Component, IHotloadManaged
	where T : SingletonComponent<T>, IHotloadManaged
{
	[SkipHotload]
	private static T? _instance;

	/// <summary>
	/// Gets the active instance of the singleton on the local machine.
	/// </summary>
	[SkipHotload]
	public static T Instance
	{
		get
		{
			if ( _instance.IsValid() && !_instance.IsProxy ) return _instance;
			
			var local = Game.ActiveScene?.GetAllComponents<T>().FirstOrDefault( x => !x.IsProxy );
			
			if ( local.IsValid() ) 
				return _instance = local;

			if ( _instance.IsValid() ) 
				return _instance;

			return _instance = Game.ActiveScene?.GetAllComponents<T>().FirstOrDefault()!;
		}
	}

	protected override void OnAwake()
	{
		if ( IsProxy ) return;
		if ( !Active ) return;

		_instance = (T)this;
	}

	void IHotloadManaged.Destroyed( Dictionary<string, object> state )
	{
		state["IsActive"] = _instance == this;
	}

	void IHotloadManaged.Created( IReadOnlyDictionary<string, object> state )
	{
		if ( state.GetValueOrDefault( "IsActive" ) is true )
			_instance = (T)this;
	}

	protected override void OnDestroy()
	{
		if ( _instance == this )
			_instance = null!;
	}
}
