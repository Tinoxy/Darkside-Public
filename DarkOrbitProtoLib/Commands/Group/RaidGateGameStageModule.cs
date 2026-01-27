namespace DarksideApi.DarkOrbit.Commands.Group
{
    public class RaidGateGameStageModule : PlayerAttributeModule
    {
        public new const short Id = -11924;
        public bool isInGate;
        public int gameStage;

        public override void Read(ByteArray reader)
        {
            reader.ReadShort(); //471 -> Empty
            base.Read(reader);

            this.gameStage = reader.ReadInt();
            this.gameStage = this.gameStage << 24 | this.gameStage >>> 8;
            this.isInGate = reader.ReadBool();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort((short)Id);
            this.@byte.WriteShort((short)0);
            // TODO: konnte nicht automatisch konvertieren: base.Read(reader);
            this.@byte.WriteInt(this.gameStage >>> 24 | this.gameStage << 8);
            // TODO: konnte nicht automatisch konvertieren: this.gameStage = this.gameStage << 24 | this.gameStage >>> 8;
            this.@byte.WriteBool(this.isInGate);
            return this.@byte.ToArray();
        }
    }
}
