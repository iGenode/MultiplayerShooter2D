using UnityEngine;

public static class Utils
{

    public static Vector3 GetRandomSpawnPoint()
    {
        return new(Random.Range(-15, 15), Random.Range(-9, 9), 0);
    }
}
