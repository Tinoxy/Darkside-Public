namespace DarksideApi.DarkOrbit.Commands.Map
{
    public class MapAddPOICommand : Command   //MappAddPoiCommand
    {
        public new const short Id = 25558;
        public int design;
        public int shape;
        public bool inverted;   //name_186
        public bool active;
        public List<int> shapeCoordinates;  //var_4996
        public string poiTypeSpecification; //var_2116
        public int poiType; //var_3710
        public string poiId;   //var_3768

        public MapAddPOICommand()
        {
            this.shapeCoordinates = [];
            this.poiTypeSpecification = string.Empty;
            this.poiId = string.Empty;
        }

        public override void Read(ByteArray reader)
        {
            var i = 0;
            int max = reader.ReadByte();
            while (i < max)
            {
                var v = reader.ReadInt();
                this.shapeCoordinates.Add(v >>> 6 | v << 26);
                i++;
            }
            if (reader.ReadShort() != 0)
            {
                this.design = reader.ReadShort();
            }
            this.poiTypeSpecification = reader.ReadUTF();
            this.inverted = reader.ReadBool();
            this.poiId = reader.ReadUTF();
            this.shape = reader.ReadShort();
            this.active = reader.ReadBool();
            if (reader.ReadShort() != 0)
            {
                this.poiType = reader.ReadShort();
            } 
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }

        public enum ShapeType
        {
            CIRCLE = 0,
            POLYGON = 2,
            RECTANGLE = 3,
        }

        public override string ToString()
        {
            return $"[MapAddPOI] Id: {this.poiId}, Type: {this.poiTypeSpecification}, Shape: {this.shape}, Active: {this.active}, Coordinates: {string.Join("; ", shapeCoordinates)}";
        }
    }
}
