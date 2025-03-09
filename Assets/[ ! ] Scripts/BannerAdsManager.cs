using UnityEngine;
using UnityEngine.SceneManagement;
using com.unity3d.mediation;

public class BannerAdsManager : MonoBehaviour
{
    public static BannerAdsManager instance;

    [SerializeField] private string appKey = "demoAppKey";
    [SerializeField] private string bannerAdUnitId = "defaultBannerAdUnitId";
    [SerializeField] private GameObject fakeBanner;
    private bool adsRemoved = false;
    private LevelPlayBannerAd bannerAd;

    private void Awake()
    {
        if (instance == null)
        {
            IronSource.Agent.setMetaData("is_test_suite", "true");
            IronSource.Agent.setAdaptersDebug(true);
            instance = this;
            CheckAdStatus();
            InitializeAds();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void CheckAdStatus()
    {
        adsRemoved = PlayerPrefs.GetInt("AdsRemoved", 0) == 1;
        if (fakeBanner != null)
            fakeBanner.SetActive(!adsRemoved);
    }

    void InitializeAds()
    {
        if (adsRemoved) return;
        if (SceneManager.GetActiveScene().name == "Gameplay")
            return;

        IronSource.Agent.init(appKey);
        Debug.Log("IronSource Initialized with App Key: " + appKey);

        bannerAd = new LevelPlayBannerAd(
            bannerAdUnitId,
            LevelPlayAdSize.BANNER,
            LevelPlayBannerPosition.TopCenter,
            "DefaultPlacement",
            false
        );

        bannerAd.OnAdLoaded += ShowBannerAd;
        bannerAd.OnAdLoadFailed += OnBannerLoadFailed;
        bannerAd.LoadAd();
        Debug.Log("Banner Ad Requested with ID: " + bannerAdUnitId);
    }

    private void ShowBannerAd(LevelPlayAdInfo adInfo)
    {
        if (adsRemoved) return;
        Debug.Log("Banner ad successfully loaded.");
        bannerAd.ShowAd();
    }

    private void OnBannerLoadFailed(LevelPlayAdError error)
    {
        Debug.LogError("Failed to load banner ad: " + error.ToString());
    }

    public void ShowBannerAds()
    {
        if (adsRemoved || SceneManager.GetActiveScene().name == "Gameplay") return;
        bannerAd.ShowAd();
        Debug.Log("Banner ad displayed.");
    }

    public void RemoveAds()
    {
        adsRemoved = true;
        PlayerPrefs.SetInt("AdsRemoved", 1);
        PlayerPrefs.Save();
        if (bannerAd != null)
        {
            bannerAd.HideAd();
        }
        if (fakeBanner != null)
        {
            fakeBanner.SetActive(false);
        }
        Debug.Log("Ads removed and hidden.");
    }
}