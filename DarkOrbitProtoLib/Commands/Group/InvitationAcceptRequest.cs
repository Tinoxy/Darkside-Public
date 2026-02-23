namespace DarksideApi.DarkOrbit.Commands.Group
{
    public class InvitationAcceptRequest : Command //InvitationAcceptRequest
    {
        public new const short Id = -1102;
        public int senderUserId;    //var_2017

        public InvitationAcceptRequest(int senderUserId)
        {
            this.senderUserId = senderUserId;
        }

        public override void Read(ByteArray reader)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteInt(this.senderUserId >>> 16 % 32 | this.senderUserId << 32 - 16 % 32);
            return this.@byte.ToArray();
        }
    }
}
