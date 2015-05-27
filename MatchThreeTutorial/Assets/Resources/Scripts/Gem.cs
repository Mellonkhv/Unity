using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

public class Gem : MonoBehaviour
{
    public GameObject Sphere;
    public GameObject Selector;
    private string[] _gemMats = {"Blue", "Green", "Grey", "Purple", "Red", "Yellow"};
    public string _color = "";
    public List<Gem> Neighbors = new List<Gem>();
    public bool IsSelected = false;
    public bool isMatched = false;

    public int Xcoord
    {
        get { return Mathf.RoundToInt(transform.localPosition.x); }
    }

    public int Ycoord
    {
        get { return Mathf.RoundToInt(transform.localPosition.y); }
    }

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
        IsSelected = !IsSelected;
        Selector.SetActive(IsSelected);
    }

    

    public void CreateGem()
    {
        //Destroy(Sphere);
        _color = _gemMats[Random.Range(0, _gemMats.Length)];
        Debug.Log(_color);
        Material m = Resources.Load("Materials/" + _color) as Material;
        Sphere.GetComponent<Renderer>().material = m;
        isMatched = false;
    }

    public void AddNeighbor(Gem g)
    {
        if(!Neighbors.Contains(g))
            Neighbors.Add(g);
    }

    public bool IsNeighborWith(Gem g)
    {
        if (Neighbors.Contains(g))
        {
            return true;
        }
        return false;
    }

    public void RemoveNeighbor(Gem g)
    {
        Neighbors.Remove(g);
    }

    public void OnMouseDown()
    {
        if (!GameObject.Find("Board").GetComponent<Board>().IsSwapping)
        {
            ToggleSelector();
            GameObject.Find("Board").GetComponent<Board>().SwapGems(this);
        }
    }
}
