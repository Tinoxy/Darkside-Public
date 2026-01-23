using DarksideApi.DarkOrbit.Commands.Ship;

namespace DarksideApi.DarkOrbit.Commands.Pet
{
    public class PetHeroActivationCommand : Command   //1618
    {
        public new const short Id = 155;
        public int ownerId; //name_179
        public int userId;  //var_1343
        public int shipType;
        public int expansionStage;
        public string name; //var_2062
        public int factionId;
        public int clanId;
        public int level;   //var_5008
        public string clanTag;
        public int x;
        public int y;
        public int petSpeed;
        public MinimapStrategyModule minimapStrategy;    //var_3189

        public PetHeroActivationCommand()
        {
            this.minimapStrategy = new();
            this.clanTag = string.Empty;
            this.name = string.Empty;
        }

        public override void Read(ByteArray reader)
        {
            this.ownerId = reader.ReadInt();
            this.ownerId = this.ownerId >>> 14 | this.ownerId << 18;
            this.userId = reader.ReadInt();
            this.userId = this.userId << 5 | this.userId >>> 27;
            this.shipType = reader.ReadShort();
            this.shipType = 65535 & ((65535 & this.shipType) << 1 | (65535 & this.shipType) >>> 15);
            this.shipType = this.shipType > 32767 ? (int)(this.shipType - 65536) : this.shipType;
            this.expansionStage = reader.ReadShort();
            this.expansionStage = 65535 & ((65535 & this.expansionStage) >>> 11 | (65535 & this.expansionStage) << 5);
            this.expansionStage = this.expansionStage > 32767 ? (int)(this.expansionStage - 65536) : this.expansionStage;
            this.name = reader.ReadUTF();
            this.factionId = reader.ReadShort();
            this.factionId = 65535 & ((65535 & this.factionId) << 1 | (65535 & this.factionId) >>> 15);
            this.factionId = this.factionId > 32767 ? (int)(this.factionId - 65536) : this.factionId;
            this.clanId = reader.ReadInt();
            this.clanId = this.clanId >>> 12 | this.clanId << 20;
            this.level = reader.ReadShort();
            this.level = 65535 & ((65535 & this.level) >>> 7 | (65535 & this.level) << 9);
            this.level = this.level > 32767 ? (int)(this.level - 65536) : this.level;
            this.clanTag = reader.ReadUTF();
            this.x = reader.ReadInt();
            this.x = this.x << 12 | this.x >>> 20;
            this.y = reader.ReadInt();
            this.y = this.y >>> 11 | this.y << 21;
            this.petSpeed = reader.ReadInt();
            this.petSpeed = this.petSpeed >>> 9 | this.petSpeed << 23;
            if (reader.ReadShort() != 0)
            {
                this.minimapStrategy.Read(reader);
            }
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
