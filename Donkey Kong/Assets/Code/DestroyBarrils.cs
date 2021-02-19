using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBarrils : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Destructive"))
            Destroy(collision.gameObject);
    }
}
