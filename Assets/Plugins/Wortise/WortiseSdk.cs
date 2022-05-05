using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WortiseSdk : MonoBehaviour
{
    #if UNITY_ANDROID
    public static AndroidJavaObject activity
    {
        get
        {
            AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            return playerClass.GetStatic<AndroidJavaObject>("currentActivity");
        }
    }
    
    private static AndroidJavaClass wortiseSdk;
    #endif
    
    public static bool IsInitialized
    {
        get
        {
            #if UNITY_ANDROID
            return wortiseSdk.CallStatic<bool>("isInitialized");
            #else
            return false;
            #endif
        }
    }

    public static bool IsReady
    {
        get
        {
            #if UNITY_ANDROID
            return wortiseSdk.CallStatic<bool>("isReady");
            #else
            return false;
            #endif
        }
    }

    public static event Action OnInitialized;

    public static bool Version
    {
        get
        {
            #if UNITY_ANDROID
            return wortiseSdk.CallStatic<bool>("getVersion");
            #else
            return false;
            #endif
        }
    }


    static WortiseSdk()
    {
        #if UNITY_ANDROID
        wortiseSdk = new AndroidJavaClass("com.wortise.ads.WortiseSdk");
        #endif
    }

    public static void Initialize(string assetKey, bool start = true)
    {
        #if UNITY_ANDROID
        if (activity != null) {
            wortiseSdk.CallStatic("initialize", activity, assetKey, start, new SdkInitializationListener());
        }
        #endif
    }
    
    public static void Start()
    {
        #if UNITY_ANDROID
        if (activity != null) {
            wortiseSdk.CallStatic("start", activity);
        }
        #endif
    }

    public static void Stop()
    {
        #if UNITY_ANDROID
        if (activity != null) {
            wortiseSdk.CallStatic("stop", activity);
        }
        #endif
    }


    #if UNITY_ANDROID
    class SdkInitializationListener : AndroidJavaProxy
    {
        public SdkInitializationListener() : base("kotlin.jvm.functions.Function0")
        {
        }
        
        public AndroidJavaObject invoke()
        {
            OnInitialized();
            return null;
        }
    }
    #endif
}
