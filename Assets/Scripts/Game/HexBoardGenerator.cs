using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// Generate a hexagonal grid in i j k coordinates
public class HexBoardGenerator : MonoBehaviourPun
{
    public int sides = 4;

    public int radius = 4;
    public float gap = 0.1f;
    [Space]
    public GameObject hexPrefab;
    public bool flatTopped = false;

    private List<GameObject> hexes;

    private float hexWidth;
    private float hexHeight;

    private Vector2 unitVectorI;
    private Vector2 unitVectorJ;
    private Vector2 unitVectorK;
    // Start is called before the first frame update
    void Start()
    {
        if (sides < 3)
        {
            sides = 3;
        }

        if (sides == 3)
        {
            // triangle
            // Calculate the amount of hexes in the grid
            Debug.LogError("Triangle hexagons not implemented yet");
        }

        if (flatTopped)
        {
            Debug.LogError("Flat topped hexagons not implemented yet");
        }

        SpriteRenderer sr = hexPrefab.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogError("No SpriteRenderer found on hexPrefab");
            return;
        }

        hexWidth = sr.bounds.size.x;
        hexHeight = sr.bounds.size.y;

        unitVectorI = new Vector2(1.5f * hexWidth + gap, 0);
        unitVectorJ = new Vector2(0.75f * hexWidth + gap, 1.5f * hexHeight + gap);
        unitVectorK = new Vector2(-0.75f * hexWidth - gap, 1.5f * hexHeight + gap);

        if (sides == 4)
        {
            // square
            for (int i = -radius; i < radius; ++i)
            {
                for (int j = -radius; j < radius; ++j)
                {
                    for (int k = -radius; k < radius; ++j)
                    {
                        GameObject hex = PhotonNetwork.Instantiate(hexPrefab.name, transform.position, transform.rotation);

                        //Positioning the 2D hexagon tile

                        Vector2 newPos = unitVectorI * i + unitVectorJ * j + unitVectorK * k;

                        hex = PhotonNetwork.Instantiate(hexPrefab.name, newPos, Quaternion.identity);

                        //hex.transform.position = new Vector3(newPos.x, newPos.y, 0);

                        hex.GetComponent<HexagonTile>().i = i;
                        hex.GetComponent<HexagonTile>().j = j;
                        hex.GetComponent<HexagonTile>().k = k;
                        hex.GetComponent<HexagonTile>().hexBoard = this;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
