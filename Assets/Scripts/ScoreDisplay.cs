using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour 
{
    // Use this for initialization
    void Start () 
    {
        Text myText = GetComponent<Text>();
        // Calculate the total score by summing scores from ScoreKepper to ScoreKepper4
        int totalScore = ScoreKepper.score + ScoreKepper2.score + ScoreKepper3.score + ScoreKepper4.score;
        myText.text = totalScore.ToString();
        
        // Optionally reset individual scores (comment the line below if you don't want to reset)
        ScoreKepper.Reset();
        ScoreKepper2.Reset();
        ScoreKepper3.Reset();
        ScoreKepper4.Reset();
    }
    
    // Update is called once per frame
    void Update () {
        
    }
}
