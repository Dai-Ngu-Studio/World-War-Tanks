using Mirror;
using UnityEngine;

public class ResourceGenerator : NetworkBehaviour
{
    [SerializeField]
    private Health health;

    [SerializeField]
    private int resourcesPerInterval = 10;

    [SerializeField]
    private float interval = 2f;

    private float timer;

    private RTSPlayer player;

    public override void OnStartServer()
    {
        timer = interval;

        player = connectionToClient.identity.GetComponent<RTSPlayer>();

        health.ServerOnDie += ServerHandleDie;
        GameOverHandler.ServerOnGameOver += ServerHandlerGameOver;
    }

    public override void OnStopServer()
    {
        health.ServerOnDie -= ServerHandleDie;
        GameOverHandler.ServerOnGameOver -= ServerHandlerGameOver;
    }

    [ServerCallback]
    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            player.SetResources(player.GetResources() + resourcesPerInterval);

            timer += interval;
        }
    }

    private void ServerHandleDie()
    {
        NetworkServer.Destroy(gameObject);
    }

    private void ServerHandlerGameOver()
    {
        enabled = false;
    }
}
