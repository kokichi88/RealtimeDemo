using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputSystem : AbstractSystem {
	public int ownerId;

	protected override void OnAddedToWorld (World world)
	{
		ownerId = world.ownerId;
	}

	public override void DoFixedUpdate (float dt)
	{
	}

	public override void ProcessMessage (World.Message message)
	{

	}

	public override void DoUpdate (float dt)
	{
		Vector3 dir = Vector3.zero;
		if(Input.GetKeyDown(KeyCode.W))
		{
			dir.y += 1;
		}

		if(Input.GetKeyDown(KeyCode.S))
		{
			dir.y -= 1;
		}

		if(Input.GetKeyDown(KeyCode.A))
		{
			dir.x -= 1;
		}

		if(Input.GetKeyDown(KeyCode.D))
		{
			dir.x += 1;
		}

		if(dir != Vector3.zero)
		{
			List<MoveComponent.MoveInput> inputs = new List<MoveComponent.MoveInput>();
			MoveComponent.MoveInput input = new MoveComponent.MoveInput();
			input.dir = dir;
			input.id = ownerId;
			inputs.Add(input);

			switch(world.mode)
			{
			case World.Mode.OFF_LINE:
				World.Message msg = new MessageList.InputMessage(inputs);
				msg.activeFrame = this.world.currentFrame + 1;
				msg.content = inputs;
				world.AddMessage(msg);
				break;
			case World.Mode.ON_LINE:
				World.Message msg2 = new MessageList.SendInputMessage(inputs);
				msg2.activeFrame = this.world.currentFrame + 1;
				msg2.content = inputs;
				world.AddMessage(msg2);
				break;
			}
		}

	}
	



}
