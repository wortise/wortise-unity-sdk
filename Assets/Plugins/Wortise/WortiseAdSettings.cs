using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WortiseAdSettings : MonoBehaviour
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
    
    private static AndroidJavaClass adContentRating;
    private static AndroidJavaClass adSettings;
    #endif
    
    public static string AssetKey
    {
        get
        {
            #if UNITY_ANDROID
            if (activity != null) {
                return adSettings.CallStatic<string>("getAssetKey", activity);
            }
            #endif

            return null;
        }
    }

    public static bool IsChildDirected
    {
        get
        {
            #if UNITY_ANDROID
            if (activity != null) {
                return adSettings.CallStatic<bool>("isChildDirected", activity);
            }
            #endif

            return false;
        }

        set
        {
            #if UNITY_ANDROID
            if (activity != null) {
                adSettings.CallStatic("setChildDirected", activity, value);
            }
            #endif
        }
    }

    public static WortiseAdContentRating? MaxAdContentRating
    {
        get
        {
            #if UNITY_ANDROID
            if (activity != null) {
                AndroidJavaObject obj = adSettings.CallStatic<AndroidJavaObject>("getMaxAdContentRating", activity);

                if (obj == null) {
                    return null;
                }

                string name = obj.Call<string>("name");

                WortiseAdContentRating rating;

                Enum.TryParse(name, true, out rating);

                return rating;
            }
            #endif
            
            return null;
        }

        set
        {
            #if UNITY_ANDROID
            if (activity != null) {
                string name = value.ToString();

                AndroidJavaObject obj = adContentRating.CallStatic<AndroidJavaObject>("valueOf", name.ToUpper());

                adSettings.CallStatic("setMaxAdContentRating", activity, obj);
            }
            #endif
        }
    }


    static WortiseAdSettings()
    {
        #if UNITY_ANDROID
        adContentRating = new AndroidJavaClass("com.wortise.ads.AdContentRating");
        adSettings      = new AndroidJavaClass("com.wortise.ads.AdSettings");
        #endif
    }
}
