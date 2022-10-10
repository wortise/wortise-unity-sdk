using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WortiseRewarded : MonoBehaviour
{
    #if UNITY_ANDROID
    private static AndroidJavaObject activity
    {
        get
        {
            return WortiseSdk.activity;
        }
    }
    
    private static AndroidJavaObject rewardedAd;
    #endif
    
    private static string currentAdUnitId;
        
    public static bool IsAvailable
    {
        get
        {
            #if UNITY_ANDROID
            if (rewardedAd != null) {
                return rewardedAd.Call<bool>("isAvailable");
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
            if (rewardedAd != null) {
                return rewardedAd.Call<bool>("isDestroyed");
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
            if (rewardedAd != null) {
                return rewardedAd.Call<bool>("isShowing");
            }
            #endif
            
            return false;
        }
    }
    
    public static event Action OnClicked;
    public static event Action<WortiseReward> OnCompleted;
    public static event Action OnDismissed;
    public static event Action OnFailed;
    public static event Action OnLoaded;
    public static event Action OnShown;

    
    public static void Destroy()
    {
        #if UNITY_ANDROID
        if (rewardedAd != null) {
            rewardedAd.Call("destroy");
            rewardedAd = null;
        }
        #endif
    }
    
    public static void LoadAd(string adUnitId)
    {
        #if UNITY_ANDROID
        bool canLoad = (currentAdUnitId != adUnitId) || (rewardedAd == null);

        if (activity != null && canLoad) {
            rewardedAd = new AndroidJavaObject("com.wortise.ads.rewarded.RewardedAd", activity, adUnitId);
            
            rewardedAd.Call("setListener", new RewardedAdListener());
            rewardedAd.Call("loadAd");
            
            currentAdUnitId = adUnitId;
        }
        #endif
    }
    
    public static bool ShowAd()
    {
        #if UNITY_ANDROID
        if (rewardedAd != null) {
            rewardedAd.Call<bool>("showAd");
        }
        #endif
        
        return false;
    }
    
    
    #if UNITY_ANDROID
    class RewardedAdListener : AndroidJavaProxy
    {
        public RewardedAdListener() : base("com.wortise.ads.rewarded.RewardedAd$Listener")
        {
        }
        
        public void onRewardedClicked(AndroidJavaObject ad)
        {
            OnClicked();
        }

        public void onRewardedCompleted(AndroidJavaObject ad, AndroidJavaObject reward)
        {
            if (reward == null) {
                return;
            }

            int    amount  = reward.Call<int>   ("getAmount");
            string label   = reward.Call<string>("getLabel");
            bool   success = reward.Call<bool>  ("getSuccess");

            WortiseReward r = new WortiseReward(success, label, amount);

            OnCompleted(r);
        }

        public void onRewardedDismissed(AndroidJavaObject ad)
        {
            OnDismissed();
        }

        public void onRewardedFailed(AndroidJavaObject ad, AndroidJavaObject error)
        {
            OnFailed();
        }

        public void onRewardedLoaded(AndroidJavaObject ad)
        {
            OnLoaded();
        }

        public void onRewardedShown(AndroidJavaObject ad)
        {
            OnShown();
        }
    }
    #endif
}
