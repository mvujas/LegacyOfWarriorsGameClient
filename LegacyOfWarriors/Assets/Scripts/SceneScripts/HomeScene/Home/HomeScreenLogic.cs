using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utils.Remote;
using UnityEngine.UI;
using Remote.Implementation;

public class HomeScreenLogic : TemporarySimpleGUIComponent
{
    [SerializeField]
    private Text userInfoText = null;
    [SerializeField]
    private Text findMatchButtonText = null;
    [SerializeField]
    private Text queueInfoText = null;

    private const string FIND_MATCH_TEXT = "Pronađi meč";
    private const string QUIT_QUEUE_TEXT = "Napusti red";

    private bool m_isInQueue = false;
    private bool m_waitingForResponse = false;

    private MutablePassiveRequestMapper m_mapper = new MutablePassiveRequestMapper();
    protected override RemoteRequestMapper GetRemoteRequestMapper()
    {
        return m_mapper;
    }

    public override void Show()
    {
        PrepareHomeScreen();
        base.Show();
    }

    private void PrepareHomeScreen()
    {
        SetCorrectButtonText();
        queueInfoText.ResetText();
        userInfoText.text = "Dobrodošao!\nKorisnik: " + globalReference.UserInfoContainer.UserInfo.Username;
    }

    private void SetCorrectButtonText()
    {
        findMatchButtonText.SetRegularText(m_isInQueue ? QUIT_QUEUE_TEXT : FIND_MATCH_TEXT);
    }

    private void Awake()
    {
        if (userInfoText == null)
        {
            throw new ArgumentNullException(nameof(userInfoText));
        }
        if (findMatchButtonText == null)
        {
            throw new ArgumentNullException(nameof(findMatchButtonText));
        }
        if (queueInfoText == null)
        {
            throw new ArgumentNullException(nameof(queueInfoText));
        }

        m_mapper.AddHandlerAction<QueueEntryResponse>(HandleQueueEntryResponse);
        m_mapper.AddHandlerAction<QueueExitResponse>(HandleQueueExitResponse);
        m_mapper.AddHandlerAction<GameFoundNotification>(HandleGameFoundNotification);
    }

    public void HandleQueueButtonClick()
    {
        if (m_waitingForResponse)
        {
            return;
        }
        m_waitingForResponse = true;
        if(m_isInQueue)
        {
            ExitQueue();
        }
        else
        {
            EnterQueue();
        }
    }

    private void EnterQueue()
    {
        globalReference.GameClient.Send(new QueueEntryRequest
        {
            Deck = new int[] { 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 6, 6, 7, 8 }
        });
    }

    private void ExitQueue()
    {
        globalReference.GameClient.Send(new QueueExitRequest());
    }
    
    private void HandleQueueEntryResponse(QueueEntryResponse response)
    {
        if (response.Successfulness)
        {
            queueInfoText.SetRegularText("U redu si za igru\nIgra će početi kada se pronađe protivnik");
            m_isInQueue = true;
            SetCorrectButtonText();
        }
        else
        {
            queueInfoText.SetRegularText("*** GREŠKA PRI ULASKU U RED ***\n" + response.Message);
        }
        m_waitingForResponse = false;
    }

    private void HandleQueueExitResponse(QueueExitResponse response)
    {
        if (response.Successfulness)
        {
            queueInfoText.SetRegularText("Nisi u redu za igru");
            m_isInQueue = false;
            SetCorrectButtonText();
        }
        else
        {
            queueInfoText.SetErrorText("*** GREŠKA PRI IZLASKU IZ REDA ***\n" + response.Message);
        }
        m_waitingForResponse = false;
    }

    private void HandleGameFoundNotification(GameFoundNotification notification)
    {
        globalReference.GameFoundNotification = notification;
        globalReference.SceneController.LoadScene("GameScene");
    }
}
