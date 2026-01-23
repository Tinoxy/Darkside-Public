using DarksideApi.DarkOrbit.Chat.Commands;
using DarksideApi.DarkOrbit.Chat.Commands.LowLevel;
using DarksideApi.DarkOrbit.Chat.Commands.Messages;
using System.Diagnostics;

namespace DarksideApi.DarkOrbit.Chat.Objects
{
    internal class ProtocolCoder
    {
        public static ByteArray Encode(Command command)
        {
            if (command is StringMessageCommand _loc3_)
            {
                Dictionary<string, object> _loc4_ = _loc3_.json;
                var _loc5_ = new BinaryMessage(new ByteArray());
                _loc5_.WriteJson(_loc4_);
                var _loc6_ = _loc5_.GetBytes();
                ByteArray result = new();
                result.WriteByte((byte)APlayMessageType.MSGTYPE_PAYLOAD_BINARY_JSON);
                result.WriteInt(_loc6_.Length);
                result.WriteByteArray(_loc6_);
                return result;
            }
            else if (command is IntroductionMessageCommand _loc7_)
            {
                ByteArray result = new();
                result.WriteByte((byte)APlayMessageType.LOWLEVEL_INTRODUCTION);
                result.WriteUTF(_loc7_.AddressString);
                return result;
            }
            else if (command is PongMessageCommand _loc8_)
            {
                ByteArray result = new();
                result.WriteByte((byte)APlayMessageType.LOWLEVEL_PONG);
                result.WriteInt(8);
                result.WriteInt64(_loc8_.SendTime);
                return result;
            }
            return null;
        }

        public static object Decode(ByteArray @byte)
        {
            if (@byte.Length() < 5)
            {
                return null;
            }
            var _loc3_ = (APlayMessageType)@byte.ReadByte();
            switch (_loc3_)
            {
                case APlayMessageType.PAYLOADGZIP:
                    return true;
                case APlayMessageType.PAYLOAD:
                    return @byte.ReadUTF();
                case APlayMessageType.MSGTYPE_PAYLOAD_BINARY_JSON:
                    var length = @byte.ReadInt();
                    var payload = @byte.ReadBytes((uint)length);
                    return payload;
                case APlayMessageType.LOWLEVEL_INTRODUCTION:
                    return @byte.ReadUTF();
                case APlayMessageType.LOWLEVEL_PING:
                    @byte.ReadInt();
                    var sendTime = @byte.ReadInt64();
                    @byte.ReadInt();
                    return new PongMessageCommand(sendTime);
                default:
                    Debug.WriteLine("unsupported message type");
                    return null;
            }
        }
    }
}
