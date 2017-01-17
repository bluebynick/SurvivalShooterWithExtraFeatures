using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public static int score; //needs to be static so it doesn't belong to the instance of the scoreManager class but it is instead stored within the class itself
    //we can have multiple instance of scoremanager but they would all share the same score b/c it belongs to the type, not the instance

    Text text;


    void Awake ()
    {
        text = GetComponent <Text> ();
        score = 0;
    }


    void Update ()
    {
        text.text = "Score: " + score;
    }
}
