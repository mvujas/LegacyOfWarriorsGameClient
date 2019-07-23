using ClientUtils;
using Remote.Implementation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils.Delegates;

delegate float TransitionFunction(float time, float start, float change, float duration);

public class LoadingLogic : MonoBehaviour
{
    [SerializeField]
    private Image logoImage = null;
    [SerializeField]
    private GameObject loadingBar = null;
    [SerializeField]
    private Text infoText = null;
    [SerializeField]
    private float logoFadeInTime = 2;
    [SerializeField]
    private float loadingBarFadeInTime = 1;

    private void OnValidate()
    {
        logoFadeInTime = Mathf.Max(.1f, logoFadeInTime);
    }

    private CustomSlider slider;

    private GlobalReference globalReference = GlobalReference.GetInstance();

    private static TransitionFunction acceleratingTransition = (t, b, c, d) =>
    {
        t /= d;
        return c * t * t * t * t + b;
    };
    private static TransitionFunction linearTransition = (t, b, c, d) =>
    {
        return c * t / d + b;
    };

    private void Awake()
    {
        CheckAssignment();
        ResetGUI();
    }

    private void Start()
    {
        StartLogoTransition();
    }

    private void CheckAssignment()
    {
        if(logoImage == null)
        {
            throw new ArgumentNullException(nameof(logoImage));
        }
        if(loadingBar == null)
        {
            throw new ArgumentNullException(nameof(loadingBar));
        }
        if(infoText == null)
        {
            throw new ArgumentNullException(nameof(infoText));
        }
        slider = loadingBar.GetComponent<CustomSlider>() ?? 
            throw new ArgumentNullException(nameof(loadingBar) + " is not custom loader!");
    }

    #region EYE CANDY
    private void ResetGUI()
    {
        SetLogoTransparency(0);
        infoText.text = "";
        slider.SetTransparency(0);
    }

    private void SetLogoTransparency(float value)
    {
        value = Mathf.Clamp01(value);
        Color color = logoImage.color;
        color.a = value;
        logoImage.color = color;
    }

    private void StartLogoTransition()
    {
        StartCoroutine(PlayInTransition(acceleratingTransition, logoFadeInTime, SetLogoTransparency, StartLoadingBarTransition));
    }

    private void StartLoadingBarTransition()
    {
        StartCoroutine(PlayInTransition(linearTransition, loadingBarFadeInTime,
            slider.SetTransparency, () => StartAfterDelay(StartActualLoading, .1f)));
    }

    private IEnumerator PlayInTransition(TransitionFunction transition, float time,
        Action<float> changeTransparency, Runnable endFunction = null)
    {
        for (float value = 0; value <= time; value += Time.fixedDeltaTime)
        {
            var newTransparency = transition(value, 0, 1, logoFadeInTime);
            changeTransparency?.Invoke(newTransparency);
            yield return new WaitForFixedUpdate();
        }

        changeTransparency?.Invoke(1f);

        endFunction?.Invoke();
    }
    #endregion

    private void ShowTextAfterDelay(string text, float delay)
    {
        StartAfterDelay(() => infoText.text = text, delay);
    }

    private void StartAfterDelay(Runnable func, float delay)
    {
        StartCoroutine(StartAfterDelayCoroutine(func, delay));
    }

    private IEnumerator StartAfterDelayCoroutine(Runnable func, float delay)
    {
        yield return new WaitForSeconds(delay);
        func?.Invoke();
    }

    #region LOADING 
    private void StartActualLoading()
    {
        infoText.text = "Započinjanje učitavanja";
        StartAfterDelay(TryToConnect, .2f);
    }

    private void TryToConnect()
    {
        GameClient gameClient = globalReference.GameClient;
        
        ConnectionAttempt(gameClient, 0, 5);
    }

    private void ConnectionAttempt(GameClient gameClient, int currentTry, int maxTries)
    {
        if(currentTry >= maxTries)
        {
            HandleFailedConnecting();
            return;
        }
        infoText.text = "Povezivanje na server" + new String('.', currentTry % 3 + 1);
        gameClient.Start(
            () => {
                slider.Percent = .5f;
                StartAfterDelay(() => CardListLoading(gameClient), .5f);
            },
            () => {
                StartAfterDelay(() => ConnectionAttempt(gameClient, currentTry + 1, maxTries), .2f);
            }
        );
    }

    private void CardListLoading(GameClient gameClient)
    {
        ExecutionQueue executionQueue = globalReference.ExecutionQueue;
        infoText.text = "Učitavanje liste karata";
        gameClient.ChangeRequestMapper(new LoadingRequestMapper(
            onUpToDate: () => executionQueue.Add(() => Debug.Log("Up TO Date!")),
            onNotUpToDate: cl =>
            {
                executionQueue.Add(() => Debug.Log("Nova verzija stigla!"));
            }
        ));
        gameClient.Send(new CardListRequest { Version = null });
    }

    private void HandleFailedConnecting()
    {
        infoText.text = "Ne mogu se povezati";
    }

    #endregion
}
