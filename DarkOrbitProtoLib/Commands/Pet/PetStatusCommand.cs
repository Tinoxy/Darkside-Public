namespace DarksideApi.DarkOrbit.Commands.Pet
{
    public class PetStatusCommand : Command   //1317
    {
        public new const short Id = 157;
        public int petId;  //var_1340
        public int level;   //var_5005      
        public double experiencePoints;
        public double experiencePointsToNextLevel;
        public int hitpoints;
        public int hitPointsMax;
        public int shieldNow;
        public int shieldMax;
        public int currentFuel; //var_5006
        public int maxFuel;     //var_5007
        public int speed;
        public string name; //var_2061
        public string lootId;

        public PetStatusCommand()
        {
            this.name = string.Empty;
            this.lootId = string.Empty;
        }

        public override void Read(ByteArray reader)
        {
            this.petId = reader.ReadInt();
            this.petId = this.petId >>> 11 % 32 | this.petId << 32 - 11 % 32;
            this.level = reader.ReadInt();
            this.level = this.level << 5 | this.level >>> 27;
            this.experiencePoints = reader.ReadDouble();
            this.experiencePointsToNextLevel = reader.ReadDouble();
            this.hitpoints = reader.ReadInt();
            this.hitpoints = this.hitpoints >>> 20 | this.hitpoints << 12;
            this.hitPointsMax = reader.ReadInt();
            this.hitPointsMax = this.hitPointsMax << 27 | this.hitPointsMax >>> 5;
            this.shieldNow = reader.ReadInt();
            this.shieldNow = this.shieldNow << 13 | this.shieldNow >>> 19;
            this.shieldMax = reader.ReadInt();
            this.shieldMax = this.shieldMax >>> 30 | this.shieldMax << 2;
            this.currentFuel = reader.ReadInt();
            this.currentFuel = this.currentFuel << 4 | this.currentFuel >>> 28;
            this.maxFuel = reader.ReadInt();
            this.maxFuel = this.maxFuel >>> 26 | this.maxFuel << 6;
            this.speed = reader.ReadInt();
            this.speed = this.speed << 20 | this.speed >>> 12;
            this.name = reader.ReadUTF();
            this.lootId = reader.ReadUTF();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
