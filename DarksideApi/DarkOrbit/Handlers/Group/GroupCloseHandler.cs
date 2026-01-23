using DarksideApi.DarkOrbit.Commands.Group;

namespace DarksideApi.DarkOrbit.Handlers.Group
{
    internal class GroupCloseHandler : ICommandHandler
    {
        private readonly Api api;

        public GroupCloseHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            GroupCloseCommand groupClose = new();
            groupClose.Read(@byte);

            this.api.Group = null!;
            this.api.WriteLog("Group closed...");
        }
    }
}
