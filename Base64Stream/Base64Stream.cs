using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base64Stream
{
    public class Base64Stream : BitStream
    {
        public const byte Pad = (byte)'=';
        public const byte BitsInChar = 6;

        private static IEnumerable<char> FromTo(char start, char end)
        {
            for (var i = start; i <= end; i++)
            {
                yield return i;
            }

        }

        public static IEnumerable<KeyValuePair<char, byte>> Pairs() => Chars().Select((c, i) => new KeyValuePair<char, byte>(c, (byte)i));

        public static IEnumerable<char> Chars()
        {
            foreach (var c in FromTo('A', 'Z'))
            {
                yield return c;
            }

            foreach (var c in FromTo('a', 'z'))
            {
                yield return c;
            }

            foreach (var c in FromTo('0', '9'))
            {
                yield return c;
            }

            yield return '+';
            yield return '/';
        }

        private readonly Dictionary<byte, char> EncodeMap = Pairs().ToDictionary(pair => pair.Value, pair => pair.Key);
        private readonly Dictionary<char, byte> DecodeMap = Pairs().ToDictionary(pair => pair.Key, pair => pair.Value);
        private int Peek = -1;

        public Base64Stream(Stream baseStream) : base(baseStream)
        {

        }

        public Base64Stream(Stream baseStream, bool leaveOpen) : base(baseStream, leaveOpen)
        {

        }

        protected override int ReadEncodedByte(out int offset, out int length)
        {
            if (this.Peek == -1)
            {
                var value = this.BaseStream.ReadByte();

                if (value == -1)
                {
                    offset = 0;
                    length = 0;
                    return -1;
                }
                else
                {
                    this.Peek = value;
                    return this.ReadEncodedByte(out offset, out length);
                }

            }

            for (var l = 0; ;)
            {
                var prev = this.Peek;
                var value = this.BaseStream.ReadByte();

                if (value == Pad)
                {
                    l++;
                }
                else
                {
                    this.Peek = value;
                    offset = 8 - BitsInChar;
                    length = BitsInChar - l;
                    return DecodeMap[(char)prev];
                }

            }

        }

        protected override bool WriteEncodedByte(byte value, int position, bool disposing)
        {
            if (disposing == true)
            {
                var remain = BitsInChar - position;
                this.BaseStream.WriteByte((byte)EncodeMap[(byte)(value << remain)]);

                for (var i = 0; i < remain / 2; i++)
                {
                    base.BaseStream.WriteByte(Pad);
                }

                return true;
            }
            else if (position == BitsInChar)
            {
                this.BaseStream.WriteByte((byte)EncodeMap[value]);
                return true;
            }
            else
            {
                return false;
            }

        }

    }

}
