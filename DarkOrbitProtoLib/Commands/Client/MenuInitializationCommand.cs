namespace DarksideApi.DarkOrbit.Commands.Client
{
    public class MenuInitializationCommand : Command    //1689
    {
        public new const short Id = 20609;
        public string itemBarPosition;
        public List<MenuCategoryInitializationCommand> categories;
        public List<SlotBarInitializationCommand> slotBars;

        public MenuInitializationCommand()
        {
            this.itemBarPosition = string.Empty;
            this.categories = [];
            this.slotBars = [];
        }

        public override void Read(ByteArray reader)
        {
            var i = 0;
            var max = reader.ReadByte();
            while (i < max)
            {
                reader.ReadShort();
                var slotBar = new SlotBarInitializationCommand();
                slotBar.Read(reader);
                this.slotBars.Add(slotBar);
                i++;
            }
            this.itemBarPosition = reader.ReadUTF();
            i = 0;
            max = reader.ReadByte();
            while (i < max)
            {
                reader.ReadShort();
                var category = new MenuCategoryInitializationCommand();
                category.Read(reader);
                this.categories.Add(category);
                i++;
            }

        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
