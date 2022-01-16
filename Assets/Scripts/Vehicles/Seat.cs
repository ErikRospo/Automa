using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seat : MonoBehaviour
{
    private Transform sitter = null;
    private bool sitting = false;
    public Vehicle vehicle;
    public bool isDriver = false;

    private void Update()
    {
        if (!sitting) return;
        sitter.transform.position = transform.position;
        if (isDriver && sitter != null)
        {
            Vector2 inputVector = Vector2.zero;

            inputVector.x = Input.GetAxis("Horizontal");
            inputVector.y = Input.GetAxis("Vertical");

            vehicle.SetInputVector(inputVector);
        }
    }

    public void UpdateSitterPos(Vector2 velocity)
    {
        if (!sitting) return;
        sitter.position = transform.position;
        sitter.GetComponent<Rigidbody2D>().velocity = velocity;
    }
 
    public void ToggleSit()
    {
        if (sitter.parent != null)
        {
            sitter.SetParent(null);
            sitter.GetComponent<MovementController>().isSeated = false;
            sitter.GetComponent<Rigidbody2D>().simulated = true;
            sitting = false;
        }
        else
        {
            sitter.SetParent(transform);
            sitter.transform.position = transform.position;
            sitter.GetComponent<MovementController>().isSeated = true;
            sitter.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            sitting = true;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (sitter == null)
        {
            sitter = collision.transform;
            InputEvents.active.onInteractPressed += ToggleSit;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (sitter == collision.transform)
        {
            sitter.SetParent(null);
            sitter.GetComponent<MovementController>().isSeated = false;
            sitter = null;
            InputEvents.active.onInteractPressed -= ToggleSit;
            sitting = false;
        }
    }
}
