using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WortiseInterstitial
{
    #if UNITY_ANDROID
    private static AndroidJavaObject activity
    {
        get
        {
            return WortiseSdk.activity;
        }
    }
    
    private AndroidJavaObject interstitialAd;
    #endif
            
    public bool IsAvailable
    {
        get
        {
            #if UNITY_ANDROID
            return interstitialAd.Call<bool>("isAvailable");
            #else
            return false;
            #endif
        }
    }
    
    public bool IsDestroyed
    {
        get
        {
            #if UNITY_ANDROID
            return interstitialAd.Call<bool>("isDestroyed");
            #else
            return false;
            #endif
        }
    }

    public bool IsShowing
    {
        get
        {
            #if UNITY_ANDROID
            return interstitialAd.Call<bool>("isShowing");
            #else
            return false;
            #endif
        }
    }
    
    public event Action OnClicked;
    public event Action OnDismissed;
    public event Action OnFailed;
    public event Action OnLoaded;
    public event Action OnShown;

    
    public WortiseInterstitial(string adUnitId)
    {
        interstitialAd = new AndroidJavaObject("com.wortise.ads.interstitial.InterstitialAd", activity, adUnitId);
        interstitialAd.Call("setListener", new InterstitialAdListener(this));
    }

    public void Destroy()
    {
        #if UNITY_ANDROID
        interstitialAd.Call("destroy");
        #endif
    }

    public void LoadAd()
    {
        #if UNITY_ANDROID
        interstitialAd.Call("loadAd");
        #endif
    }
    
    public bool ShowAd()
    {
        #if UNITY_ANDROID
        return interstitialAd.Call<bool>("showAd");
        #else
        return false;
        #endif
    }
    
    
    #if UNITY_ANDROID
    private class InterstitialAdListener : AndroidJavaProxy
    {
        private WortiseInterstitial interstitialAd;


        public InterstitialAdListener(WortiseInterstitial interstitialAd) : base("com.wortise.ads.interstitial.InterstitialAd$Listener")
        {
            this.interstitialAd = interstitialAd;
        }
        
        public void onInterstitialClicked(AndroidJavaObject ad)
        {
            interstitialAd.OnClicked();
        }

        public void onInterstitialDismissed(AndroidJavaObject ad)
        {
            interstitialAd.OnDismissed();
        }

        public void onInterstitialFailed(AndroidJavaObject ad, AndroidJavaObject error)
        {
            interstitialAd.OnFailed();
        }

        public void onInterstitialLoaded(AndroidJavaObject ad)
        {
            interstitialAd.OnLoaded();
        }

        public void onInterstitialShown(AndroidJavaObject ad)
        {
            interstitialAd.OnShown();
        }
    }
    #endif
}
