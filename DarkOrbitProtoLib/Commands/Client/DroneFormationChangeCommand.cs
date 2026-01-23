namespace DarksideApi.DarkOrbit.Commands.Client
{
    public class DroneFormationChangeCommand : Command  //class_1546
    {
        public new const short Id = 257;
        public int userId;
        public int selectedFormation;   //var_4746

        public DroneFormationChangeCommand()
        {

        }

        public override void Read(ByteArray reader)
        {
            this.userId = reader.ReadInt();
            this.userId = this.userId >>> 15 % 32 | this.userId << 32 - 15 % 32;
            this.selectedFormation = reader.ReadInt();
            this.selectedFormation = this.selectedFormation << 8 % 32 | this.selectedFormation >>> 32 - 8 % 32;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
