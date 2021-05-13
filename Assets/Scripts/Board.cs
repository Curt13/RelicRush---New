using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    wait, 
    move
}


public class Board : MonoBehaviour
{
    public GameState currentState = GameState.move; 

    public int width;
    public int height;
    public int offSet;

    public GameObject tilePrefab;

    public GameObject[] dots;
    private BackgroundTile[,] allTiles;
    private FindMatches findMatches;
    private ScoreManager scoreManager; 

    public int baseRelicScore = 25;
    public int[] scoreGoal; 
    public int streakScore = 1;

    public GameObject[,] allDots; 

    // Start is called before the first frame update
    void Start()
    {
        findMatches = FindObjectOfType<FindMatches>();
        allTiles = new BackgroundTile[width, height];
        allDots = new GameObject[width, height];
        scoreManager = FindObjectOfType<ScoreManager>();
        SetUp();
    }

    private void SetUp() //board generation
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                Vector2 tempPosition = new Vector2(i, j + offSet);
                GameObject backgroundTile = Instantiate(tilePrefab, tempPosition, Quaternion.identity) as GameObject;
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = "( " + i + ", " + j + " )";

                int dotToUse = Random.Range(0, dots.Length);

                int maxIterations = 0; //prevent infinate loop
                while (MatchesAt(i, j, dots[dotToUse]) && maxIterations < 100)
                {
                    dotToUse = Random.Range(0, dots.Length);
                    maxIterations++;
                }
                maxIterations = 0;
                Debug.Log(maxIterations);

                GameObject dot = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                dot.GetComponent<Dot>().row = j;
                dot.GetComponent<Dot>().column = i;
                dot.transform.parent = this.transform;
                dot.name = "( " + i + ", " + j + " )";
                allDots[i, j] = dot; 
            }
        }
    }

    private bool MatchesAt(int column, int row, GameObject piece)
    {
        if(column > 1 && row > 1 )
        {
            if(allDots[column -1, row].tag == piece.tag && allDots[column -2, row].tag == piece.tag)
            {
                return true; 
            }
            if (allDots[column, row -1].tag == piece.tag && allDots[column, row -2].tag == piece.tag)
            {
                return true;
            }
        } else if(column <= 1 || row <= 1) {
            if(row > 1)
            {
                if(allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
                {
                    return true; 
                }
            }
            if (column > 1)
            {
                if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
                {
                    return true;
                }
            }
        }

        return false; 
    }

    private void DestroyMatchesAt(int column, int row) //helper method
    {
        if(allDots[column, row].GetComponent<Dot>().isMatched)
        {
            findMatches.currentMatches.Remove(allDots[column, row]);
            Destroy(allDots[column, row]);
            scoreManager.IncrementScore(baseRelicScore * streakScore);
            allDots[column, row] = null; 
        }
    }
    
    public void DestroyMatches()
    {
        for(int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++) //checking for pieces, if they're matched they're destroyed
            {
                if(allDots[i, j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }
        StartCoroutine(DecreaseRowCo());
    }

    private IEnumerator DecreaseRowCo()
    {
        int nullCount = 0; //when a match is made sets the value to null then cauese the upper dots to drop and fill the posistion 
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if(allDots[i, j] == null)
                {
                    nullCount++; 
                } else if(nullCount > 0) {
                    allDots[i, j].GetComponent<Dot>().row -= nullCount;
                    allDots[i, j] = null; 
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(.4f);
        StartCoroutine(FillBoardCo());

    }

    private void RefillBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] == null) //checks through all pieces if they're null
                {
                    Vector2 tempPosition = new Vector2(i, j + offSet);
                    int dotToUse = Random.Range(0, dots.Length);
                    GameObject piece = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                    allDots[i, j] = piece;

                    piece.GetComponent<Dot>().row = j;
                    piece.GetComponent<Dot>().column = i;
                    piece.transform.parent = this.transform;
                    piece.name = "( " + i + ", " + j + " )";
                }
            }
        } 
    }

    private bool MatchesOnBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if(allDots[i, j] != null)
                {
                    if(allDots[i, j].GetComponent<Dot>().isMatched) //checks all dots for matches
                    {
                        return true; 
                    }
                }
            }
        }
                return false; 
    }

    private IEnumerator FillBoardCo()
    {
        RefillBoard();
        yield return new WaitForSeconds(.5f);

        while(MatchesOnBoard())
        {
            streakScore += 1; 
            yield return new WaitForSeconds(.5f);
            DestroyMatches(); 
        }
        yield return new WaitForSeconds(.5f);
        currentState = GameState.move;
        streakScore = 1;
    }
}
