using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkStationScript : MonoBehaviour
{
    public WorkStation WrkS = new WorkStation();

    GameObject player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject;
            player.GetComponent<PlayerEntity>().currentWorkStation = gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.GetComponent<PlayerEntity>().currentWorkStation = null;
            player = null;
        }
    }

}
