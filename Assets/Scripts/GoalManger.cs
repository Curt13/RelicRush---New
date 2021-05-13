using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Goal   //data container
{
    public int numberOfGoals;
    public int numberOfMatches;
    public Sprite goalSprite;
    public string matchValue;
}


public class GoalManger : MonoBehaviour
{
    public Goal[] goals;

    public GameObject goalPrefab;
    public GameObject goalGameParent;
    public GameObject goalIntroParent; 
    // Start is called before the first frame update
    void Start()
    {
        IntroGoals();
    }

    void IntroGoals()
    {
        for(int i = 0; i < goals.Length; i++)
        {
            GameObject goal = Instantiate(goalPrefab, goalIntroParent.transform.position, Quaternion.identity);
            goal.transform.SetParent(goalIntroParent.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
