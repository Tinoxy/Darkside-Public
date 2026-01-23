namespace DarksideApi.DarkOrbit.Commands.Group
{
    public class InvitationInitializationCommand : Command   //InvitationInitalizationCommand
    {
        public new const short Id = 13610;
        public string inviterUsername; //var_5515
        public int candidateId;  //var_1058
        public string candidateUsername;  //name_22
        public int inviterId;    //var_2016
        public ShipIconModule candidateShip;
        public ShipIconModule inviterShip;

        public InvitationInitializationCommand()
        {
            this.inviterUsername = string.Empty;
            this.candidateUsername = string.Empty;
            this.candidateShip = new();
            this.inviterShip = new();
        }

        public override void Read(ByteArray reader)
        {
            this.inviterUsername = reader.ReadUTF();
            if (reader.ReadShort() != 0)
            {
                this.inviterShip.Read(reader);
            }
            this.candidateUsername = reader.ReadUTF();
            this.inviterId = reader.ReadInt();
            this.inviterId = this.inviterId << 5 % 32 | this.inviterId >>> 32 - 5 % 32;
            if (reader.ReadShort() != 0)
            {
                this.candidateShip.Read(reader);
            }
            this.candidateId = reader.ReadInt();
            this.candidateId = this.candidateId >>> 6 % 32 | this.candidateId << 32 - 6 % 32;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
