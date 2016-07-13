using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputSystem : AbstractSystem {
	public static float DELAY_INPUT = 0.1f;
	public int ownerId;
	private float count;
	private Vector3 dir = Vector3.zero;
	protected override void OnAddedToWorld (World world)
	{
		ownerId = world.ownerId;
	}

	public override void DoFixedUpdate (float dt)
	{
	}

	public override void ProcessMessage (MessageList.Message message)
	{

	}

	public override void DoUpdate (float dt)
	{
		if(Input.GetKeyUp(KeyCode.W))
		{
			dir.y -= 1;
		}

		if(Input.GetKeyDown(KeyCode.W))
		{
			dir.y += 1;
		}



		if(Input.GetKeyDown(KeyCode.S))
		{
			dir.y -= 1;
		}

		if(Input.GetKeyUp(KeyCode.S))
		{
			dir.y += 1;
		}

		if(Input.GetKeyDown(KeyCode.A))
		{
			dir.x -= 1;
		}

		if(Input.GetKeyUp(KeyCode.A))
		{
			dir.x += 1;
		}

		if(Input.GetKeyUp(KeyCode.D))
		{
			dir.x -= 1;
		}

		if(Input.GetKeyDown(KeyCode.D))
		{
			dir.x += 1;
		}



		count -= dt;
		if(count <= 0)
		{
			count = DELAY_INPUT;
			List<MoveComponent.MoveInput> inputs = new List<MoveComponent.MoveInput>();
			MoveComponent.MoveInput input = new MoveComponent.MoveInput();
			input.dir = new Vector3(dir.x, dir.y, dir.z);
			input.id = ownerId;
			inputs.Add(input);

			switch(world.mode)
			{
			case World.Mode.OFF_LINE:
				MessageList.InputMessage msg = new MessageList.InputMessage(inputs);
				msg.activeFrame = this.world.currentFrame + 1;
				msg.content = inputs;
				world.AddMessage(msg);
				break;
			case World.Mode.ON_LINE:
				MessageList.SendInputMessage msg2 = new MessageList.SendInputMessage(inputs);
				msg2.activeFrame = this.world.currentFrame + 1;
				msg2.content = inputs;
				world.AddMessage(msg2);
				break;
			}
		}

	}
	



}
