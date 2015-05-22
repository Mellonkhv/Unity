using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour 
{

    public List<Gem> gems = new List<Gem>();
    public int GridWidth;
    public int GridHeight;
    public GameObject GemPrefab;
    public Gem LastGem;
    public Vector3 Gem1Start;
    public Vector3 Gem1End;
    public Vector3 Gem2Start;
    public Vector3 Gem2End;
    public bool IsSwapping = false;
    public Gem Gem1;
    public Gem Gem2;
    public float StartTime;
    public float SwapRate = 2;

    // Use this for initialization
	void Start () 
    {
	    for (int y = 0; y < GridHeight; y++)
        {
            for (int x = 0; x < GridWidth; x++)
            {
                GameObject g = Instantiate(GemPrefab, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                g.transform.parent = gameObject.transform;
                gems.Add(g.GetComponent<Gem>());
            }

        }
        gameObject.transform.position = new Vector3(-2.5f, -2f, 0);
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (IsSwapping)
	    {
	        MoveGem(Gem1, Gem1End,Gem1Start);
            MoveNegGem(Gem2, Gem2End, Gem2Start);
	        if (Vector3.Distance(Gem1.transform.position, Gem1End) < .1f ||
	            Vector3.Distance(Gem2.transform.position, Gem2End) < .1f)
	        {
	            Gem1.transform.position = Gem1End;
                Gem2.transform.position = Gem2End;
	            IsSwapping = false;
	        }
	    }
	}

    public void MoveGem(Gem gemToMove, Vector3 toPos, Vector3 fromPos)
    {
        Vector3 center = (fromPos - toPos) *.5f;
        center -= new Vector3(0,0,1f);
        Vector3 riseRelCenter = fromPos - center;
        Vector3 setRelCenter = toPos - center;
        float fracComplete = (Time.time - StartTime)/SwapRate;
        gemToMove.transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
        gemToMove.transform.position += center;
    }

    public void MoveNegGem(Gem gemToMove, Vector3 toPos, Vector3 fromPos)
    {
        Vector3 center = (fromPos - toPos) * .5f;
        center -= new Vector3(0, 0, -1f);
        Vector3 riseRelCenter = fromPos - center;
        Vector3 setRelCenter = toPos - center;
        float fracComplete = (Time.time - StartTime) / SwapRate;
        gemToMove.transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
        gemToMove.transform.position += center;
    }

    public void SwapGems(Gem currentGem)
    {
        if (LastGem == null)
            LastGem = currentGem;
        else if (LastGem == currentGem)
            LastGem = null;
        else
        {
            if (LastGem.IsNeighborWith(currentGem))
            {
                Gem1Start = LastGem.transform.position;
                Gem2Start = currentGem.transform.position;
                Gem1End = currentGem.transform.position;
                Gem2End = LastGem.transform.position;

                StartTime = Time.time;

                Gem1 = LastGem;
                Gem2 = currentGem;
                IsSwapping = true;
            }
            else
            {
                LastGem.ToggleSelector();
                LastGem = currentGem;
            }
        }
    }


}