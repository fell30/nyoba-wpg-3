using UnityEngine;

public class RespawnTracker : MonoBehaviour
{
    public RespawnManager manager;
    public int spawnPointIndex;

    private void Start()
    {

    }

    private void OnDestroy()
    {
        if (manager != null)
        {
            manager.NotifyDestroyed(spawnPointIndex);

        }
    }
}
