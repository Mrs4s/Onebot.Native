using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onebot.Native.Core.Models
{
    public class BytesPacket : BinaryWriter
    {
        private static readonly Encoding _encoding = Encoding.GetEncoding("GB18030");

        private readonly MemoryStream _stream;

        public BytesPacket(MemoryStream ms) : base(ms) => _stream = ms;

        public BytesPacket WriteInt16(short value) 
        {
            Write(BitConverter.GetBytes(value).Reverse().ToArray());
            return this;
        }

        public BytesPacket WriteInt32(int value)
        {
            Write(BitConverter.GetBytes(value).Reverse().ToArray());
            return this;
        }

        public BytesPacket WriteInt64(long value)
        {
            Write(BitConverter.GetBytes(value).Reverse().ToArray());
            return this;
        }

        public BytesPacket WriteBool(bool value) => WriteInt32(value ? 1 : 0);

        public BytesPacket WriteString( string value)
        {
            var b = _encoding.GetBytes(value);
            WriteInt16((short)b.Length);
            Write(b);
            return this;
        }

        public BytesPacket WriteShortPacket(Action<BytesPacket> func)
        {
            using (var ms = new MemoryStream())
            using (var packet = new BytesPacket(ms))
            {
                func(packet);
                var b = packet.ToByteArray();
                WriteInt16((short)b.Length).Write(b);
                return this;
            }
        }

        public byte[] ToByteArray() => _stream.ToArray();

        public string ToBase64() => Convert.ToBase64String(ToByteArray());
    }
}
