namespace DarksideApi.DarkOrbit.Commands.Group
{
    public class InvitationCreationRequest : Command   //InvitationCreationRequest
    {
        public new const short Id = 1018;
        public string username; //name

        public InvitationCreationRequest(string username)
        {
            this.username = username;
        }

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteUTF(this.username);
            return this.@byte.ToArray();
        }
    }
}
