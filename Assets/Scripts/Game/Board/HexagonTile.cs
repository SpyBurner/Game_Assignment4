using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HexagonTile : MonoBehaviourPunCallbacks, IPunObservable
{
    public int i;
    public int j;
    public int k;

    public HexBoardGenerator hexBoard;

    public GameObject onThisTile = null;

    private Queue<int> tileDamage = new Queue<int>();

    private TurnManager turnManager;

    private bool isDirty = false;
    private Color currentColor = Color.white;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!turnManager)
        {
            turnManager = FindAnyObjectByType<TurnManager>();
            turnManager.OnAdvanceTurn.AddListener(OnTurnStart);
            if (!turnManager)
            {
                return;
            }
        }

        if (!hexBoard)
        {
            hexBoard = FindAnyObjectByType<HexBoardGenerator>();
            if (!hexBoard)
            {
                return;
            }
        }

        bool nearCurrentPlayer = IsNearCurrentPlayer();
        UpdateTileColor(nearCurrentPlayer);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Obstacle"))
        {
            SetOnTile(collision.gameObject);
        }
    }

    private bool IsNearCurrentPlayer()
    {
        foreach (HexagonTile tile in GetNeighbours(this))
        {
            if (tile.onThisTile != null && tile.onThisTile.CompareTag("Player"))
            {
                PlayerCore playerCore = tile.onThisTile.GetComponent<PlayerCore>();
                PlayerStat playerStat = tile.onThisTile.GetComponent<PlayerStat>();

                if (playerCore != null && playerStat != null &&
                    playerCore.turnID == turnManager.turnID &&
                    playerStat.movePoint > 0)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void UpdateTileColor(bool nearCurrentPlayer)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (nearCurrentPlayer)
        {
            if (onThisTile == null)
            {
                spriteRenderer.color = Color.green;
            }
            else if (onThisTile.CompareTag("Player"))
            {
                spriteRenderer.color = Color.red;
            }
        }
        else
        {
            spriteRenderer.color = Color.white;
        }

        if (currentColor != spriteRenderer.color)
        {
            isDirty = true;
            currentColor = spriteRenderer.color;
        }
    }

    public void SetOnTile(GameObject _gameObject)
    {
        onThisTile = _gameObject;
        isDirty = true;
    }

    public bool IsNeighbour(HexagonTile other)
    {
        return (Mathf.Abs(i - other.i) <= 1) && (Mathf.Abs(j - other.j) <= 1) && (Mathf.Abs(k - other.k) <= 1);
    }

    public List<HexagonTile> GetNeighbours(HexagonTile tile)
    {
        //Get all 6 neighbours
        List<HexagonTile> result = new List<HexagonTile>();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                for (int k = -1; k <= 1; k++)
                {
                    if (i == 0 && j == 0 && k == 0)
                    {
                        continue;
                    }
                    if (i + j + k == 0)
                    {
                        GameObject hexObject = hexBoard.GetTileAt(this.i + i, this.j + j, this.k + k);
                        if (hexObject != null)
                        {
                            result.Add(hexObject.GetComponent<HexagonTile>());
                        }
                    }
                }
            }
        }
        return result;
    }

    public bool IsSameTile(HexagonTile other)
    {
        return i == other.i && j == other.j && k == other.k;
    }

    //Manhattan distance
    public int DistanceTo(HexagonTile other)
    {
        return (Mathf.Abs(i - other.i) + Mathf.Abs(j - other.j) + Mathf.Abs(k - other.k)) / 2;
    }

    public void OnTurnStart()
    {
        this.GetComponent<SpriteRenderer>().color = Color.white;
        if (tileDamage.Count > 0)
        {
            int damage = tileDamage.Dequeue();
            if (onThisTile != null)
            {
                PlayerStat playerStat = onThisTile.GetComponent<PlayerStat>();
                if (playerStat != null)
                {
                    playerStat.TakeDamage(damage);
                }
            }
            isDirty = true;
        }
    }

    public void OnTurnEnd()
    {
        this.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (!isDirty)
            {
                return;
            }
            object[] objects = new object[2];
            objects[0] = currentColor;
            objects[1] = tileDamage;

            stream.SendNext(objects);
            isDirty = false;
        }
        else if (stream.IsReading)
        {
            object[] objects = (object[])stream.ReceiveNext();

            currentColor = (Color)objects[0];
            tileDamage = (Queue<int>)objects[1];
        }
    }
}
