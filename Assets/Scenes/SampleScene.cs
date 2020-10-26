using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleScene : MonoBehaviour
{
    private Queue<Action> executionQueue = new Queue<Action>();
    
    private Button buttonShow;
    private Text   textStatus;
    
    
    void Start()
    {
        buttonShow = GameObject.Find("Button Show").GetComponent<Button>();        
        textStatus = GameObject.Find("Text").GetComponent<Text>();

        WortiseSdk.OnInitialized += () => Enqueue(OnSdkInitialized());

        WortiseSdk.Initialize("1f838a77-7032-4436-bfe4-4a902ec70b7a");
        
        WortiseInterstitial.OnFailed += () => Enqueue(OnInterstitialFailed());
        WortiseInterstitial.OnLoaded += () => Enqueue(OnInterstitialLoaded());
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

        EnableShowInterstitial(false);

        textStatus.text = "Interstitial failed";
        
        yield return null;
    }
    
    private IEnumerator OnInterstitialLoaded()
    {
        Debug.Log("Interstitial loaded!");

        EnableShowInterstitial(true);

        textStatus.text = "Interstitial loaded!";
        
        yield return null;
    }

    private IEnumerator OnSdkInitialized()
    {
        Debug.Log("SDK initialized");

        textStatus.text = "SDK initialized";

        WortiseConsentManager.RequestOnce();

        yield return null;
    }
    
    private void EnableShowInterstitial(bool enable)
    {
        buttonShow.interactable = enable;
    }
    
    private void Enqueue(IEnumerator action)
    {
        lock (executionQueue) {
            executionQueue.Enqueue (() => StartCoroutine(action));
        }
    }
    
    public void LoadInterstitial()
    {
        string adUnitId = GameObject.Find("InputAdUnit").GetComponent<InputField>().text;
        
        if (!String.IsNullOrEmpty(adUnitId)) {
            WortiseInterstitial.LoadAd(adUnitId);
        }
    }

    public void ShowConsentDialog()
    {
        WortiseConsentManager.Request();
    }
    
    public void ShowInterstitial()
    {
        WortiseInterstitial.ShowAd();
    }
}
