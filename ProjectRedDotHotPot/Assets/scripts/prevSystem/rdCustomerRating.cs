using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rdCustomerRating : MonoBehaviour
{
    int _stars = 5;
    float _timeLeft;
    bool _completed;
    void Start() {
        _timeLeft = 25;
        _stars = 5;
    }
    //void AddTime() {
    //    _timeLeft += 5;
    //    if (_timeLeft > 30)
    //        _timeLeft = 30;
    //    int s = Mathf.RoundToInt(_timeLeft / 5);
    //    if (s > 5)
    //        s = 5;
    //    _stars = s;
    //}
    void Update() {
        if (_completed) {
            Debug.Log("Customer gave this rating: " + _stars);
            NewCustomer();
            return;
        }
        _timeLeft -= Time.deltaTime;
        int s = Mathf.RoundToInt(_timeLeft / 5);
        _stars = s;
        if (_timeLeft <= 0 && !_completed) {
            Debug.Log("Customer lost");
            NewCustomer();
        }
    }
    void NewCustomer() {
        _stars = 5;
        _timeLeft = 25;
        _completed = false;
    }
}
