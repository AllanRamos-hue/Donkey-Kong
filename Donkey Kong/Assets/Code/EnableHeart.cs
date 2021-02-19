using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableHeart : MonoBehaviour
{
    public GameObject heart;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            heart.SetActive(true);
        }
            
    }

}
