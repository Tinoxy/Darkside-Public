namespace DarksideApi.DarkOrbit.Commands.Hero
{
    public class UserInventoryModule : Command //UserInventoryModule
    {
        public new const short Id = 5868;
        public int activeRebateSubDiscount;
        public List<InventoryItemModule> inventoryItemModules;
        public int activePremiumDiscount;
        public int activeHappyUpgradeDiscount;

        public UserInventoryModule()
        {
            this.inventoryItemModules = [];
        }

        public override void Read(ByteArray reader)
        {  
            this.activeHappyUpgradeDiscount = reader.ReadInt();
            this.activeHappyUpgradeDiscount = this.activeHappyUpgradeDiscount << 10 | this.activeHappyUpgradeDiscount >>> 22;
            this.activePremiumDiscount = reader.ReadInt();
            this.activePremiumDiscount = this.activePremiumDiscount << 10 | this.activePremiumDiscount >>> 22;
            var i = 0;
            var max = reader.ReadShort();
            while (i < max)
            {
                reader.ReadShort();
                var e = new InventoryItemModule();
                e.Read(reader);
                this.inventoryItemModules.Add(e);
                i++;
            }
            this.activeRebateSubDiscount = reader.ReadInt();
            this.activeRebateSubDiscount = this.activeRebateSubDiscount >>> 8 | this.activeRebateSubDiscount << 24;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
