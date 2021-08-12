using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WortiseConsentManager : MonoBehaviour
{
    #if UNITY_ANDROID
    private static AndroidJavaObject activity
    {
        get
        {
            return WortiseSdk.activity;
        }
    }
    
    private static AndroidJavaClass consentActivity;
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

    public static bool IsGranted
    {
        get
        {
            #if UNITY_ANDROID
            if (activity != null) {
                return consentManager.CallStatic<bool>("isGranted", activity);
            }
            #endif
            
            return false;
        }
    }

    public static bool IsReplied
    {
        get
        {
            #if UNITY_ANDROID
            if (activity != null) {
                return consentManager.CallStatic<bool>("isReplied", activity);
            }
            #endif
            
            return false;
        }
    }
    
    public static bool IsRequired
    {
        get
        {
            #if UNITY_ANDROID
            return consentManager.CallStatic<bool>("isRequired");
            #else
            return false;
            #endif
        }
    }
    
    
    static WortiseConsentManager()
    {
        #if UNITY_ANDROID
        consentActivity = new AndroidJavaClass("com.wortise.ads.consent.ConsentActivity");
        consentManager  = new AndroidJavaClass("com.wortise.ads.consent.ConsentManager");
        #endif
    }
    
    public static bool Request(bool withOptOut = false)
    {
        #if UNITY_ANDROID
        if (activity != null) {
            return consentActivity.CallStatic<bool>("request", activity, withOptOut);
        }
        #endif

        return false;
    }
    
    public static bool RequestOnce(bool withOptOut = false)
    {
        #if UNITY_ANDROID
        if (activity != null) {
            return consentActivity.CallStatic<bool>("requestOnce", activity, withOptOut);
        }
        #endif

        return false;
    }

    public static void Set(bool granted)
    {
        #if UNITY_ANDROID
        if (activity != null) {
            consentManager.CallStatic("set", activity, granted);
        }
        #endif
    }

    public static void SetIabString(string value)
    {
        #if UNITY_ANDROID
        if (activity != null) {
            consentManager.CallStatic("setIabString", activity, value);
        }
        #endif
    }
}
