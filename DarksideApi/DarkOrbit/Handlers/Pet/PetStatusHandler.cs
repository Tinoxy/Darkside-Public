using DarksideApi.DarkOrbit.Commands.Pet;
using System.Diagnostics;

namespace DarksideApi.DarkOrbit.Handlers.Pet
{
    internal class PetStatusHandler : ICommandHandler
    {
        private readonly Api api;

        public PetStatusHandler(Api api)
        {
            this.api = api;
        }

        public void Handle(ByteArray @byte)
        {
            PetStatusCommand petStatus = new();
            petStatus.Read(@byte);

            Debug.WriteLine($"Pet Status Update: ID: {petStatus.petId}, Level: {petStatus.level}, Speed: {petStatus.speed}, Hitpoints: {petStatus.hitpoints}/{petStatus.hitPointsMax}, Fuel: {petStatus.currentFuel}/{petStatus.maxFuel}, Name: {petStatus.name}");

            if (api.Hero.IsPetActive && petStatus.petId == api.Hero.Pet.UserId)
            {
                api.Hero.Pet.Level = petStatus.level;
                api.Hero.Pet.Speed = petStatus.speed;
                api.Hero.Pet.HitPoints = petStatus.hitpoints;
                api.Hero.Pet.MaxHitPoints = petStatus.hitPointsMax;
                api.Hero.Pet.Fuel = petStatus.currentFuel;
                api.Hero.Pet.MaxFuel = petStatus.maxFuel;
                api.Hero.Pet.Name = petStatus.name;
                return;
            }
            if (!this.api.Pets.TryGetValue(petStatus.petId, out var pet))
            {
                return;
            }
            pet.Level = petStatus.level;
            pet.Speed = petStatus.speed;
            pet.HitPoints = petStatus.hitpoints;
            pet.MaxHitPoints = petStatus.hitPointsMax;
            pet.Fuel = petStatus.currentFuel;
            pet.MaxFuel = petStatus.maxFuel;
            pet.Name = petStatus.name;
        }
    }
}
