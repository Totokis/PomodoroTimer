using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AuthUnityModel : MonoBehaviour
{
     static FirebaseAuth Auth;
     static FirebaseUser User;
     string message;
    
    [HideInInspector] public UnityEvent loginSuccess = new UnityEvent();
    [HideInInspector] public UnityEvent registerSuccess = new UnityEvent();
    [HideInInspector] public UnityEvent loginFailed = new UnityEvent();
    [HideInInspector] public UnityEvent registerFailed = new UnityEvent();
    [HideInInspector] public UnityEvent passwordFailed = new UnityEvent();
    [HideInInspector] public UnityEvent usernameFailed = new UnityEvent();

    [SerializeField]  TMP_InputField usernameInput;
    [SerializeField]  TMP_InputField passwordInput;
    [SerializeField]  Button loginButton;
    [SerializeField]  Button registerButton;
    
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    
     void Awake()
    {
        loginFailed.AddListener((() => {
            Debug.Log(message);
        }));
         registerFailed.AddListener((() => {
            Debug.Log(message);
        }));
         passwordFailed.AddListener((() => {
            Debug.Log(message);
        }));
         
         loginButton.onClick.AddListener(()=> {
             StartCoroutine(Login(usernameInput.text, passwordInput.text));
         });

         registerButton.onClick.AddListener((() => {
             StartCoroutine(Register(usernameInput.text, passwordInput.text));
         }));
    }

     void Start()
    {
        StartCoroutine(CheckAndFixDependencies());
    }
    

     IEnumerator CheckAndFixDependencies()
     { 
         yield return new WaitForEndOfFrame();
        var checkAndFixDependeciesTask = FirebaseApp.CheckAndFixDependenciesAsync();

        yield return new WaitUntil(predicate: () => checkAndFixDependeciesTask.IsCompleted);

        var dependencyResult = checkAndFixDependeciesTask.Result;
        Debug.Log($"Firebase Database {FirebaseDatabase.DefaultInstance.RootReference==null}");

        if (dependencyResult == DependencyStatus.Available)
        {
            InitializeFirebase();
        }
        else
        {
            Debug.LogError($"Could not resolve all Firebase dependecies: {dependencyResult}");
        }
    }

     void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        Auth = FirebaseAuth.DefaultInstance;
        Auth.StateChanged += AuthStateChanged;
        CheckUser();
        //AuthStateChanged(this,null);
        //StartCoroutine(CheckAutoLogin());
    }
     private void CheckUser()
     {
         if (FirebaseAuth.DefaultInstance.CurrentUser != null)
         {
             loginSuccess.Invoke();
         }
     }
     void AuthStateChanged(object sender, EventArgs e)
    {
        CheckUser();
    }
     IEnumerator CheckAutoLogin()
    {
        yield return new WaitForEndOfFrame();

        if (User != null)
        {
            var reloadUserTask = User.ReloadAsync();

            yield return new WaitUntil(predicate: () => reloadUserTask.IsCompleted);
            
            AutoLogin();
        }
        else
        {
            Debug.Log($"Auto logging failed :( {User}");
            
        }
      
    }

     void AutoLogin()
    {
        if (User != null)
        {
            loginSuccess.Invoke();
            Debug.Log($"Auto logging {User}");
        }
        else
        {
            Debug.Log($"Auto logging failed :( {User}");
        }
    }

    
     IEnumerator Login(string email, string password)
    {
        var LoginTask = Auth.SignInWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            Debug.LogWarning($"Failed to register task with {LoginTask.Exception}");
            var firebaseException = LoginTask.Exception.GetBaseException() as FirebaseException;
            var errorCode = (AuthError)firebaseException.ErrorCode;


            message = "Login failed: ";

            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message += " Missing Email";
                    usernameFailed.Invoke();
                    break;
                case AuthError.MissingPassword:
                    message += " Missing Password";
                    passwordFailed.Invoke();
                    break;
                case AuthError.WrongPassword:
                    message += " Wrong Password";
                    passwordFailed.Invoke();
                    break;
                case AuthError.InvalidEmail:
                    message += " Invalid Email";
                    usernameFailed.Invoke();
                    break;
                case AuthError.UserNotFound:
                    message += " User not found";
                    usernameFailed.Invoke();
                    break;
            }
            loginFailed.Invoke();
        }
        else
        {
            User = LoginTask.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
            yield return new WaitForSeconds(1);
            loginSuccess.Invoke();
        }
    }
    
     IEnumerator Register(string email, string password)
    {
        var RegisterTask = Auth.CreateUserWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

        if (RegisterTask.Exception != null)
        {
            Debug.LogWarning($"Failed to register task with {RegisterTask.Exception}");
            var firebaseException = RegisterTask.Exception.GetBaseException() as FirebaseException;
            var errorCode = (AuthError)firebaseException.ErrorCode;

            message = "Register Failed: ";

            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message += " Missing Email";
                    usernameFailed.Invoke();
                    break;
                case AuthError.MissingPassword:
                    message += " Missing Password";
                    passwordFailed.Invoke();
                    break;
                case AuthError.WeakPassword:
                    message += " Weak Password";
                    passwordFailed.Invoke();
                    break;
                case AuthError.EmailAlreadyInUse:
                    message += " Email aleady in use";
                    usernameFailed.Invoke();
                    break;
            }
            registerFailed.Invoke();
        }
        else
        {
            registerSuccess.Invoke();
            Debug.Log("Registration completed !");
        }
    }

     void OnDestroy()
     {
         Auth.StateChanged -= AuthStateChanged;
         Debug.Log("Unsubscribed");
     }
}
