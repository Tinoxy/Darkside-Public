namespace DarksideApi.DarkOrbit.Commands.Action
{
    public class MenuActionRequest : Command    //MenuActionRequest
    {
        public new const short Id = 32474;
        public short sourceType;     //var_2573
        public string menuItemId;   //var_389
        public short actionType;   //3171

        public const int ACTION_STYLE_DEACTIVATE = 0;
        public const int ACTION_STYLE_ACTIVATE = 1;

        public const int BAR_TYPE_BAR = 0;
        public const int BAR_TYPE_NONE = 1;

        public MenuActionRequest(short source, string menuItemId, short type)
        {
            this.sourceType = source;
            this.menuItemId = menuItemId;
            this.actionType = type;
        }

        public override void Read(ByteArray reader)
        {
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteShort(this.sourceType);
            this.@byte.WriteUTF(this.menuItemId);
            this.@byte.WriteShort(this.actionType);
            return this.@byte.ToArray();
        }
    }
}
