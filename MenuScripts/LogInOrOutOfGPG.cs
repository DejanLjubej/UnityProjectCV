using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;

public class LogInOrOutOfGPG : MonoBehaviour
{
    [SerializeField] GameObject logoutImageObj;

    bool isPlayerAuthenticatedInGPG=false;

    void Start()
    {
        isPlayerAuthenticatedInGPG = PlayGamesPlatform.Instance.IsAuthenticated();
        logoutImageObj.SetActive(isPlayerAuthenticatedInGPG);
    }

    void Update() { 
        
        if(isPlayerAuthenticatedInGPG != PlayGamesPlatform.Instance.IsAuthenticated())
        {
            isPlayerAuthenticatedInGPG = !isPlayerAuthenticatedInGPG;
            logoutImageObj.SetActive(isPlayerAuthenticatedInGPG);
        }
    }

    public void LogInOrOut()
    {
        if (!isPlayerAuthenticatedInGPG)
        {
            PlayGamesPlatform.Instance.Authenticate(GooglePlayGames.BasicApi.SignInInteractivity.CanPromptAlways, result => {
                string resultString = result.ToString();
                if ((resultString != "Success") && (resultString != "AlreadyInProgress"))
                {
                    NotifyPlayerMessage.ShowMessageToPlayer($"Could not log you in. {resultString}");
                }
            });
        }
        else
        {
            PlayGamesPlatform.Instance.SignOut();
            NotifyPlayerMessage.ShowMessageToPlayer("You signed out");
        }
    }
}
