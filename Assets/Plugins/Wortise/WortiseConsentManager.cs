using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WortiseConsentManager
{
    #if UNITY_ANDROID
    private static AndroidJavaObject activity
    {
        get
        {
            return WortiseSdk.activity;
        }
    }
    
    private static AndroidJavaObject consentManager;
    #endif
    
    public static bool CanCollectData
    {
        get
        {
            #if UNITY_ANDROID
            if (activity != null) {
                return consentManager.CallStatic<bool>("canCollectData", activity);
            }
            #endif
            
            return false;
        }
    }

    public static bool Exists
    {
        get
        {
            #if UNITY_ANDROID
            if (activity != null) {
                return consentManager.CallStatic<bool>("exists", activity);
            }
            #endif
            
            return false;
        }
    }
    
    
    static WortiseConsentManager()
    {
        #if UNITY_ANDROID
        consentManager = new AndroidJavaClass("com.wortise.ads.consent.ConsentManager");
        #endif
    }
    
    public static void Request()
    {
        #if UNITY_ANDROID
        if (activity != null) {
            consentManager.CallStatic("request", activity);
        }
        #endif
    }
    
    public static void RequestIfRequired()
    {
        #if UNITY_ANDROID
        if (activity != null) {
            consentManager.CallStatic("requestIfRequired", activity);
        }
        #endif
    }
}
