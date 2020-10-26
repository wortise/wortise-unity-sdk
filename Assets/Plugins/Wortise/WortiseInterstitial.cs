using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WortiseInterstitial : MonoBehaviour
{
    #if UNITY_ANDROID
    private static AndroidJavaObject activity
    {
        get
        {
            return WortiseSdk.activity;
        }
    }
    
    private static AndroidJavaObject interstitialAd;
    #endif
    
    private static string currentAdUnitId;
        
    public static bool IsAvailable
    {
        get
        {
            #if UNITY_ANDROID
            if (interstitialAd != null) {
                return interstitialAd.Call<bool>("isAvailable");
            }
            #endif
            
            return false;
        }
    }
    
    public static bool IsDestroyed
    {
        get
        {
            #if UNITY_ANDROID
            if (interstitialAd != null) {
                return interstitialAd.Call<bool>("isDestroyed");
            }
            #endif
            
            return false;
        }
    }

    public static bool IsLoaded
    {
        get
        {
            #if UNITY_ANDROID
            if (interstitialAd != null) {
                return interstitialAd.Call<bool>("isLoaded");
            }
            #endif
            
            return false;
        }
    }

    public static bool IsLoading
    {
        get
        {
            #if UNITY_ANDROID
            if (interstitialAd != null) {
                return interstitialAd.Call<bool>("isLoading");
            }
            #endif
            
            return false;
        }
    }
    
    public static event Action OnClicked;
    public static event Action OnDismissed;
    public static event Action OnFailed;
    public static event Action OnLoaded;
    public static event Action OnShown;

    
    public static void Destroy()
    {
        #if UNITY_ANDROID
        if (interstitialAd != null) {
            interstitialAd.Call("destroy");
            interstitialAd = null;
        }
        #endif
    }
    
    public static void LoadAd(string adUnitId)
    {
        #if UNITY_ANDROID
        bool canLoad = (currentAdUnitId != adUnitId) || (interstitialAd == null);

        if (activity != null && canLoad) {
            interstitialAd = new AndroidJavaObject("com.wortise.ads.interstitial.InterstitialAd", activity, adUnitId);
            
            interstitialAd.Call("setListener", new InterstitialAdListener());
            interstitialAd.Call("loadAd");
            
            currentAdUnitId = adUnitId;
        }
        #endif
    }
    
    public static bool ShowAd()
    {
        #if UNITY_ANDROID
        if (interstitialAd != null) {
            interstitialAd.Call<bool>("showAd");
        }
        #endif
        
        return false;
    }
    
    
    #if UNITY_ANDROID
    class InterstitialAdListener : AndroidJavaProxy
    {
        public InterstitialAdListener() : base("com.wortise.ads.interstitial.InterstitialAd$Listener")
        {
        }
        
        public void onInterstitialClicked(AndroidJavaObject ad)
        {
            OnClicked();
        }

        public void onInterstitialDismissed(AndroidJavaObject ad)
        {
            OnDismissed();
        }

        public void onInterstitialFailed(AndroidJavaObject ad, AndroidJavaObject error)
        {
            OnFailed();
        }

        public void onInterstitialLoaded(AndroidJavaObject ad)
        {
            OnLoaded();
        }

        public void onInterstitialShown(AndroidJavaObject ad)
        {
            OnShown();
        }
    }
    #endif
}
