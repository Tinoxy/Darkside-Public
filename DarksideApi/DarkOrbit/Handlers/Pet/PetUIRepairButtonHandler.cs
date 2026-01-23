using DarksideApi.DarkOrbit.Commands.Pet;

namespace DarksideApi.DarkOrbit.Handlers.Pet
{
    internal class PetUIRepairButtonHandler : ICommandHandler
    {
        private readonly Api api;

        public PetUIRepairButtonHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            PetUIRepairButtonCommand petUIRepairButton = new();
            petUIRepairButton.Read(@byte);

            api.Logging.Log($"Pet Repair Button: " + petUIRepairButton.enabled + ", cost: " + petUIRepairButton.repairCost);
        }
    }
}
