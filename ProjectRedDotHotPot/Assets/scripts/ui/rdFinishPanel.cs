using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class rdFinishPanel : MonoBehaviour
{
    public Button ReturnToStartButton;
    public Button CloseButton;
    public Text AverageStarText;
    // Start is called before the first frame update
    void Start()
    {
        ReturnToStartButton.onClick.AddListener(ReturnToStart);
        ReturnToStartButton.onClick.AddListener(Close);

        AverageStarText.text = "Average Stars: " +rdScoreSystem.Seele.CurrentStarScores.ToString();
    }

    private void OnDestroy() {
        ReturnToStartButton.onClick.RemoveListener(ReturnToStart);
        ReturnToStartButton.onClick.RemoveListener(Close);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    void ReturnToStart() {
        //start menu
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);

        //for test delete below
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Close() {
        Application.Quit();
    }
}
