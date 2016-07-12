using UnityEngine;
using System.Collections;

public class MoveComponent : MonoBehaviour {
	public float speed;
	public Vector3 dir;
	public float friction;

	public float currSpeed;
	public Vector3 pos;

	public class MoveInput
	{
		public int id;
		public Vector3 dir;
	}
}
