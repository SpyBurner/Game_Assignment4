#define DEBUG

using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Linq;
using Unity.VisualScripting;

// Generate a hexagonal grid in i j k coordinates
public class HexBoardGenerator : PhotonSingleton<HexBoardGenerator>
{
    [Header("Grid")]
    public int gridRadius = 3;
    public float hexRadius = 1f;
    public int extraTiles = 2; // Number of extra tiles on each side for each row
    public GameObject hexPrefab;

    [Space]
    [Header("Obstacle")]
    public float obstacleChance = 0.1f;
    public GameObject obstaclePrefab;


    private float hexWidth;
    private float hexHeight;

    private Hashtable hexagons = new Hashtable();

    [SerializeField]
    private List<HexagonTile> spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
#if DEBUG
        PhotonNetwork.OfflineMode = true;
        PhotonNetwork.CreateRoom(null);
#endif
        spawnPoints = new List<HexagonTile>();

        SpriteRenderer selfSR = gameObject.GetComponent<SpriteRenderer>();
        if (selfSR)
        {
            selfSR.enabled = false;
        }

        SpriteRenderer sr = hexPrefab.GetComponent<SpriteRenderer>();
        hexWidth = sr.bounds.size.x;
        hexHeight = sr.bounds.size.y;

        GenerateHexGrid();
        GenerateObstacle();
    }
    private void GenerateHexGrid()
    {

        for (int r = -gridRadius; r <= gridRadius; r++)
        {
            for (int q = -gridRadius - extraTiles; q <= gridRadius + extraTiles; q++)
            {
                int s = -q - r;
                if (Mathf.Abs(s) <= gridRadius + extraTiles)
                {
                    Vector3 hexPosition = HexToPixel(q, r, s) + transform.position;
                    GameObject hex = PhotonNetwork.Instantiate(hexPrefab.name, hexPosition, Quaternion.identity);
                    hex.name = $"Hex_{q}_{r}_{s}";
                    hex.transform.SetParent(this.transform);

                    hexagons.Add((q, r, s), hex);

                    if (r == 0 && Mathf.Abs(q) == gridRadius + extraTiles && s + q == 0)
                    {
                        spawnPoints.Add(hex.GetComponent<HexagonTile>());
                    }

                    hex.GetComponent<HexagonTile>().i = q;
                    hex.GetComponent<HexagonTile>().j = r;
                    hex.GetComponent<HexagonTile>().k = s;

                    hex.GetComponent<HexagonTile>().hexBoard = this;
                }
            }
        }
    }

    private void GenerateObstacle()
    {
        foreach (GameObject hex in hexagons.Values)
        {
            if (spawnPoints.Contains(hex.GetComponent<HexagonTile>()))
            {
                continue;
            }
            if (Random.value < obstacleChance)
            {
                GameObject obstacle = PhotonNetwork.Instantiate(obstaclePrefab.name, hex.transform.position, Quaternion.identity);
                obstacle.transform.SetParent(transform);
                hex.GetComponent<HexagonTile>().SetOnTile(obstacle);
            }
        }
    }
    public Vector3 HexToPixel(int q, int r, int s)
    {
        float x = hexRadius * (Mathf.Sqrt(3) * q + Mathf.Sqrt(3) / 2 * r);
        float y = hexRadius * (3f / 2 * r);
        return new Vector3(x, y, 0);
    }

    public GameObject GetTileAt(int q, int r, int s)
    {
        if (hexagons.ContainsKey((q, r, s)))
            return hexagons[(q, r, s)] as GameObject;
        else
            return null;
    }

    public HexagonTile GetSpawnPoint()
    {
        try
        {
            HexagonTile spawnPoint = spawnPoints[0];
            spawnPoints.RemoveRange(0, 1);

            return spawnPoint;
        }
        catch (System.Exception e)
        {
            Debug.LogError("GetSpawnPoint error: " + e.Message);
            return null;
        }
    }
}
