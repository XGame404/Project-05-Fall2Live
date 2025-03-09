using UnityEngine;
using UnityEngine.Purchasing;
using System.Collections;
using UnityEngine.UI;

public class BannerAdsButton : MonoBehaviour
{
    [SerializeField] private GameObject removeAdsButton;
    public Text priceText;
    private void Start()
    {
        if (CodelessIAPStoreListener.Instance != null && CodelessIAPStoreListener.Instance.StoreController != null)
        {
            var product = CodelessIAPStoreListener.Instance.StoreController.products.WithID("com.OutbreakCompany.Fall2Live.RemoveAds");

            if (product != null)
            {
                priceText.text = product.metadata.localizedPriceString;
            }
            else
            {
                priceText.text = "N/A";
            }
            if (removeAdsButton != null)
                removeAdsButton.SetActive(PlayerPrefs.GetInt("AdsRemoved", 0) == 0);
        }
    }

    public void OnPurchaseComplete(Product product)
    {
        if (product.definition.id == "com.OutbreakCompany.Fall2Live.RemoveAds")
        {
            StartCoroutine(ProcessRemoveAdsCoroutine());
        }
    }

    private IEnumerator ProcessRemoveAdsCoroutine()
    {
        yield return new WaitForEndOfFrame();
        BannerAdsManager.instance.RemoveAds();
        if (removeAdsButton != null)
            removeAdsButton.SetActive(false);
        Debug.Log("Ads removed!");
    }
}
