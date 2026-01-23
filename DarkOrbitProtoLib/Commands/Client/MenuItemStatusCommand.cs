namespace DarksideApi.DarkOrbit.Commands.Client
{
    public class MenuItemStatusCommand : Command    //842
    {
        public new const short Id = -17500;
        public bool activatable;
        public bool visible;
        public string menuItemId;
        public bool buyable;
        public MessageKeyListWildcardCommand toolTipSlotBar;
        public string iconLootId;
        public bool available;
        public bool selected;
        public int counterStyle;
        public long counterValue;
        public bool blocked;
        public long maxCounterValue;
        public MessageKeyListWildcardCommand toolTipItemBar;

        public MenuItemStatusCommand()
        {
            this.menuItemId = string.Empty;
            this.toolTipSlotBar = new();
            this.iconLootId = string.Empty;
            this.toolTipItemBar = new();
        }

        public override void Read(ByteArray reader)
        {
            this.available = reader.ReadBool();
            this.blocked = reader.ReadBool();
            this.menuItemId = reader.ReadUTF();
            this.activatable = reader.ReadBool();
            this.selected = reader.ReadBool();
            if (reader.ReadShort() != 0)
            {
                this.toolTipSlotBar.Read(reader);
            }
            this.iconLootId = reader.ReadUTF();
            this.visible = reader.ReadBool();
            this.maxCounterValue = (long)reader.ReadDouble();
            this.counterStyle = reader.ReadShort();
            this.buyable = reader.ReadBool();
            if (reader.ReadShort() != 0)
            {
                this.toolTipItemBar.Read(reader);
            }
            this.counterValue = (long)reader.ReadDouble();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
