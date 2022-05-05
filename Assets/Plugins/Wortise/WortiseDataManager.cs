using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WortiseDataManager : MonoBehaviour
{
    #if UNITY_ANDROID
    private static AndroidJavaObject activity
    {
        get
        {
            return WortiseSdk.activity;
        }
    }

    private static AndroidJavaClass dataManager;
    private static AndroidJavaClass userGender;
    #endif
    
    public static int? Age
    {
        get
        {
            #if UNITY_ANDROID
            if (activity != null) {
                AndroidJavaObject obj = dataManager.CallStatic<AndroidJavaObject>("getAge", activity);

                if (obj == null) {
                    return null;
                }

                return obj.Call<int>("intValue");
            }
            #endif
            
            return null;
        }

        set
        {
            #if UNITY_ANDROID
            if (activity != null) {
                dataManager.CallStatic("setAge", activity, value);
            }
            #endif
        }
    }

    public static List<string> Emails
    {
        get
        {
            List<string> list = new List<string>();

            #if UNITY_ANDROID
            if (activity != null) {
                AndroidJavaObject obj = dataManager.CallStatic<AndroidJavaObject>("getEmails", activity);

                int size = obj.Call<int>("size");

                for (int i = 0; i < size; i++) {
                    list.Add(obj.Call<string>("get", i));
                }
            }
            #endif

            return list;
        }

        set
        {
            #if UNITY_ANDROID
            if (activity != null) {
                AndroidJavaObject obj = new AndroidJavaObject("java.util.ArrayList");

                value.ForEach(delegate(string email) {
                    obj.Call<bool>("add", email);
                });
                
                dataManager.CallStatic("setEmails", activity, obj);
            }
            #endif
        }
    }

    public static WortiseUserGender? Gender
    {
        get
        {
            #if UNITY_ANDROID
            if (activity != null) {
                AndroidJavaObject obj = dataManager.CallStatic<AndroidJavaObject>("getGender", activity);

                if (obj == null) {
                    return null;
                }

                string name = obj.Call<string>("name");

                WortiseUserGender gender;

                Enum.TryParse(name, true, out gender);

                return gender;
            }
            #endif
            
            return null;
        }

        set
        {
            #if UNITY_ANDROID
            if (activity != null) {
                string name = value.ToString();

                AndroidJavaObject obj = userGender.CallStatic<AndroidJavaObject>("valueOf", name.ToUpper());

                dataManager.CallStatic("setGender", activity, obj);
            }
            #endif
        }
    }
    
    
    static WortiseDataManager()
    {
        #if UNITY_ANDROID
        dataManager = new AndroidJavaClass("com.wortise.ads.data.DataManager");
        userGender  = new AndroidJavaClass("com.wortise.ads.user.UserGender");
        #endif
    }

    public static void AddEmail(string email)
    {
        #if UNITY_ANDROID
        if (activity != null) {
            dataManager.CallStatic("addEmail", activity, email);
        }
        #endif
    }

    public static bool RequestAccount()
    {
        return RequestAccount(true);
    }

    public static bool RequestAccount(bool onlyIfNotAvailable)
    {
        #if UNITY_ANDROID
        if (activity != null) {
            return dataManager.CallStatic<bool>("requestAccount", activity, onlyIfNotAvailable);
        }
        #endif

        return false;
    }
}
