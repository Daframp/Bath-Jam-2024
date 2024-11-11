using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WithinBoardCheck : MonoBehaviour
{
    public GameObject Player;
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().FellOffBoard();
        }
    }
}
