using Game.Scripts.Infrastructure.DI;
using Game.Scripts.Infrastructure.Services.UI;
using Game.Scripts.Logic.OrderLogic;
using Game.Scripts.Logic.PurchaseAreaLogic;
using Game.Scripts.Logic.ScriptableObjects;
using Game.Scripts.UI.Windows;

namespace Game.Scripts.Infrastructure.Services.StaticData
{
    public interface IStaticDataService : IService
    {
        void LoadCustomer();
        void LoadDishes();
        void LoadCookingAreas();
        void LoadPurchasableAreas();
        void LoadWindows();
        CustomerData DataForCustomer();
        DishData DataForDish(DishTypeId typeId);
        CookingAreaData DataForCookingArea(DishTypeId typeId);
        PurchasableAreaData DataForPurchasableArea(PurchasableAreaTypeId typeId);
        WindowConfig ForWindow(WindowId windowId);
    }
}