namespace DarksideApi.DarkOrbit.Commands.Group
{
    public class ClanModule : PlayerAttributeModule   //1094
    {
        public new const short Id = 18051;
        public int clanId;
        public string clanTag;

        public ClanModule()
        {
            this.clanTag = string.Empty;
        }

        public override void Read(ByteArray reader)
        {
            reader.ReadShort(); //471 -> Empty
            base.Read(reader);

            this.clanTag = reader.ReadUTF();
            this.clanId = reader.ReadInt();
            this.clanId = this.clanId >>> 9 | this.clanId << 23;
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort((short)Id);
            this.@byte.WriteShort((short)0);
            // TODO: konnte nicht automatisch konvertieren: base.Read(reader);
            this.@byte.WriteUTF(this.clanTag);
            this.@byte.WriteInt(this.clanId >>> 9 | this.clanId << 23);
            // TODO: konnte nicht automatisch konvertieren: this.clanId = this.clanId >>> 9 | this.clanId << 23;
            return this.@byte.ToArray();
        }
    }
}
