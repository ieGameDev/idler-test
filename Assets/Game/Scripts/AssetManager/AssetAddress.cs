namespace Game.Scripts.AssetManager
{
    public static class AssetAddress
    {
        public static class Player
        {
            public const string PlayerPath = "Player/Player";
            public const string HUDPath = "Player/PlayerUI";
        }

        public static class Customers
        {
            public const string CustomerSpawnManagerPath = "Logic/CustomerSpawnManager";
        }

        public static class OrderPath
        {
            public const string DishPath = "Order/Dish";
            public const string CashPath = "Order/Cash";
            public const string CookingAreaPath = "Order/CookingArea";
        }

        public static class DataPath
        {
            public const string CustomerDataPath = "ScriptableObjects/CharactersData/CustomerData";
            public const string DishesDataPath = "ScriptableObjects/Dishes";
            public const string CookingAreaDataPath = "ScriptableObjects/CookingAreas";
        }
    }
}