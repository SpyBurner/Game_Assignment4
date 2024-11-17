using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonTile : MonoBehaviour
{
    public int i;
    public int j;
    public int k;

    public HexBoardGenerator hexBoard;

    public GameObject onThisTile { get; private set; } = null;

    private Queue<int> tileDamage = new Queue<int>();

    // Start is called before the first frame update
    void Start()
    {
        TurnManager.Instance.OnAdvanceTurn.AddListener(OnTurnStart);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetOnTile(GameObject _gameObject)
    {
        onThisTile = _gameObject;
    }

    public bool IsNeighbour(HexagonTile other)
    {
        return (Mathf.Abs(i - other.i) <= 1) && (Mathf.Abs(j - other.j) <= 1) && (Mathf.Abs(k - other.k) <= 1);
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
        }
    }

    public void OnTurnEnd()
    {

    }

}
