using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
public class MillionairBaby : MonoBehaviour, IStoreListener
{

    private static IStoreController storeCtrl;
    private static IExtensionProvider storeExtProvider;

    private Text debugger;

    //Purchase IDS
    [HideInInspector]
    public string removeAds = "removeads";

    private void Start()
    {
        Initialize();
    }

    void Debugger(string debugStr)
    {
        if (debugger)
        {
            debugger.text += '\n' + debugStr;
        }
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

            //Find Debugger
            if (GameObject.Find("DEBUG_OUTPUT"))
            {
                debugger = GameObject.Find("DEBUG_OUTPUT").GetComponent<Text>();
            }
        }
    }

    public void Purchase(string ID)
    {
        if (IsInitialized()) {
            Product product = storeCtrl.products.WithID(ID);
            if (product != null && product.availableToPurchase)
            {
                //Debug.Log("Purchasing Product...");
                Debugger($"recept check: {checkPurchase(ID)}");
                Debugger("Purchasing Product...");
                storeCtrl.InitiatePurchase(ID);
                Debugger("Purchased");
            }
            else
            {
                Debugger("Attempted to purchase an item but IAP is not initalised");
                Debug.LogWarning("Attempted to purchase an item but it either doesn't exists or is not available to be purchased");
            }
        }
        else
        {
            Debugger("Attempted to purchase an item but IAP is not initalised");
            Debug.LogWarning("Attempted to purchase an item but IAP is not initalised");
        }
    }

    public bool checkPurchase(string ID)
    {
        Product product = storeCtrl.products.WithID(ID);

        return product.hasReceipt;
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
