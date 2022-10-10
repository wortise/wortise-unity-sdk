using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleScene : MonoBehaviour
{
    private Queue<Action> executionQueue = new Queue<Action>();
    
    private WortiseInterstitial interstitialAd;
    private WortiseRewarded     rewardedAd;

    private Button buttonShowInterstitial;
    private Button buttonShowRewarded;
    private Text   textStatus;
    
    
    void Start()
    {
        buttonShowInterstitial = GameObject.Find("Button Show Interstitial").GetComponent<Button>();
        buttonShowRewarded     = GameObject.Find("Button Show Rewarded")    .GetComponent<Button>();

        textStatus = GameObject.Find("Text Status").GetComponent<Text>();

        WortiseSdk.OnInitialized += () => Enqueue(OnSdkInitialized());

        WortiseSdk.Initialize("1f838a77-7032-4436-bfe4-4a902ec70b7a");

        interstitialAd = new WortiseInterstitial("test-interstitial");

        interstitialAd.OnFailed += () => Enqueue(OnInterstitialFailed());
        interstitialAd.OnLoaded += () => Enqueue(OnInterstitialLoaded());

        rewardedAd = new WortiseRewarded("test-rewarded");

        rewardedAd.OnCompleted += (reward) => Enqueue(OnRewardedCompleted(reward));
        rewardedAd.OnFailed    += () => Enqueue(OnRewardedFailed());
        rewardedAd.OnLoaded    += () => Enqueue(OnRewardedLoaded());
    }

    void Update()
    {
        lock(executionQueue) {
            while (executionQueue.Count > 0) {
                executionQueue.Dequeue().Invoke();
            }
        }
    }
    
    private IEnumerator OnInterstitialFailed()
    {
        Debug.Log("Interstitial failed");

        EnableButton(buttonShowInterstitial, false);

        textStatus.text = "Interstitial failed";
        
        yield return null;
    }
    
    private IEnumerator OnInterstitialLoaded()
    {
        Debug.Log("Interstitial loaded!");

        EnableButton(buttonShowInterstitial, true);

        textStatus.text = "Interstitial loaded!";
        
        yield return null;
    }

    private IEnumerator OnRewardedCompleted(WortiseReward reward)
    {
        string message = "Rewarded completed! (amount = " + reward.amount + ", label = " + reward.label + ", success = " + reward.success + ")";

        Debug.Log(message);

        EnableButton(buttonShowInterstitial, true);

        textStatus.text = message;
        
        yield return null;
    }

    private IEnumerator OnRewardedFailed()
    {
        Debug.Log("Rewarded failed");

        EnableButton(buttonShowRewarded, false);

        textStatus.text = "Rewarded failed";
        
        yield return null;
    }
    
    private IEnumerator OnRewardedLoaded()
    {
        Debug.Log("Rewarded loaded!");

        EnableButton(buttonShowRewarded, true);

        textStatus.text = "Rewarded loaded!";
        
        yield return null;
    }

    private IEnumerator OnSdkInitialized()
    {
        Debug.Log("SDK initialized");

        textStatus.text = "SDK initialized";

        WortiseConsentManager.RequestOnce();

        yield return null;
    }
    
    private void EnableButton(Button button, bool enable)
    {
        button.interactable = enable;
    }
    
    private void Enqueue(IEnumerator action)
    {
        lock (executionQueue) {
            executionQueue.Enqueue (() => StartCoroutine(action));
        }
    }
    
    public void LoadInterstitial()
    {        
        interstitialAd.LoadAd();
    }

    public void LoadRewarded()
    {        
        rewardedAd.LoadAd();
    }

    public void ShowConsentDialog()
    {
        WortiseConsentManager.Request();
    }
    
    public void ShowInterstitial()
    {
        interstitialAd.ShowAd();
    }

    public void ShowRewarded()
    {
        rewardedAd.ShowAd();
    }
}
