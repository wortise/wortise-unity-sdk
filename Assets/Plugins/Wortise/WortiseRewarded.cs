using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WortiseRewarded
{
    #if UNITY_ANDROID
    private static AndroidJavaObject activity
    {
        get
        {
            return WortiseSdk.activity;
        }
    }
    
    private AndroidJavaObject rewardedAd;
    #endif
            
    public bool IsAvailable
    {
        get
        {
            #if UNITY_ANDROID
            return rewardedAd.Call<bool>("isAvailable");
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
            return rewardedAd.Call<bool>("isDestroyed");
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
            return rewardedAd.Call<bool>("isShowing");
            #else
            return false;
            #endif
        }
    }
    
    public event Action OnClicked;
    public event Action<WortiseReward> OnCompleted;
    public event Action OnDismissed;
    public event Action OnFailed;
    public event Action OnLoaded;
    public event Action OnShown;

    
    public WortiseRewarded(string adUnitId)
    {
        rewardedAd = new AndroidJavaObject("com.wortise.ads.rewarded.RewardedAd", activity, adUnitId);
        rewardedAd.Call("setListener", new RewardedAdListener(this));
    }

    public void Destroy()
    {
        #if UNITY_ANDROID
        rewardedAd.Call("destroy");
        #endif
    }
    
    public void LoadAd()
    {
        #if UNITY_ANDROID
        rewardedAd.Call("loadAd");
        #endif
    }
    
    public bool ShowAd()
    {
        #if UNITY_ANDROID
        return rewardedAd.Call<bool>("showAd");
        #else
        return false;
        #endif
    }
    
    
    #if UNITY_ANDROID
    class RewardedAdListener : AndroidJavaProxy
    {
        private WortiseRewarded rewardedAd;


        public RewardedAdListener(WortiseRewarded rewardedAd) : base("com.wortise.ads.rewarded.RewardedAd$Listener")
        {
            this.rewardedAd = rewardedAd;
        }
        
        public void onRewardedClicked(AndroidJavaObject ad)
        {
            rewardedAd.OnClicked();
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

            rewardedAd.OnCompleted(r);
        }

        public void onRewardedDismissed(AndroidJavaObject ad)
        {
            rewardedAd.OnDismissed();
        }

        public void onRewardedFailed(AndroidJavaObject ad, AndroidJavaObject error)
        {
            rewardedAd.OnFailed();
        }

        public void onRewardedLoaded(AndroidJavaObject ad)
        {
            rewardedAd.OnLoaded();
        }

        public void onRewardedShown(AndroidJavaObject ad)
        {
            rewardedAd.OnShown();
        }
    }
    #endif
}
