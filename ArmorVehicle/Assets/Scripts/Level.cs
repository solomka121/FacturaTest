using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public LevelData levelData;
    
    public Transform startPoint;
    public FinishPoint finishPoint;

    public int enemiesToSpawn = 10;

    public float SpawnZoneLenght => finishPoint.transform.localPosition.z - startPoint.localPosition.z;
}
