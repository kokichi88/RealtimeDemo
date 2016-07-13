using UnityEngine;
using System.Collections;

public class AuthoritativeServerClientPredictionMain : AuthoritativeServerMain {

	void Start () {
		World2WorldPipeline connector = gameObject.GetComponent<World2WorldPipeline>();


		GameObject server = GameObject.Instantiate(server_prefab) as GameObject;
		serverWorld = server.GetComponent<World>();
		serverWorld.AddSystem(new MoveSystem());
		serverWorld.AddSystem(new NetworkServerSystem(connector, 1f/updateStateTimesPerSecond));

		serverWorld.AddPlayerToWorld();
		connector.SetServer(serverWorld);

		GameObject client = GameObject.Instantiate(client_prefab) as GameObject;
		clientWorld = client.GetComponent<World>();
		clientWorld.AddSystem(new PredictionSystem());
		clientWorld.AddSystem(new InputSystem());
		clientWorld.AddSystem(new NetworkClientSystem(connector));
		clientWorld.AddSystem(new ClientViewSystem());
		clientWorld.AddPlayerToWorld();
		connector.AddClients(clientWorld);

	}
	
}
