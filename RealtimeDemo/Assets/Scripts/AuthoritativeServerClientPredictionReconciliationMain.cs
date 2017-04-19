using UnityEngine;
using System.Collections;

public class AuthoritativeServerClientPredictionReconciliationMain : AuthoritativeServerMain {
	public GameObject client_prefab2;
	public GameObject actor_prefab2;

	World clientWorld2;

	void Start () {
		World2WorldPipeline connector = gameObject.GetComponent<World2WorldPipeline>();

		GameObject server = GameObject.Instantiate(server_prefab) as GameObject;
		serverWorld = server.GetComponent<World>();
		serverWorld.AddSystem(new MoveSystem());
		serverWorld.AddSystem(new NetworkServerSystem(connector, 1f/updateStateTimesPerSecond));

		serverWorld.AddPlayerToWorld();
		serverWorld.AddPlayerToWorld(actor_prefab2);
		connector.SetServer(serverWorld);

		GameObject client1 = GameObject.Instantiate(client_prefab) as GameObject;
		clientWorld = client1.GetComponent<World>();
		clientWorld.AddSystem(new PredictionSystem());
		clientWorld.AddSystem(new InputSystem());
		clientWorld.AddSystem(new NetworkClientSystem(connector));
		clientWorld.AddSystem(new ClientInterpolationSystem());
		clientWorld.AddSystem(new ReconciliationSystem());
		clientWorld.AddPlayerToWorld();
		clientWorld.AddPlayerToWorld(actor_prefab2);

		GameObject client2 = GameObject.Instantiate(client_prefab2) as GameObject;
		clientWorld2 = client2.GetComponent<World> ();
		clientWorld2.AddSystem(new PredictionSystem());
		clientWorld2.AddSystem(new InputSystem());
		clientWorld2.AddSystem(new NetworkClientSystem(connector));
		clientWorld2.AddSystem(new ClientInterpolationSystem());
		clientWorld2.AddSystem(new ReconciliationSystem());
		clientWorld2.AddPlayerToWorld();
		clientWorld2.AddPlayerToWorld(actor_prefab2);

		connector.AddClients(clientWorld);
		connector.AddClients(clientWorld2);

	}

	void Update()
	{
		serverWorld.frameTime = frameTimeServer;
		clientWorld.frameTime = frameTimeClient;
		clientWorld2.frameTime = frameTimeClient;
		
		if(updateStateTimesPerSecond > 0)
		{
			MessageList.Message msg = new MessageList.ChangeServerUpdateTime(1f/ updateStateTimesPerSecond);
			msg.activeFrame = serverWorld.currentFrame + 1;
			serverWorld.DispatchMessage(msg);
		}

		ClientInterpolationSystem.MAX_STEP = interpolation_step;
	}
	
}
