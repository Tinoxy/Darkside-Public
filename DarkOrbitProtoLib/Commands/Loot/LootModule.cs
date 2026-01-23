namespace DarksideApi.DarkOrbit.Commands.Loot
{
    public class LootModule : Command  //690
    {
        public new const short Id = 2016;
        public string lootId;
        public int amount;
        public string modifier;

        public LootModule()
        {
            this.lootId = string.Empty;
            this.amount = 0;
            this.modifier = string.Empty;
        }

        public override void Read(ByteArray reader)
        {
            this.lootId = reader.ReadUTF();
            this.amount = reader.ReadInt();
            this.amount = this.amount << 6 % 32 | this.amount >>> 32 - 6 % 32;
            this.modifier = reader.ReadUTF();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"LootModule: {this.lootId}, Amount: {this.amount}, Modifier: {this.modifier}";
        }
    }
}
