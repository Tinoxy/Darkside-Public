namespace DarksideApi.DarkOrbit.Commands.Booster
{
    public class BoosterSharedDetailModule : BoosterDetailModule
    {
        public new const short Id = -20104;
        public int numberOfBoosters;

        public BoosterSharedDetailModule()
        {
            this.lootId = string.Empty;
        }

        public override void Read(ByteArray reader)
        {
            reader.ReadShort();
            base.Read(reader);

            this.numberOfBoosters = reader.ReadInt();
            this.numberOfBoosters = this.numberOfBoosters >>> 30 | this.numberOfBoosters << 2;
        }

        public override string ToString()
        {
            return base.ToString() + ", numberOfBoosters: " + this.numberOfBoosters;
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort((short)Id);
            this.@byte.WriteShort((short)0);
            // TODO: konnte nicht automatisch konvertieren: base.Read(reader);
            this.@byte.WriteInt(this.numberOfBoosters >>> 30 | this.numberOfBoosters << 2);
            // TODO: konnte nicht automatisch konvertieren: this.numberOfBoosters = this.numberOfBoosters >>> 30 | this.numberOfBoosters << 2;
            return this.@byte.ToArray();
        }
    }
}
