using System.Diagnostics;

namespace DarksideApi.DarkOrbit.Chat.Objects
{
    public class BinaryMessage
    {
        private const int NULL_VALUE = -2147483648;
        private ByteArray byteArray;

        public BinaryMessage(ByteArray byteArray)
        {
            this.byteArray = byteArray;
        }

        public byte[] GetBytes()
        {
            return this.byteArray.ToArray();
        }

        public string ReadString()
        {
            var length = (uint)this.byteArray.ReadInt();
            return this.byteArray.ReadUTF(length);
        }

        public Dictionary<string, object> ReadJson()
        {
            string _loc4_ = null;
            var _loc1_ = this.ReadInt();
            if (_loc1_ < 0)
            {
                return null;
            }
            Dictionary<string, object> _loc2_ = [];
            var _loc3_ = 0;
            while (_loc3_ < _loc1_)
            {
                _loc4_ = this.ReadString();
                _loc2_[_loc4_] = this.ReadTypeDecoded();
                _loc3_++;
            }
            return _loc2_;
        }

        public object ReadTypeDecoded()
        {
            BinaryMessage _loc1_ = this;
            var _loc2_ = _loc1_.ReadByte();
            object? _loc3_ = null;
            switch (_loc2_)
            {
                case 0:
                    _loc3_ = new();
                    break;
                case 1:
                    _loc3_ = _loc1_.ReadBoolean();
                    break;
                case 2:
                    _loc3_ = _loc1_.ReadJArray();
                    break;
                case 3:
                    _loc3_ = _loc1_.ReadInt();
                    break;
                case 4:
                    _loc3_ = _loc1_.ReadJson();
                    break;
                case 5:
                    _loc3_ = _loc1_.ReadString();
                    break;
                case 6:
                    _loc3_ = _loc1_.ReadByte();
                    break;
                case 7:
                    _loc3_ = _loc1_.ReadLong();
                    break;
                case 8:
                    _loc3_ = _loc1_.ReadFloat();
                    break;
                case 9:
                    _loc3_ = _loc1_.ReadDouble();
                    break;
            }
            return _loc3_;
        }

        public void WriteTypeEncoded(object param1)
        {
            BinaryMessage _loc2_ = this;
            if (param1 == null)
            {
                _loc2_.WriteByte(0);
                return;
            }
            if (param1 is bool)
            {
                _loc2_.WriteByte(1);
                _loc2_.WriteBoolean((bool)param1);
            }
            else if (param1 is Dictionary<string, object>)
            {
                _loc2_.WriteByte(2);
                _loc2_.WriteJArray((Dictionary<string, object>)param1);
            }
            else if (param1 is int)
            {
                _loc2_.WriteByte(3);
                _loc2_.WriteInt((int)param1);
            }
            else if (param1 is string)
            {
                _loc2_.WriteByte(5);
                _loc2_.WriteString((string)param1);
            }
            else if (param1 is float)
            {
                _loc2_.WriteByte(8);
                _loc2_.WriteFloat((float)param1);
            }
            else
            {
                if (!(param1 is object))
                {
                    Debug.WriteLine("unknown value .. " + param1 + " " + param1.GetType().Name);
                }
                _loc2_.WriteByte(4);
                _loc2_.WriteJson((Dictionary<string, object>)param1);
            }
        }

        public int ReadByte()
        {
            return this.byteArray.ReadByte();
        }

        public int ReadInt()
        {
            var _loc1_ = this.byteArray.ReadInt();
            return _loc1_ == NULL_VALUE ? 0 : _loc1_;
        }

        public long ReadLong()
        {
            var _loc1_ = this.byteArray.ReadInt64();
            return _loc1_ == NULL_VALUE ? 0L : _loc1_;
        }

        public float ReadFloat()
        {
            var _loc1_ = this.byteArray.ReadFloat();
            return _loc1_ == NULL_VALUE ? 0f : _loc1_;
        }

        public double ReadDouble()
        {
            var _loc1_ = this.byteArray.ReadDouble();
            return _loc1_ == NULL_VALUE ? 0d : _loc1_;
        }

        public bool ReadBoolean()
        {
            var _loc1_ = this.byteArray.ReadByte();
            return _loc1_ != 0 && _loc1_ == 2;
        }

        public List<object> ReadJArray()
        {
            var _loc1_ = this.ReadInt();
            if (_loc1_ < 0)
            {
                return null;
            }
            List<object> _loc2_ = [];
            var _loc3_ = 0;
            while (_loc3_ < _loc1_)
            {
                _loc2_.Add(this.ReadTypeDecoded());
                _loc3_++;
            }
            return _loc2_;
        }

        public void WriteString(string param1)
        {
            if (param1 == null)
            {
                this.byteArray.WriteInt(0);
            }
            else
            {
                var _loc2_ = new ByteArray();
                _loc2_.WriteUTF(param1);
                this.byteArray.WriteInt(_loc2_.Length());
                this.byteArray.WriteByteArray(_loc2_.ToArray());
            }
        }

        public void WriteJson(Dictionary<string, object> param1)
        {
            if (param1 == null)
            {
                this.WriteInt(-1);
            }
            else
            {
                this.WriteInt(param1.Count);
                foreach (KeyValuePair<string, object> _loc2_ in param1)
                {
                    this.WriteString(_loc2_.Key);
                    this.WriteTypeEncoded(_loc2_.Value);
                }
            }
        }

        public void WriteByte(byte param1)
        {
            this.byteArray.WriteByte(param1);
        }

        public void WriteInt(int param1)
        {
            if (param1 == null)
            {
                this.byteArray.WriteInt(NULL_VALUE);
            }
            else
            {
                this.byteArray.WriteInt(param1);
            }
        }

        public void WriteLong(long param1)
        {
            if (param1 == null)
            {
                this.byteArray.WriteInt64(NULL_VALUE);
            }
            else
            {
                this.byteArray.WriteInt64(param1);
            }
        }

        public void WriteFloat(float param1)
        {
            if (param1 == null || param1 == float.NaN)
            {
                this.byteArray.WriteFloat(NULL_VALUE);
            }
            else
            {
                this.byteArray.WriteFloat(param1);
            }
        }

        public void WriteBoolean(bool param1)
        {
            if (param1 == null)
            {
                this.byteArray.WriteByte(0);
            }
            else
            {
                this.byteArray.WriteByte((byte)(!!param1 ? 2 : 1));
            }
        }

        public void WriteJArray(Dictionary<string, object> param1)
        {
            var _loc2_ = 0;
            if (param1 == null)
            {
                this.WriteInt(-1);
            }
            else
            {
                this.WriteInt(param1.Count);
                _loc2_ = 0;
                while (_loc2_ < param1.Count)
                {
                    this.WriteTypeEncoded(param1.ElementAt(_loc2_));
                    _loc2_++;
                }
            }
        }

        public bool ReadOptional()
        {
            return this.ReadBoolean();
        }

        public bool WriteOptional(object param1)
        {
            var _loc2_ = param1 != null;
            this.WriteBoolean(_loc2_);
            return _loc2_;
        }
    }
}
