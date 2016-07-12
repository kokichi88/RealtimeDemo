using UnityEngine;
using System.Collections;

public abstract class AbstractSystem {

	protected World world;

	public void OnAdd(World world)
	{
		this.world = world;
		OnAddedToWorld(world);
	}

	protected virtual void OnAddedToWorld(World world){}
	public abstract void DoFixedUpdate(float dt);
	public abstract void DoUpdate(float dt);
	public abstract void ProcessMessage(World.Message message);
}
