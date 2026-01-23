using DarksideApi.DarkOrbit.Commands.Shop;

namespace DarksideApi.DarkOrbit.Handlers.Shop
{
    internal class ShopItemListHandler : ICommandHandler
    {
        public void Handle(ByteArray @byte)
        {
            ShopItemListCommand shopItemList = new();
            shopItemList.Read(@byte);

            //Debug.WriteLine(shopItemList.ToString());
        }
    }
}
