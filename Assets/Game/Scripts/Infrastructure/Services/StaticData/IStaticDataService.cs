using Game.Scripts.Infrastructure.DI;
using Game.Scripts.Logic.OrderLogic;
using Game.Scripts.Logic.ScriptableObjects;

namespace Game.Scripts.Infrastructure.Services.StaticData
{
    public interface IStaticDataService : IService
    {
        void LoadCustomer();
        void LoadDishes();
        void LoadCookingAreas();
        CustomerData DataForCustomer();
        DishData DataForDish(DishTypeId typeId);
        CookingAreaData DataForCookingArea(DishTypeId typeId);
    }
}