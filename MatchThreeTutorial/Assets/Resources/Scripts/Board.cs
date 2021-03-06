﻿using UnityEngine;
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
    public bool SwapBack = false;
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
                
	            LastGem = null;

                IsSwapping = false;
                TogglePhysics(false);
	            if (!SwapBack)
	            {
                    Gem1.ToggleSelector();
                    Gem2.ToggleSelector();
	                CheckMatch();
	            }
	            else
	            {
	                SwapBack = false;
	            }
	        }
	    }
	    else if (!DetermineBoardState())
	    {
	        for (int i = 0; i < gems.Count; i++)
	        {
	            CheckForNearbyMatches(gems[i]);
	        }

            if (!DoesBoardContainMatches())
            {
                IsMatched = true;
                for (int i = 0; i < gems.Count; i++)
                {
                    gems[i].isMatched = true;
                }
            }
	    } 
	}

    public bool DoesBoardContainMatches()
    {
        TogglePhysics(true);
        for (int i = 0; i < gems.Count; i++)
        {
            for (int j = 0; j < gems.Count; j++)
            {
                if (gems[i].IsNeighborWith(gems[j]))
                {
                    Gem g = gems[i];
                    Gem f = gems[j];
                    Vector3 GTemp = g.transform.position;
                    Vector3 FTemp = f.transform.position;
                    List<Gem> tempNeighbors = new List<Gem>(g.Neighbors);
                    g.transform.position = FTemp;
                    f.transform.position = GTemp;
                    g.Neighbors = f.Neighbors;
                    f.Neighbors = tempNeighbors;
                    List<Gem> testListG = new List<Gem>();
                    ConstructMatchList(g._color, g, g.Xcoord, g.Ycoord, ref testListG);
                    if (TestMatchList(g,testListG))
                    {
                        g.transform.position = GTemp;
                        f.transform.position = FTemp;
                        f.Neighbors = g.Neighbors;
                        g.Neighbors = tempNeighbors;
                        TogglePhysics(false);
                        return true;
                    }
                    List<Gem> testListF = new List<Gem>();
                    ConstructMatchList(f._color, f, f.Xcoord, f.Ycoord, ref testListF);
                    if (TestMatchList(f, testListF))
                    {
                        g.transform.position = GTemp;
                        f.transform.position = FTemp;
                        f.Neighbors = g.Neighbors;
                        g.Neighbors = tempNeighbors;
                        TogglePhysics(false);
                        return true;
                    }
                    g.transform.position = GTemp;
                    f.transform.position = FTemp;
                    f.Neighbors = g.Neighbors;
                    g.Neighbors = tempNeighbors;
                    TogglePhysics(true);
                }
            }
        }

        return false;
    }

    public bool DetermineBoardState()
    {
        for (int i = 0; i < gems.Count; i++)
        {
            if (gems[i].transform.localPosition.y > 4)
            {
                return true;
            }
            else if (gems[i].GetComponent<Rigidbody>().velocity.y > .1f)
            {
                return true;
            }
        }
        return false;
    }

    public void CheckMatch()
    {
        List<Gem> gem1List = new List<Gem>();
        List<Gem> gem2List = new List<Gem>();

        ConstructMatchList(Gem1._color, Gem1, Gem1.Xcoord, Gem1.Ycoord, ref gem1List);
        FixMatchList(Gem1, gem1List);
        ConstructMatchList(Gem2._color, Gem2, Gem2.Xcoord, Gem2.Ycoord, ref gem2List);
        FixMatchList(Gem2, gem2List);
        if (!IsMatched)
        {
            SwapBack = true;
            ResetGems();
        }
    }

    public void ResetGems()
    {
        Gem1Start = Gem1.transform.position;
        Gem2Start = Gem2.transform.position;
        Gem1End = Gem2.transform.position;
        Gem2End = Gem1.transform.position;

        StartTime = Time.time;

        TogglePhysics(true);

        IsSwapping = true;
    }

    public void CheckForNearbyMatches(Gem g)
    {
        List<Gem> gemlList = new List<Gem>();
        ConstructMatchList(g._color, g, g.Xcoord, g.Ycoord, ref gemlList);
        FixMatchList(g, gemlList);
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

    public bool TestMatchList(Gem gem, List<Gem> listToFix)
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
            return true;
        }
        if (collumns.Count >= AmountToMatch)
        {
            return true;
        }
        return false;
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
            GameObject.Find("Game").GetComponent<GameUI>().AddScore(1);
            IsMatched = true;
            for (int i = 0; i < rows.Count; i++)
            {
                rows[i].isMatched = true;
            }
        }
        if (collumns.Count >= AmountToMatch)
        {
            GameObject.Find("Game").GetComponent<GameUI>().AddScore(1);
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