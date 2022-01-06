using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Droppod : Building
{
    public EnvironmentData environment;
    public GameObject interior, exterior;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        interior.SetActive(true);
        exterior.SetActive(false);
        Creature creature = collision.GetComponent<Creature>();
        if (creature != null) creature.ChangeEnvironment(environment);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        interior.SetActive(false);
        exterior.SetActive(true);
        Creature creature = collision.GetComponent<Creature>();
        if (creature != null) creature.ChangeEnvironment(null);
    }
}
