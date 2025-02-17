﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WortiseAppOpen
{
    public enum AdOrientation
    {
        Landscape,
        Portrait
    }


    #if UNITY_ANDROID
    private static AndroidJavaObject activity
    {
        get
        {
            return WortiseSdk.activity;
        }
    }

    private AndroidJavaObject appOpenAd;
    #endif

    public bool AutoReload
    {
        get
        {
            #if UNITY_ANDROID
            return appOpenAd.Call<bool>("getAutoReload");
            #else
            return false;
            #endif
        }

        set
        {
            #if UNITY_ANDROID
            appOpenAd.Call("setAutoReload", value);
            #endif
        }
    }

    public bool IsAvailable
    {
        get
        {
            #if UNITY_ANDROID
            return appOpenAd.Call<bool>("isAvailable");
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
            return appOpenAd.Call<bool>("isDestroyed");
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
            return appOpenAd.Call<bool>("isShowing");
            #else
            return false;
            #endif
        }
    }


    public event Action OnClicked;
    public event Action OnDismissed;
    public event Action OnFailedToLoad;
    public event Action onFailedToShow;
    public event Action OnImpression;
    public event Action OnLoaded;
    public event Action OnShown;


    public WortiseAppOpen(string adUnitId) {
        appOpenAd = new AndroidJavaObject("com.wortise.ads.appopen.AppOpenAd", activity, adUnitId);
        appOpenAd.Call("setListener", new AppOpenAdListener(this));
    }
    
    public void Destroy()
    {
        #if UNITY_ANDROID
        appOpenAd.Call("destroy");
        #endif
    }
    
    public void LoadAd(string adUnitId)
    {
        #if UNITY_ANDROID
        appOpenAd.Call("loadAd");
        #endif
    }
    
    public void ShowAd()
    {
        #if UNITY_ANDROID
        if (activity != null) {
            appOpenAd.Call("showAd", activity);
        }
        #endif
    }

    public void TryToShowAd()
    {
        #if UNITY_ANDROID
        appOpenAd.Call("tryToShowAd", activity);
        #endif
    }
    
    
    #if UNITY_ANDROID
    class AppOpenAdListener : AndroidJavaProxy
    {
        private WortiseAppOpen appOpenAd;


        public AppOpenAdListener(WortiseAppOpen appOpenAd) : base("com.wortise.ads.appopen.AppOpenAd$Listener")
        {
            this.appOpenAd = appOpenAd;
        }
        
        public void onAppOpenClicked(AndroidJavaObject ad)
        {
            if (appOpenAd.OnClicked != null) {
                appOpenAd.OnClicked();
            }
        }

        public void onAppOpenDismissed(AndroidJavaObject ad)
        {
            if (appOpenAd.OnDismissed != null) {
                appOpenAd.OnDismissed();
            }
        }

        public void onAppOpenFailedToLoad(AndroidJavaObject ad, AndroidJavaObject error)
        {
            if (appOpenAd.OnFailedToLoad != null) {
                appOpenAd.OnFailedToLoad();
            }
        }

        public void onAppOpenFailedToShow(AndroidJavaObject ad, AndroidJavaObject error)
        {
            if (appOpenAd.onFailedToShow != null) {
                appOpenAd.onFailedToShow();
            }
        }

        public void onAppOpenImpression(AndroidJavaObject ad)
        {
            if (appOpenAd.OnImpression != null) {
                appOpenAd.OnImpression();
            }
        }

        public void onAppOpenLoaded(AndroidJavaObject ad)
        {
            if (appOpenAd.OnLoaded != null) {
                appOpenAd.OnLoaded();
            }
        }

        public void onAppOpenShown(AndroidJavaObject ad)
        {
            if (appOpenAd.OnShown != null) {
                appOpenAd.OnShown();
            }
        }
    }
    #endif
}
