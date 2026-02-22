namespace DarksideApi.DarkOrbit.Commands.Group
{
    public class InvitationRejectRequest : Command  //InvitationRejectRequest
    {
        public new const short Id = 27456;
        public int candidateId;

        public InvitationRejectRequest(int candidateId)
        {
            this.candidateId = candidateId;
        }

        public override void Read(ByteArray reader)
        {
            this.candidateId = reader.ReadInt();
            this.candidateId = this.candidateId << 31 % 32 | this.candidateId >>> 1 % 32;
        }
        public override byte[] Write()
        {
            this.@byte.WriteShort(Id);
            this.@byte.WriteInt(this.candidateId >>> 31 | this.candidateId << 1);
            return this.@byte.ToArray();
        }
    }
}
