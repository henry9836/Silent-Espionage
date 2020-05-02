using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
public class MillionairBaby : MonoBehaviour, IStoreListener
{

    private static IStoreController storeCtrl;
    private static IExtensionProvider storeExtProvider;

    //Purchase IDS
    public static string removeAds = "";

    public void Initialize()
    {
        if (storeCtrl == null)
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            //Add products
            builder.AddProduct(removeAds, ProductType.NonConsumable);

            UnityPurchasing.Initialize(this, builder);

        }
    }

    public void Purchase(string ID)
    {

    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        throw new System.NotImplementedException();
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
    {
        throw new System.NotImplementedException();
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        throw new System.NotImplementedException();
    }
}
