using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class PlayerCore : MonoBehaviour
{
    public GameObject UIPrefab = null;

    private HexBoardGenerator board = null;
    public HexagonTile currentTile { get; private set; } = null;

    public int turnID { get; private set; } = -1;
    private TurnManager turnManager = null;

    public UnityEvent OnTurnStart;
    public UnityEvent OnTurnEnd;

    private PlayerStat playerStat = null;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("InitPlayer", 0.5f);
    }

    void InitPlayer()
    {
            playerStat = GetComponent<PlayerStat>();
            board = FindAnyObjectByType<HexBoardGenerator>();
            turnManager = FindAnyObjectByType<TurnManager>();
        
        if (GetComponent<PhotonView>().IsMine)
        {
            GameObject newUI = Instantiate(UIPrefab);
            newUI.GetComponent<UIUpdater>().player = gameObject;

            currentTile = board.GetSpawnPoint();
            currentTile.SetOnTile(gameObject);

            turnID = turnManager.AddPlayer(gameObject);
            Debug.Log("Added player to turn manager" + gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!turnManager)
        {
            turnManager = FindAnyObjectByType<TurnManager>();
            if (!turnManager)
            {
                return;
            }
        }

        if (!board)
        {
            board = FindAnyObjectByType<HexBoardGenerator>();
            if (!board)
            {
                return;
            }
        }

        if (!GetComponent<PhotonView>().IsMine)
            return;
        //Set first tile
        if (currentTile == null || turnID == -1)
        {
            if (currentTile == null)
                Debug.Log("[" + gameObject.name + "] Current tile is null");
            if (turnID == -1)
                Debug.Log("[" + gameObject.name + "] Turn ID is -1");
            return;
        }

        transform.position = currentTile.transform.position;
    }

    public void StartTurn()
    {
        PlayerStat playerStat = GetComponent<PlayerStat>();
        playerStat.StartTurnReset();
        OnTurnStart.Invoke();
    }

    public void EndTurn()
    {
        OnTurnEnd.Invoke();
    }

    public void Interact(HexagonTile otherTile)
    {
        if (otherTile == null)
            return;
        //Move
        if (otherTile.onThisTile == null)
        {
            if (currentTile.IsNeighbour(otherTile))
            {
                if (playerStat.UseMana(1))
                {
                    currentTile.SetOnTile(null);
                    currentTile = otherTile;
                    currentTile.SetOnTile(gameObject);
                }
            }
        }
        else
        {
            //Attack
            PlayerStat otherPlayerStat = otherTile.onThisTile.GetComponent<PlayerStat>();

            if (otherPlayerStat != null)
            {
                if (currentTile.IsNeighbour(otherTile) && !currentTile.IsSameTile(otherTile))
                {
                    if (playerStat.UseMana(0))
                        otherPlayerStat.TakeDamage(1);
                }
            }
        }
    }
}
