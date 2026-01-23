using System.Text;

namespace DarksideApi.DarkOrbit.Commands.MessageStack
{
    public class ArgStackCommand : Command
    {
        public new const short Id = 918;
        public ArgKey key;
        public int type;
        public string value;
        public BaseStackCommand value2;

        public ArgStackCommand()
        {
            this.value = string.Empty;
            this.value2 = new();
        }

        public override void Read(ByteArray reader)
        {
            if (reader.ReadShort() != 0)
                this.value2.Read(reader);
            this.value = reader.ReadUTF();
            this.type = reader.ReadInt();
            this.key = (ArgKey)reader.ReadShort();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }

        public enum ArgKey
        {
            _NAME_ = 9,
            _MINIMUM_BID = 8,
            _notesLimit_ = 7,
            _AMOUNT_ = 10,
            XXX = 12,
            _CURRENCY_ = 11,
            YYY = 13,
            DATETIME = 14,
            _maxMessages_ = 6,
            _date_ = 4,
            _userLevel_ = 5,
            _BOOSTERNAME_ = 1,
            _COOLDOWN_ = 0,
            _MAX_BID_ = 15,
            _module_ = 3,
            _level_ = 3,
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"ArgStackCommand: {this.key}");
            sb.AppendLine($"Type: {this.type}");
            sb.AppendLine($"Value: {this.value}");
            if (this.value2 != null)
                sb.AppendLine($"Value2: {this.value2.ToString()}");
            return sb.ToString();
        }
    }
}
