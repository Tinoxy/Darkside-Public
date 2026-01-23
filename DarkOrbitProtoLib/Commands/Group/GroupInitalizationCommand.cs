namespace DarksideApi.DarkOrbit.Commands.Group
{
    public class GroupInitalizationCommand : Command
    {
        public new const short Id = 25151;
        public int leaderId;
        public int maxSize;
        public int id;
        public List<GroupMemberInitalizationCommand> members;
        public InvitationModeModule invitationMode;

        public GroupInitalizationCommand()
        {
            this.members = [];
            this.invitationMode = new();
        }

        public override void Read(ByteArray reader)
        {
            if (reader.ReadShort() != 0)
                this.invitationMode.Read(reader);
            var i = 0;
            var max = reader.ReadByte();
            while (i < max)
            {
                reader.ReadShort();
                var member = new GroupMemberInitalizationCommand();
                member.Read(reader);

                this.members.Add(member);
                i++;
            }
            this.maxSize = reader.ReadInt();
            this.maxSize = this.maxSize >>> 18 | this.maxSize << 14;
            this.id = reader.ReadInt();
            this.id = this.id << 20 | this.id >>> 12;
            this.leaderId = reader.ReadInt();
            this.leaderId = this.leaderId >>> 15 | this.leaderId << 17;
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
