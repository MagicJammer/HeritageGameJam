using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class rdScoreSystem : Singleton<rdScoreSystem>
{


    [Header("Score Settings")]
    public float StartTime = 20;
 
    public bool timePaused = false;
    public float starDecreasePerSecond = 3;

    public Text Stars_Text;
    public Text Timer_Text;

    float _previousTime;
    float _timer;
    float _stars = 25;
    bool _completed;

    void Start()
    {
        _timer = StartTime;
        _previousTime = _timer;
    }

    void Update()
    {
        Timer_Text.text = "" + (int)_timer;
        Stars_Text.text = _stars.ToString();

        _stars -= starDecreasePerSecond * Time.deltaTime;
        _stars = Mathf.RoundToInt(_stars);
        Stars_Text.text = _stars.ToString();

        if (!timePaused)
        {
            StartTimer();
        }

        if(_stars < 0)
        {
            _stars = Mathf.Clamp(_stars, 0.0f, 10f);
        }
    }


    public void StartTimer()
    {
       
        _timer -= Time.deltaTime;
        int seconds = Mathf.RoundToInt(_timer / 5);
        

        if (_previousTime - (int)_timer == starDecreasePerSecond)
        {
            DeleteStar(3);
            _previousTime = (int)_timer;
        }

        if (_timer <= 0)
        {
             _timer = Mathf.Clamp(_timer, 0.0f, 10f);
            Debug.Log("Customer no food, Customer sad");
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
        Stars_Text.text = _stars.ToString();
    }

    public void DeleteStar(int removeStar)
    {
        _stars -= removeStar;
        Stars_Text.text = _stars.ToString();
    }
}