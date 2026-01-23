namespace DarksideApi.DarkOrbit.Commands.Client
{
    public class MenuCategoryInitializationCommand : Command    //2092
    {
        public new const short Id = -6128;
        public string categoryId;
        public List<MenuItemInitializationCommand> menuItems;

        public MenuCategoryInitializationCommand()
        {
            this.categoryId = string.Empty;
            this.menuItems = [];
        }

        public override void Read(ByteArray reader)
        {
            this.categoryId = reader.ReadUTF();
            var i = 0;
            var max = reader.ReadByte();
            while (i < max)
            {
                reader.ReadShort();
                var menuItem = new MenuItemInitializationCommand();
                menuItem.Read(reader);
                this.menuItems.Add(menuItem);
                i++;
            }
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
