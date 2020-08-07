using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class rdSceneChanger : MonoBehaviour
{
    public void GoGameScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void GoStartScene()
    {
        SceneManager.LoadScene("New Scene");

    }
}
