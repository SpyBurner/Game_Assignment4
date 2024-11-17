#define DEBUG

using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// Generate a hexagonal grid in i j k coordinates
public class HexBoardGenerator : MonoBehaviourPun
{
    public int gridRadius = 3;
    public float hexRadius = 1f;
    public GameObject hexPrefab;

    private float hexWidth;
    private float hexHeight;

    private Hashtable hexagons = new Hashtable();
    // Start is called before the first frame update
    void Start()
    {
#if DEBUG
        PhotonNetwork.OfflineMode = true;
        PhotonNetwork.CreateRoom(null);
#endif



        SpriteRenderer sr = hexPrefab.GetComponent<SpriteRenderer>();
        hexWidth = sr.bounds.size.x;
        hexHeight = sr.bounds.size.y;

        GenerateHexGrid();
    }

    private void GenerateHexGrid()
    {
        int extraTiles = 2; // Number of extra tiles on each side for each row

        for (int r = -gridRadius; r <= gridRadius; r++)
        {
            for (int q = -gridRadius - extraTiles; q <= gridRadius + extraTiles; q++)
            {
                int s = -q - r;
                if (Mathf.Abs(s) <= gridRadius + extraTiles)
                {
                    Vector3 hexPosition = HexToPixel(q, r, s);
                    GameObject hex = PhotonNetwork.Instantiate(hexPrefab.name, hexPosition, Quaternion.identity);
                    hex.name = $"Hex_{q}_{r}_{s}";
                    hex.transform.SetParent(this.transform);

                    hexagons.Add((q, r, s), hex);
                }
            }
        }
    }

    private Vector3 HexToPixel(int q, int r, int s)
    {
        float x = hexRadius * (Mathf.Sqrt(3) * q + Mathf.Sqrt(3) / 2 * r);
        float y = hexRadius * (3f / 2 * r);
        return new Vector3(x, y, 0);
    }

    GameObject GetTileAt(int q, int r, int s)
    {
        return hexagons[(q, r, s)] as GameObject;
    }
}
