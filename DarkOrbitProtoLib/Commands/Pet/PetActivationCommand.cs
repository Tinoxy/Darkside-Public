using DarksideApi.DarkOrbit.Commands.Ship;

namespace DarksideApi.DarkOrbit.Commands.Pet
{
    public class PetActivationCommand : Command   //1317
    {
        public new const short Id = 232;
        public int ownerId; //name_179
        public int petId;  //var_1340
        public int var_3664;
        public int var_2718;
        public string username; //var_2061
        public int factionId;   //var_41
        public int clanId;  //var_3518
        public int level;   //var_5005
        public string clanTag;
        public ClanRelationModule var_3306;
        public int x;
        public int y;
        public int speed;    //var_5238
        public bool var_861;
        public bool isVisible;
        public MinimapStrategyModule var_3188;

        public PetActivationCommand()
        {
            this.username = string.Empty;
            this.clanTag = string.Empty;
            this.var_3306 = new();
            this.var_3188 = new();
        }

        public override void Read(ByteArray reader)
        {
            this.ownerId = reader.ReadInt();
            this.ownerId = this.ownerId << 7 % 32 | this.ownerId >>> 32 - 7 % 32;
            this.petId = reader.ReadInt();
            this.petId = this.petId >>> 11 % 32 | this.petId << 32 - 11 % 32;
            this.var_3664 = reader.ReadShort();
            this.var_3664 = 65535 & ((65535 & this.var_3664) << 13 % 16 | (65535 & this.var_3664) >>> 16 - 13 % 16);
            this.var_3664 = this.var_3664 > 32767 ? this.var_3664 - 65536 : this.var_3664;
            this.var_2718 = reader.ReadShort();
            this.var_2718 = 65535 & ((65535 & this.var_2718) << 6 % 16 | (65535 & this.var_2718) >>> 16 - 6 % 16);
            this.var_2718 = this.var_2718 > 32767 ? this.var_2718 - 65536 : this.var_2718;
            this.username = reader.ReadUTF();
            this.factionId = reader.ReadShort();
            this.factionId = 65535 & ((65535 & this.factionId) >>> 5 % 16 | (65535 & this.factionId) << 16 - 5 % 16);
            this.factionId = this.factionId > 32767 ? this.factionId - 65536 : this.factionId;
            this.clanId = reader.ReadInt();
            this.clanId = this.clanId << 14 % 32 | this.clanId >>> 32 - 14 % 32;
            this.level = reader.ReadShort();
            this.level = 65535 & ((65535 & this.level) >>> 8 % 16 | (65535 & this.level) << 16 - 8 % 16);
            this.level = this.level > 32767 ? this.level - 65536 : this.level;
            this.clanTag = reader.ReadUTF();
            if (reader.ReadShort() != 0)
            {
                this.var_3306.Read(reader);
            }
            this.x = reader.ReadInt();
            this.x = this.x << 2 % 32 | this.x >>> 32 - 2 % 32;
            this.y = reader.ReadInt();
            this.y = this.y >>> 5 % 32 | this.y << 32 - 5 % 32;
            this.speed = reader.ReadInt();
            this.speed = this.speed << 11 % 32 | this.speed >>> 32 - 11 % 32;
            this.var_861 = reader.ReadBool();
            this.isVisible = reader.ReadBool();
            if (reader.ReadShort() != 0)
            {
                this.var_3188.Read(reader);
            }
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
