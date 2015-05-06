using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class Board : MonoBehaviour 
{
    public List<Gem> gems = new List<Gem>();

    public int GridWidth;
    public int GridHeight;
    public GameObject gemPrefab;
    public Gem lastGem;
    public Vector3 gem1Start, gem1End, gem2Start, gem2End;
    public bool isSwappin = false;
    public Gem gem1, gem2;
    public float startTime;
    public float SwapRate = 2;

	// Use this for initialization
	void Start () 
    {
	    for(int y = 0; y < GridHeight; y++)
        {
            for (int x = 0; x < GridWidth; x++)
            {
                GameObject g = Instantiate(gemPrefab, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                g.transform.parent = gameObject.transform;
                gems.Add(g.GetComponent<Gem>());
            }
        }
        gameObject.transform.position = new Vector3(-2.5f, -2f, 0);
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (isSwappin)
        {
            MoveGem(gem1, gem1End, gem1Start);
            MoveNegGem(gem2, gem2End, gem2Start);
            if (Vector3.Distance(gem1.transform.position, gem1End) < .1f || Vector3.Distance(gem2.transform.position, gem2End) < .1f)
            {
                gem1.transform.position = gem1End;
                gem2.transform.position = gem2End;

                gem1.ToggleSelector();
                gem2.ToggleSelector();

                isSwappin = false;
                TogglePhysics(false);

                lastGem = null;
            }
        }
	}

    public void MoveGem(Gem gemToMove, Vector3 toPos, Vector3 fromPos)
    {
        Vector3 center = (fromPos + toPos) * .5f;
        center -= new Vector3(0, 0, .01f);
        Vector3 riseRelCenter = fromPos - center;
        Vector3 setRelCenter = toPos - center;
        float fracComplete = (Time.time - startTime) / SwapRate;
        gemToMove.transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
        gemToMove.transform.position += center;
    }

    public void MoveNegGem(Gem gemToMove, Vector3 toPos, Vector3 fromPos)
    {
        Vector3 center = (fromPos + toPos) * .5f;
        center -= new Vector3(0, 0, -.01f);
        Vector3 riseRelCenter = fromPos - center;
        Vector3 setRelCenter = toPos - center;
        float fracComplete = (Time.time - startTime) / SwapRate;
        gemToMove.transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
        gemToMove.transform.position += center;
    }

    public void TogglePhysics(bool isOn)
    {
        for (int i = 0; i < gems.Count; i++)
        {
            gems[i].GetComponent<Rigidbody>().isKinematic = isOn;
        }
    }

    public void SwapGems(Gem currentGem)
    {
        if (lastGem == null)
        {
            lastGem = currentGem;
        }
        else if(lastGem == currentGem)
        {
            lastGem = null;
        }
        else if (lastGem != null && lastGem != currentGem)
        {
            if(lastGem.IsNeighborWith(currentGem))
            {
                gem1Start = lastGem.transform.position;
                gem1End = currentGem.transform.position;
                gem2Start = currentGem.transform.position;
                gem2End = lastGem.transform.position;

                startTime = Time.time;
                TogglePhysics(true);
                
                gem1 = lastGem;
                gem2 = currentGem;

                isSwappin = true;
            }
            else
            {
                lastGem.ToggleSelector();
                lastGem = currentGem;
            }
        }
    }
}
