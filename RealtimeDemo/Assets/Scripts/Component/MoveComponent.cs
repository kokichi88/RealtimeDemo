using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveComponent : MonoBehaviour {
	public float speed;
	public Vector3 dir;
	public float friction;
	public List<int> lastProcessedInputs = new List<int>();
	public List<MoveComponent.MoveInput> queueInputs = new List<MoveComponent.MoveInput>();
	public float currSpeed;
	public Vector3 pos;

	public class MoveInput
	{
		public int actorId;
		public int inputId;
		public int activeFrame;
		public Vector3 dir;
	}
}
