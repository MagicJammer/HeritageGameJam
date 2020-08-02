using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pocWorkStation : MonoBehaviour {
    public List<IngredientType> TestFinishedIngredients = new List<IngredientType>();
    public List<IngredientType> _currentIngredients = new List<IngredientType>();
    public float _currentCookTime;
    public float InteractRange = 1f;
    public Vector3 InteractOffset;
    public LayerMask PlayerLayer;
    public bool IsPlayerNearby;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        Vector3 currentPos = this.transform.position;

        Collider2D col = Physics2D.OverlapCircle(currentPos + InteractOffset, InteractRange, PlayerLayer);
        IsPlayerNearby = col != null;

        if (_currentCookTime > 0) {
            _currentCookTime -= Time.deltaTime;
            if (_currentCookTime <= 0) {
                foreach (var item in _currentIngredients) {
                    TestFinishedIngredients.Add(item);
                    print("done cook/slice item" + _currentIngredients);
                }
                _currentIngredients.Clear();
            }
        }
    }

    public IngredientType[] GetCurrentCookedIngredients() {
        IngredientType[] currentIngredients = TestFinishedIngredients.ToArray();
        TestFinishedIngredients.Clear();
        return currentIngredients;
    }

    public void CookingTimeUpdate(float time, IngredientType ingredient) {
        _currentCookTime += time;
        _currentIngredients.Add(ingredient);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Vector3 currentPos = this.transform.position;
        Gizmos.DrawWireSphere(currentPos + InteractOffset, InteractRange);
    }
}