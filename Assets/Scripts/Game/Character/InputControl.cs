using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControl : MonoBehaviour
{
    public PlayerCore playerCore = null;
    public PlayerStat playerStat = null;

    public LayerMask whatIsTile;
    void Start()
    {
        playerCore = GetComponent<PlayerCore>();
        playerStat = GetComponent<PlayerStat>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TurnManager.Instance.turnID != playerCore.turnID)
            return;

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, whatIsTile);

            if (hit.collider == null)
            {
                return;
            }

            HexagonTile clickedTile = hit.collider.gameObject.GetComponent<HexagonTile>();

            Debug.Log("Clicked tile: " + clickedTile.name);
            playerCore.Interact(clickedTile);
        }
    }
}