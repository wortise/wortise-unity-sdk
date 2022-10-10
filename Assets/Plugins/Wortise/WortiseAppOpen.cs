using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WortiseAppOpen : MonoBehaviour
{
    #if UNITY_ANDROID
    private static AndroidJavaObject activity
    {
        get
        {
            return WortiseSdk.activity;
        }
    }
    
    private static AndroidJavaObject appOpenAd;
    #endif
    
    private static string currentAdUnitId;
        
    public static bool IsAvailable
    {
        get
        {
            #if UNITY_ANDROID
            if (appOpenAd != null) {
                return appOpenAd.Call<bool>("isAvailable");
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
            if (appOpenAd != null) {
                return appOpenAd.Call<bool>("isDestroyed");
            }
            #endif
            
            return false;
        }
    }

    public static bool IsShowing
    {
        get
        {
            #if UNITY_ANDROID
            if (appOpenAd != null) {
                return appOpenAd.Call<bool>("isShowing");
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
        if (appOpenAd != null) {
            appOpenAd.Call("destroy");
            appOpenAd = null;
        }
        #endif
    }
    
    public static void LoadAd(string adUnitId)
    {
        #if UNITY_ANDROID
        bool canLoad = (currentAdUnitId != adUnitId) || (appOpenAd == null);

        if (activity != null && canLoad) {
            appOpenAd = new AndroidJavaObject("com.wortise.ads.appopen.AppOpenAd", activity, adUnitId);
            
            appOpenAd.Call("setListener", new AppOpenAdListener());
            appOpenAd.Call("loadAd");
            
            currentAdUnitId = adUnitId;
        }
        #endif
    }
    
    public static bool ShowAd()
    {
        #if UNITY_ANDROID
        if (appOpenAd != null) {
            appOpenAd.Call<bool>("showAd", activity);
        }
        #endif
        
        return false;
    }

    public static bool TryToShowAd()
    {
        #if UNITY_ANDROID
        if (appOpenAd != null) {
            appOpenAd.Call<bool>("tryToShowAd", activity);
        }
        #endif
        
        return false;
    }
    
    
    #if UNITY_ANDROID
    class AppOpenAdListener : AndroidJavaProxy
    {
        public AppOpenAdListener() : base("com.wortise.ads.appopen.AppOpenAd$Listener")
        {
        }
        
        public void onAppOpenClicked(AndroidJavaObject ad)
        {
            OnClicked();
        }

        public void onAppOpenDismissed(AndroidJavaObject ad)
        {
            OnDismissed();
        }

        public void onAppOpenFailed(AndroidJavaObject ad, AndroidJavaObject error)
        {
            OnFailed();
        }

        public void onAppOpenLoaded(AndroidJavaObject ad)
        {
            OnLoaded();
        }

        public void onAppOpenShown(AndroidJavaObject ad)
        {
            OnShown();
        }
    }
    #endif
}
