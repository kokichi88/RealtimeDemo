﻿using UnityEngine;
using System.Collections;

public class AuthoritativeServerMain : MonoBehaviour {
	public GameObject client_prefab;
	public float clientFrameTime = 0.02f;

	public GameObject server_prefab;
	public float serverFrameTime = 0.1f;

	protected World serverWorld;

	protected World clientWorld;

	void Start () {
		World2WorldPipeline connector = gameObject.GetComponent<World2WorldPipeline>();


		GameObject server = GameObject.Instantiate(server_prefab) as GameObject;
		serverWorld = server.GetComponent<World> ();
		serverWorld.AddSystem(new MoveSystem());
		serverWorld.AddSystem(new NetworkServerSystem(connector));

		serverWorld.AddPlayerToWorld();
		connector.SetServer(serverWorld);

		GameObject client = GameObject.Instantiate(client_prefab) as GameObject;
		clientWorld = client.GetComponent<World> ();
		clientWorld.AddSystem(new MoveSystem());
		clientWorld.AddSystem(new InputSystem());
		clientWorld.AddSystem(new NetworkClientSystem(connector));
		clientWorld.AddSystem(new ClientViewSystem());
		clientWorld.AddPlayerToWorld();
		connector.AddClients(clientWorld);

	}
	
	// Update is called once per frame
	void Update () {
		serverWorld.frameTime = serverFrameTime;
		clientWorld.frameTime = clientFrameTime;
	}
}
