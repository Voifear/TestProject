using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerClimb : MonoBehaviour
{
    Player CharControl;
    BoxCollider2D box;
    public LayerMask Detected;

    void Start()
    {
        CharControl = GetComponentInParent<Player>();
        box = GetComponents<BoxCollider2D>()[0];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((Detected.value & 1 << collision.gameObject.layer) !=0) { box.enabled = false; }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((Detected.value & 1 << collision.gameObject.layer) != 0) { box.enabled = true; }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((Detected.value & 1 << collision.gameObject.layer) != 0 && CharControl.rb.velocity.y <= 0) { CharControl.StartAnimLadge(); }
    }

}
