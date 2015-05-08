using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gem : MonoBehaviour 
{
    public GameObject gemHolder;
    public GameObject sphere;
    public GameObject selector;

    string[] gemMats = { "Blue", "Green", "Grey", "Purple","Red", "Yellow"};
    public string color = "";

    public List<Gem> Neighbors = new List<Gem>();
    
    public bool isSelected = false;
    public bool isMatched = false;

    public int XCoord
    {
        get
        {
            return Mathf.RoundToInt(transform.localPosition.x);
        }
    }

    public int YCoord
    {
        get
        {
            return Mathf.RoundToInt(transform.localPosition.y);
        }
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
        isSelected = !isSelected;
        sphere.transform.FindChild("Selector").gameObject.SetActive(isSelected);
    }

    public void CreateGem()
    {
        Destroy(sphere);
        color = gemMats[Random.Range(0, gemMats.Length)];
        GameObject gemPrefab = Resources.Load("Prefabs/" + color) as GameObject;
        sphere = (GameObject)Instantiate(gemPrefab, Vector3.zero, Quaternion.identity);
        sphere.transform.parent = gemHolder.transform;
        sphere.transform.localPosition = Vector3.zero;
        isMatched = false;
    }

    public void AddNeighbor(Gem g)
    {
        if(!Neighbors.Contains(g))
            Neighbors.Add(g);
    }

    public bool IsNeighborWith(Gem g)
    {
        if(Neighbors.Contains(g))
        {
            return true;
        }
        return false;
    }

    public void RemoveNeighbor(Gem g)
    {
        Neighbors.Remove(g);
    }

    void OnMouseDown()
    {
        if (!GameObject.Find("Board").GetComponent<Board>().isSwappin)
        {
            ToggleSelector();
            GameObject.Find("Board").GetComponent<Board>().SwapGems(this);
        }
    }
}
