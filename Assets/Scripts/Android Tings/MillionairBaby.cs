using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
public class MillionairBaby : MonoBehaviour, IStoreListener
{

    private static IStoreController storeCtrl;
    private static IExtensionProvider storeExtProvider;

    //Purchase IDS
    [HideInInspector]
    public string removeAds = "removeads";

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        //Are we already initialised return
        if (!IsInitialized())
        {

            //Debug.Log("Initialising IAP...");

            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            //Add products
            builder.AddProduct(removeAds, ProductType.NonConsumable);

            UnityPurchasing.Initialize(this, builder);
        }
    }

    public void Purchase(string ID)
    {
        if (IsInitialized()) {
            Product product = storeCtrl.products.WithID(ID);
            if (product != null && product.availableToPurchase)
            {
                //Debug.Log("Purchasing Product...");
                storeCtrl.InitiatePurchase(ID);
            }
            else
            {
                Debug.LogWarning("Attempted to purchase an item but it either doesn't exists or is not available to be purchased");
            }
        }
        else
        {
            Debug.LogWarning("Attempted to purchase an item but IAP is not initalised");
        }
    }

    

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        //Debug.Log("IAP OnInitializeFailed InitializationFailureReason:" + error);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {

        switch (e.purchasedProduct.definition.id.ToString())
        {
            case "removeads":
                {
                    PlayerPrefs.SetInt("noAdsPurchased", 1);
                    //Debug.Log("Brought remove ads");
                    break;
                }

            default:
                {
                    Debug.LogError("Attempted to purchase item but there is no behaviour set up for when brought");
                    break;
                }
        }

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
    {
        Debug.LogWarning("Purchase Failed");
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // Purchasing has succeeded initializing. Collect our Purchasing references.
        //Debug.Log("OnInitialized: PASS");

        storeCtrl = controller;
        storeExtProvider = extensions;
    }

    public bool IsInitialized()
    { // Check if both the Purchasing references are set
        return storeCtrl != null && storeExtProvider != null; 
    }
}
