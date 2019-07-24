using ClientUtils;
using Remote.Implementation;
using Remote.InGameObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils.Delegates;


[RequireComponent(typeof(CardListLoader))]
public class LoadingLogic : MonoBehaviourWithAddOns
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

    private CardListLoader cardListLoader = null;

    private CustomSlider slider;

    private GlobalReference globalReference = GlobalReference.GetInstance();

    private static TransitionFunction<float> acceleratingTransition = (t, b, c, d) =>
    {
        t /= d;
        return c * t * t * t * t + b;
    };
    private static TransitionFunction<float> linearTransition = (t, b, c, d) =>
    {
        return c * t / d + b;
    };

    private void Awake()
    {
        cardListLoader = GetComponent<CardListLoader>();
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
        slider.OnSliderFillUp = OnLoadingFinished;
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
            slider.SetTransparency, () => ExecuteAfterDelay(StartActualLoading, .1f)));
    }

    private IEnumerator PlayInTransition(TransitionFunction<float> transition, float time,
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
        ExecuteAfterDelay(() => infoText.text = text, delay);
    }

    #region LOADING 
    private void StartActualLoading()
    {
        infoText.text = "Započinjanje učitavanja";
        ExecuteAfterDelay(TryToConnect, .2f);
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
                ExecuteAfterDelay(() => CardListLoading(gameClient), .5f);
            },
            () => {
                ExecuteAfterDelay(() => ConnectionAttempt(gameClient, currentTry + 1, maxTries), .2f);
            }
        );
    }

    private void CardListLoading(GameClient gameClient)
    {
        infoText.text = "Učitavanje liste karata";
        CardList currentCardList = cardListLoader.LoadCardList();
        gameClient.ChangeRequestMapper(new LoadingRequestMapper(
            onUpToDate: () => RunInMainThread(() => FinishAddingNewCardList(currentCardList)),
            onNotUpToDate: newCardList => RunInMainThread(() => FinishAddingNewCardList(newCardList, true))
        ));

        CardListRequest request = new CardListRequest
        {
            Version = (currentCardList == null ? 
                    null : 
                    currentCardList.Vesion)
        };

        gameClient.Send(request);
    }

    private void FinishAddingNewCardList(CardList cardList, bool overwriteFile = false)
    {
        globalReference.CardList = cardList;
        if(overwriteFile)
        {
            cardListLoader.SaveCardList(cardList);
        }
        slider.Percent = 1f;
        ExecuteAfterDelay(() => {
            infoText.text = "";
        }, .3f);
    }

    private void HandleFailedConnecting()
    {
        infoText.color = Color.red;
        infoText.text = "Greška: Ne može se uspostaviti veza sa serverom\n\nProzor će se automatski ugasiti za nekoliko trenutaka";

        ExecuteAfterDelay(Application.Quit, 5f);
    }

    private void OnLoadingFinished()
    {
        globalReference.SceneController.LoadScene("LoginScene");
    }

    #endregion
}
