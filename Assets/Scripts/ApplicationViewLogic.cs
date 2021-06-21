using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ApplicationViewLogic : MonoBehaviour
{
    [Header("Firebase auth")]
    [SerializeField] AuthUnityModel authUnityModel;
    [SerializeField] Button loginButton;
    [SerializeField] Animator transition;


    void Awake()
    { 
        authUnityModel.loginSuccess.AddListener(SwitchToMainPage);
        //loginButton.onClick.AddListener(SwitchToMainPage);
    }
     void SwitchToMainPage()
     {
         LoadNextLevel();
     }
     void LoadNextLevel()
     {
         
         StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
     }
     IEnumerator LoadLevel(int buildIndex)
     {
         transition.SetTrigger("Start");
         yield return new WaitForSeconds(1f);
         SceneManager.LoadScene(buildIndex);
     }

}
