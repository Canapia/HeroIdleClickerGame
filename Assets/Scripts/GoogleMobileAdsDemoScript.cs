using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class GoogleMobileAdsDemoScript : MonoBehaviour
{
    private BannerView bannerView;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_ANDROID
        //string appId = "ca-app-pub-3940256099942544~3347511713";        // test ID
        string appId = "ca-app-pub-6488858218587027~3142930947";      // 실제 ID
#elif UNITY_IPHONE
            string appId = "ca-app-pub-3940256099942544~1458002511";
#else
            string appId = "unexpected_platform";
#endif

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);

        this.RequestBanner();
    }

    private void RequestBanner()
    {
#if UNITY_ANDROID
        //string adUnitId = "ca-app-pub-3940256099942544/6300978111";     // test ID
        string adUnitId = "ca-app-pub-6488858218587027/3213075523";   // 실제 ID
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
            string adUnitId = "unexpected_platform";
#endif

        //// Clean up Banner ad before creating a new one.
        //if(this.bannerView != null)
        //{
        //    this.bannerView.Destroy();
        //}

        //AdSize adaptiveSize =
        //    AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);

        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);

        //Register for ad events.
        //this.bannerView.OnAdLoaded += this.HandleAdLoaded;
        //this.bannerView.OnAdFailedToLoad += this.HandleAdFailedToLoad;
        //this.bannerView.OnAdOpening += this.HandleAdOpened;
        //this.bannerView.OnAdClosed += this.HandleAdClosed;
        //this.bannerView.OnAdLeavingApplication += this.HandleAdLeftApplication;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        //AdRequest request = new AdRequest.Builder()
        //    .AddTestDevice(AdRequest.TestDeviceSimulator)
        //    .AddTestDevice("0123456789ABCDEF0123456789ABCDEF")
        //    .Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);
    }

    #region Banner callback handlers

    public void HandleAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
        MonoBehaviour.print(String.Format("Ad Height: {0}, width: {1}",
            this.bannerView.GetHeightInPixels(),
            this.bannerView.GetWidthInPixels()));
    }

    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print(
                "HandleFailedToReceiveAd event received with message: " + args.Message);
    }

    public void HandleAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
    }

    public void HandleAdLeftApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeftApplication event received");
    }

    #endregion
}
