using UnityEngine;

[CreateAssetMenu(menuName = "Game/LevelSpawnData")]
public class LevelSpawnData : ScriptableObject
{
    public float maxXValidPoint = 5;
    
    public float xRandomness = 4;
    public float zRandomness = 5;
}
