using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gem : MonoBehaviour 
{
    public GameObject sphere;
    public GameObject selector;

    string[] gemMats = { "Red", "Blue", "Green", "Orange", "Yellow", "Black", "Purple" };
    string color = "";

    public List<Gem> Neighbors = new List<Gem>();
    
    public bool isSelected = false;

	// Use this for initialization
	void Start () 
    {
        CreateGem();
	}
	
	// Update is called once per frame
	void Update () 
    {

    }

    public void ToggleSelector()
    {
        isSelected = !isSelected;
        selector.SetActive(isSelected);
    }

    public void CreateGem()
    {
        color = gemMats[Random.Range(0, gemMats.Length)];
        Material m = Resources.Load("Materials/" + color) as Material;
        sphere.GetComponent<Renderer>().material = m;
    }

    public void AddNeighbor(Gem g)
    {
        if(!Neighbors.Contains(g))
            Neighbors.Add(g);
    }

    public void RemoveNeighbor(Gem g)
    {
        Neighbors.Remove(g);
    }

    void OnMouseDown()
    {
        ToggleSelector();
    }
}
