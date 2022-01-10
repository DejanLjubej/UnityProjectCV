using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotifyPlayerMessage : MonoBehaviour
{
    static TMP_Text messageToPlayer;
    static Animator messageBoxAnimator;

    static NotifyPlayerMessage _instance;

    public static NotifyPlayerMessage Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<NotifyPlayerMessage>();

                DontDestroyOnLoad(_instance.gameObject);

            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            if (this != _instance)
            {
                Destroy(this.gameObject);
            }
        }


    }

    public static void ShowMessageToPlayer(string message)
    {
        messageBoxAnimator = _instance.GetComponentInChildren<Animator>();
        messageToPlayer = _instance.GetComponentInChildren<TMP_Text>();
        messageToPlayer.text = message;
        messageBoxAnimator.SetTrigger("ShowMessage");
    }
}
