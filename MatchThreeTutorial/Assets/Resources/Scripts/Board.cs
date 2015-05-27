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
    public int AmountToMatch = 3;
    public bool IsMatched = false;

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
	    if (IsMatched)
	    {
	        for (int i = 0; i < gems.Count; i++)
	        {
	            if (gems[i].isMatched)
	            {
	                gems[i].CreateGem();
	                gems[i].transform.position = new Vector3(
                        gems[i].transform.position.x, 
                        gems[i].transform.position.y + 6,
	                    gems[i].transform.position.z);
	            }
	        }

	        IsMatched = false;
	    }
	    else if (IsSwapping)
	    {
	        MoveGem(Gem1, Gem1End,Gem1Start);
            MoveNegGem(Gem2, Gem2End, Gem2Start);
	        if (Vector3.Distance(Gem1.transform.position, Gem1End) < .1f ||
	            Vector3.Distance(Gem2.transform.position, Gem2End) < .1f)
	        {
	            Gem1.transform.position = Gem1End;
                Gem2.transform.position = Gem2End;
                Gem1.ToggleSelector();
                Gem2.ToggleSelector();
	            LastGem = null;

                IsSwapping = false;
                TogglePhysics(false);
                CheckMatch();
	        }
	    }
	}

    public void CheckMatch()
    {
        List<Gem> gem1List = new List<Gem>();
        List<Gem> gem2List = new List<Gem>();

        ConstructMatchList(Gem1._color, Gem1, Gem1.Xcoord, Gem1.Ycoord, ref gem1List);
        FixMatchList(Gem1, gem1List);
        ConstructMatchList(Gem2._color, Gem2, Gem2.Xcoord, Gem2.Ycoord, ref gem2List);
        FixMatchList(Gem2, gem2List);
        
    }

    public void ConstructMatchList(string color, Gem gem, int XCoord, int YCoord, ref List<Gem> MatchList)
    {
        if (gem == null)
        {
            return;
        }
        else if (gem._color !=color)
        {
            return;
        }
        else if (MatchList.Contains(gem))
        {
            return;
        }
        else
        {
            MatchList.Add(gem);
            if (XCoord == gem.Xcoord || YCoord == gem.Ycoord)
            {
                foreach (Gem g in gem.Neighbors)
                {
                    ConstructMatchList(color, g, XCoord, YCoord, ref MatchList);
                }
            }
        }
    }

    public void FixMatchList(Gem gem, List<Gem> listToFix )
    {
        List<Gem> rows = new List<Gem>();
        List<Gem> collumns = new List<Gem>();

        for (int i = 0; i < listToFix.Count; i++)
        {
            if (gem.Xcoord == listToFix[i].Xcoord)
            {
                rows.Add(listToFix[i]);
            }
            if (gem.Ycoord == listToFix[i].Ycoord)
            {
                collumns.Add(listToFix[i]);
            }
        }

        if (rows.Count >= AmountToMatch)
        {
            IsMatched = true;
            for (int i = 0; i < rows.Count; i++)
            {
                rows[i].isMatched = true;
            }
        }
        if (collumns.Count >= AmountToMatch)
        {
            IsMatched = true;
            for (int i = 0; i < collumns.Count; i++)
            {
                collumns[i].isMatched = true;
            }
        }
    }

    public void MoveGem(Gem gemToMove, Vector3 toPos, Vector3 fromPos)
    {
        Vector3 center = (fromPos + toPos) *.5f;
        center -= new Vector3(0,0,.1f);
        Vector3 riseRelCenter = fromPos - center;
        Vector3 setRelCenter = toPos - center;
        float fracComplete = (Time.time - StartTime)/SwapRate;
        gemToMove.transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
        gemToMove.transform.position += center;
    }

    public void MoveNegGem(Gem gemToMove, Vector3 toPos, Vector3 fromPos)
    {
        Vector3 center = (fromPos + toPos) * .5f;
        center -= new Vector3(0, 0, -.1f);
        Vector3 riseRelCenter = fromPos - center;
        Vector3 setRelCenter = toPos - center;
        float fracComplete = (Time.time - StartTime) / SwapRate;
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

                TogglePhysics(true);

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