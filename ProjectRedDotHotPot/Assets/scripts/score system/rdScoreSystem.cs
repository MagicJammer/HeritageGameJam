using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class rdScoreSystem : Singleton<rdScoreSystem>
{
    [Header("Score Settings")]
    public GameObject TimerText;
    public GameObject ScoreText;
    public GameObject FinishPanel;
    public bool timePaused = false;
    public float StartTime = 20;
    float _percentage = 0.20f;
 
     Text Stars_Text;
     Text Timer_Text;

    [SerializeField] float _timeChecker;
    [SerializeField] float _previousTime;
    [SerializeField] float _timer;
    int _stars = 5;

    [Header("Average Scores")]
    public int CurrentRecipeNumbersAverage;
    public int CurrentStarScores;
    [SerializeField] bool _justStarted = true;

    protected override void OnDestroy() {
        base.OnDestroy();
        if (rdRecipeManager.Seele != null) {
            rdRecipeManager.Seele.OnNewRecipe -= OnNewRecipe;
        }
    }

    private void Start() {
        rdRecipeManager.Seele.OnNewRecipe += OnNewRecipe;

        GameObject tt = Instantiate(TimerText);
        GameObject st =  Instantiate(ScoreText);
        tt.transform.SetParent(rdUIManager.Seele.MainCanvas.transform, false);
        st.transform.SetParent(rdUIManager.Seele.MainCanvas.transform, false);

        Stars_Text = st.GetComponent<Text>();
        Timer_Text = tt.GetComponent<Text>();

        OnNewRecipe();
        CurrentRecipeNumbersAverage = rdRecipeManager.Seele._allOrders;
    }

    void OnNewRecipe() {
        float updateTime = rdRecipeManager.Seele._currentCustomer.RecipeMakingTime;
        timePaused = false;
         _timer = updateTime;
        _timeChecker = _timer * _percentage;
        _stars = 5;

        if (!_justStarted) {
            CurrentStarScores += _stars;
        }
        _justStarted = false;
    }

    public void OrderServed() {
        timePaused = true;
    }

    //ending one
    public void ShowScore() {
        CurrentStarScores += _stars;
        print("final stars" + CurrentStarScores);
        //public gameobject showing stars and have button thingy
        GameObject fp = Instantiate(FinishPanel);
        fp.transform.SetParent(rdUIManager.Seele.MainCanvas.transform, false);
    }

    void PausedTime() {
        Timer_Text.text = "";
        Stars_Text.text = _stars.ToString() + " stars";
    }

    void Update()
    {
        if (timePaused) {
            PausedTime();
            return;
        }

        Timer_Text.text = "" + (int)_timer;
        Stars_Text.text = _stars.ToString() + " stars";


        if (!timePaused)
        {
            StartTimer();
        }

        if(_stars < 0)
        {
            _stars = Mathf.Clamp(_stars, 0, 10);
            
        }
    }


    public void StartTimer()
    {      
        _timer -= Time.deltaTime;
        _previousTime += Time.deltaTime;

        if (_previousTime > _timeChecker)
        {
            DeleteStar(1);
            _previousTime = 0;
        }

        if (_timer <= 0)
        {
             _timer = Mathf.Clamp(_timer, 0.0f, 10f);
            DeleteStar(1);
            Debug.Log("Customer no food, Customer sad");
            timePaused = true;
        }
    }

    public void ResetTimer()
    {
        _timer = StartTime;
        _previousTime = _timer;
    }
    public void AddStar(int newStar)
    {
        _stars += newStar;
        Stars_Text.text = _stars.ToString() + " stars";
    }

    public void DeleteStar(int removeStar)
    {
        _stars -= removeStar;
        Stars_Text.text = _stars.ToString() + " stars";
    }
}