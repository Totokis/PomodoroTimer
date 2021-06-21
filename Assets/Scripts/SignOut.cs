using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SignOut : MonoBehaviour
{
    [SerializeField] List<Button> buttons;
    void Awake()
    {
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(SignOutAndGoToLoginPage);
        }
    }
    void SignOutAndGoToLoginPage()
    {
       FirebaseAuth.DefaultInstance.SignOut();
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

    }
}
