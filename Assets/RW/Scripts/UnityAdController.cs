using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdController : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{

    [SerializeField] string _iosGameId;
    [SerializeField] string _androidgameId;

    [SerializeField] string _androidAdUnitId = "Interstitial_Android";
    [SerializeField] string _iOsAdUnitId = "Interstitial_iOS";

    string _gameId;
    string _adUnitId;
    bool TestMode = true;

    bool isShowAd = false;

    void Awake()
    {
        InitializeAd();
        _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer ? _iOsAdUnitId : _androidAdUnitId);
    }


    //------------------------------Initialsing ad-----------------------------------------------------------------

    void InitializeAd()
    {
        _gameId = (Application.platform == RuntimePlatform.IPhonePlayer ? _iosGameId : _androidgameId);
        Debug.Log("Initialsing Ad : " + _gameId);
        Advertisement.Initialize(_gameId, TestMode, this);

    }

    //------------------------------Initialisation Interface Methods Implementation----------------------------------

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        //LoadAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }


    //--------------------------------LoAding Ad-------------------------------------------------------

    void LoadAd()
    {
        Debug.Log("Loading Ad : " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }

    //------------------------------Load Interface Methods Implementation----------------------------------

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        // Optionally execute code if the Ad Unit successfully loads content.

    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
    }


    //--------------------------Showing Ad-----------------------------------------------------

    void ShowAd()
    {
        Debug.Log("Showing Ad : " + _adUnitId);
        Advertisement.Show(_adUnitId, this);
    }

    //------------------------------Show Interface Methods Implementation----------------------------------

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState) { }

    IEnumerator AdRepeater()
    {
        while (true)
        {
            yield return new WaitForSeconds(20);

            isShowAd = !isShowAd;

            if (isShowAd)
            {

                LoadAd();
                ShowAd();
            }

        }
    }

    void Start()
    {

        StartCoroutine(AdRepeater());

    }



}
