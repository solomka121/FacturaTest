using UnityEngine;

public class Level : MonoBehaviour
{
    public LevelSpawnData levelSpawnData;

    public float enemiesHealth = 10;
    
    public Transform startPoint;
    public FinishPoint finishPoint;

    public int enemiesToSpawn = 10;

    public float SpawnZoneLenght => finishPoint.transform.localPosition.z - startPoint.localPosition.z;
}
