using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;

public class GoogleRewardAd : MonoBehaviour
{

    string _adUnitId;
    public Button showBtn;
    private RewardedAd rewardedAd;
    public Image RewardImg;
    [SerializeField] MouseController coinRef;
    [SerializeField] Image adLimitDialog;

    int rewardAdLimit;
    bool check = false;

    void Start()
    {
        //MobileAds.Initialize((InitializationStatus initStatus) =>
        //{

        //});

        LoadRewardAd();

        showBtn.onClick.AddListener(showRewardedAd);

        rewardAdLimit = 3;
    }
    private void Update()
    {
        if (check)
        {
            check = false;
            RewardImg.gameObject.SetActive(true);
            Debug.Log("RewardScreenShown");
            Invoke(nameof(HideRewardImg), 1);
            Debug.Log("Invoke Called");

        }
    }
    void LoadRewardAd()
    {
#if UNITY_ANDROID
        _adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
           _adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
        _adUnitId = "unused";
#endif

        //if (rewardedAd != null)
        //{
        //    rewardedAd.Destroy();
        //    rewardedAd = null;
        //}

        AdRequest adRequest = new AdRequest.Builder().Build();

        RewardedAd.Load(_adUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {

                Debug.LogError("Rewarded ad failed to load an ad " +
                               "with error : " + error);
                return;
            }

            Debug.Log("Rewarded ad loaded with response : "
                        + ad.GetResponseInfo());

            rewardedAd = ad;
            RegisterEventHandlers(rewardedAd);

        });

    }

    public void showRewardedAd()
    {
        if (rewardAdLimit > 0)
        {
            coinRef.check = true;
            coinRef.RestartDialog.gameObject.SetActive(false);

            const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

            if (rewardedAd != null && rewardedAd.CanShowAd())
            {
                rewardedAd.Show((Reward reward) =>
                {
                    Debug.Log(string.Format(rewardMsg, reward.Type, reward.Amount));
                });
            }
        }
        else
        {
            adLimitDialog.gameObject.SetActive(true);
            Invoke(nameof(HideadLimitDialog), 1f);
        }

    }

    void HideadLimitDialog()
    {
        adLimitDialog.gameObject.SetActive(false);
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(string.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));


        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");

        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
            check = true;

        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

    void HideRewardImg()
    {
        Debug.Log("Inside Invoke");
        if (rewardAdLimit > 0)
        {
            Debug.Log("Reward Resulted");
            RewardImg.gameObject.SetActive(false);
            coinRef.coins += 10;
            coinRef.coinsCollectedLabel.text = coinRef.coins.ToString();
            coinRef.isDead = !coinRef.isDead;
            coinRef.mouseAnimator.SetBool("isDead", coinRef.isDead);
            coinRef.mouseAnimator.SetTrigger("dieOnceTrigger");
            coinRef.check = false;
            LoadRewardAd();
            rewardAdLimit -= 1;
            Debug.Log("Loading Reward Ad Again After Watching First");
        }



    }

    //void OnDestroy()
    //{
    //    showBtn.onClick.RemoveAllListeners();
    //}





}
