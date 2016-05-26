using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using aStar;

public class AstarTest : MonoBehaviour
{

    private AStarPathFind aStar;

    public void Start()
    {
        byte[,] bytes = new byte[3, 3];
        bytes[0, 0] = (byte)TerrainType.NoWay;
        bytes[0, 1] = (byte)TerrainType.Normal;
        bytes[0, 2] = (byte)TerrainType.Normal;

        bytes[1, 0] = (byte)TerrainType.NoWay;
        bytes[1, 1] = (byte)TerrainType.Normal;
        bytes[1, 2] = (byte)TerrainType.NoWay;

        bytes[2, 0] = (byte)TerrainType.Normal;
        bytes[2, 1] = (byte)TerrainType.Normal;
        bytes[2, 2] = (byte)TerrainType.NoWay;

        this.aStar = new AStarPathFind(3, 3, bytes);

        this.StartCoroutine(waitShowResult());
    }


    private IEnumerator waitShowResult()
    {
        yield return new WaitForSeconds(1.0f);
        List<AStarPosition> result = this.aStar.findPath(2, 0, 0, 2);
        Debug.Log(result);
    }
}
