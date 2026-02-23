using DarksideApi.DarkOrbit.Commands.MessageStack;
using System.Text;

namespace DarksideApi.DarkOrbit.Commands.Expedition
{
    public class ExpeditionSelectShipCommand : Command
    {
        public new const short Id = 15182;
        public bool result;
        public BaseStackCommand stack;

        public ExpeditionSelectShipCommand()
        {
            this.stack = new();
        }

        public override void Read(ByteArray reader)
        {
            this.result = reader.ReadBool();
            if (reader.ReadShort() != 0)
                this.stack.Read(reader);
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"ExpeditionSelectShipCommand: {this.result}");
            if (this.stack != null)
            {
                sb.AppendLine(this.stack.ToString());
            }
            return sb.ToString();
        }
    }
}
