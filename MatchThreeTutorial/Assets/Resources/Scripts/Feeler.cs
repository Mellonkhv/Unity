using UnityEngine;
using System.Collections;

public class Feeler : MonoBehaviour
{
    public Gem Owner;

    void OnTriggerEnter(Collider c)
    {
        if(c.tag == "Gem")
            Owner.AddNeighbor(c.GetComponent<Gem>());
    }

    void OnTriggerExit(Collider c)
    {
        if (c.tag == "Gem")
            Owner.RemoveNeighbor(c.GetComponent<Gem>());
    }
}
