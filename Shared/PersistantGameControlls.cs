using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PersistantGameControlls : MonoBehaviour
{
    bool appQuitTrigger = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!appQuitTrigger)
                StartCoroutine(QuitApplication());
            else
                Application.Quit();
        }
    }

    IEnumerator QuitApplication()
    {
        NotifyPlayerMessage.ShowMessageToPlayer("You're about to quit");
        appQuitTrigger = true;
        float timer = 0;
        while (true)
        {
            timer += Time.unscaledDeltaTime;
            if (timer > 1.5f)
            {
                appQuitTrigger = false;
                Debug.Log($"quit app trigger is {appQuitTrigger}");
                yield break;
            }
            yield return null;

        }
    }
}
