namespace DarksideApi.DarkOrbit.Commands.Hero
{
    public class CargoUpdateCommand : Command   //AttributeOreCountUpdateCommand
    {
        public new const short Id = 106;
        public List<OreCountModule> cargoUpdates;
        public long UsedCargoSpace => this.cargoUpdates.Where(m => m.OreType != 3).Sum(module => module.count);

        public CargoUpdateCommand()
        {
            this.cargoUpdates = [];
        }

        public override void Read(ByteArray reader)
        {
            this.cargoUpdates = [];
            var i = 0;
            int max = reader.ReadByte();
            while (i < max)
            {
                reader.ReadShort();
                var module = new OreCountModule();
                module.Read(reader);
                this.cargoUpdates.Add(module);
                i++;
            }
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
