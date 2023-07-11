using System;
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
    
    private static AndroidJavaClass appOpenOrientation;

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

    public AdOrientation? Orientation
    {
        get
        {
            #if UNITY_ANDROID
            AndroidJavaObject obj = appOpenAd.Call<AndroidJavaObject>("getOrientation");

            if (obj == null) {
                return null;
            }

            string name = obj.Call<string>("name");

            AdOrientation orientation;

            Enum.TryParse(name, true, out orientation);

            return orientation;
            #else
            return null;
            #endif
        }

        set
        {
            #if UNITY_ANDROID
            if (value == null) {
                return;
            }

            string name = value.ToString();

            AndroidJavaObject obj = appOpenOrientation.CallStatic<AndroidJavaObject>("valueOf", name.ToUpper());

            appOpenAd.Call("setOrientation", obj);
            #endif
        }
    }

    public event Action OnClicked;
    public event Action OnDismissed;
    public event Action OnFailed;
    public event Action OnLoaded;
    public event Action OnShown;


    static WortiseAppOpen()
    {
        #if UNITY_ANDROID
        appOpenOrientation = new AndroidJavaClass("com.wortise.ads.appopen.AppOpenAd$Orientation");
        #endif
    }

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
    
    public bool ShowAd()
    {
        #if UNITY_ANDROID
        return appOpenAd.Call<bool>("showAd", activity);
        #else
        return false;
        #endif
    }

    public bool TryToShowAd()
    {
        #if UNITY_ANDROID
        return appOpenAd.Call<bool>("tryToShowAd", activity);
        #else
        return false;
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

        public void onAppOpenFailed(AndroidJavaObject ad, AndroidJavaObject error)
        {
            if (appOpenAd.OnFailed != null) {
                appOpenAd.OnFailed();
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
