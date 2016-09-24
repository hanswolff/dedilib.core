using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DediLib.IO;
using Xunit;

namespace Test.DediLib.IO
{
    public class TestAsyncBinaryWriter
    {
        [Fact]
        public void Constructor_StreamNotWriteable_Throws()
        {
            var stream = new MemoryStream(new byte[1], false);

            Assert.Throws<ArgumentException>(() => new AsyncBinaryWriter(stream));
        }

        [Fact]
        public async Task WriteByte_Value_Bytes()
        {
            const byte value = byte.MaxValue;
            var mem = new MemoryStream();

            await new AsyncBinaryWriter(mem).WriteByte(value, CancellationToken.None).ConfigureAwait(false);

            var result = mem.ToArray()[0];
            Assert.Equal(value, result);
        }

        [Fact]
        public async Task WriteSByte_Value_Bytes()
        {
            const sbyte value = sbyte.MaxValue;
            var mem = new MemoryStream();

            await new AsyncBinaryWriter(mem).WriteSByte(value, CancellationToken.None).ConfigureAwait(false);

            var result = (sbyte)mem.ToArray()[0];
            Assert.Equal(value, result);
        }

        [Fact]
        public async Task WriteInt16()
        {
            const short value = short.MaxValue;
            var mem = new MemoryStream();

            await new AsyncBinaryWriter(mem).WriteInt16(value, CancellationToken.None).ConfigureAwait(false);

            var result = BitConverter.ToInt16(mem.ToArray(), 0);
            Assert.Equal(value, result);
        }
        
        [Fact]
        public async Task WriteUInt16()
        {
            const ushort value = ushort.MaxValue;
            var mem = new MemoryStream();

            await new AsyncBinaryWriter(mem).WriteUInt16(value, CancellationToken.None).ConfigureAwait(false);

            var result = BitConverter.ToUInt16(mem.ToArray(), 0);
            Assert.Equal(value, result);
        }
        
        [Fact]
        public async Task WriteInt32()
        {
            const int value = int.MaxValue;
            var mem = new MemoryStream();

            await new AsyncBinaryWriter(mem).WriteInt32(value, CancellationToken.None).ConfigureAwait(false);

            var result = BitConverter.ToInt32(mem.ToArray(), 0);
            Assert.Equal(value, result);
        }
        
        [Fact]
        public async Task WriteUInt32()
        {
            const uint value = uint.MaxValue;
            var mem = new MemoryStream();

            await new AsyncBinaryWriter(mem).WriteUInt32(value, CancellationToken.None).ConfigureAwait(false);

            var result = BitConverter.ToUInt32(mem.ToArray(), 0);
            Assert.Equal(value, result);
        }
        
        [Fact]
        public async Task WriteInt64()
        {
            const long value = long.MaxValue;
            var mem = new MemoryStream();

            await new AsyncBinaryWriter(mem).WriteInt64(value, CancellationToken.None).ConfigureAwait(false);

            var result = BitConverter.ToInt64(mem.ToArray(), 0);
            Assert.Equal(value, result);
        }
        
        [Fact]
        public async Task WriteUInt64()
        {
            const ulong value = ulong.MaxValue;
            var mem = new MemoryStream();

            await new AsyncBinaryWriter(mem).WriteUInt64(value, CancellationToken.None).ConfigureAwait(false);

            var result = BitConverter.ToUInt64(mem.ToArray(), 0);
            Assert.Equal(value, result);
        }
        
        [Fact]
        public async Task WriteSingle_Value_Bytes()
        {
            const float value = float.MaxValue;
            var mem = new MemoryStream();

            await new AsyncBinaryWriter(mem).WriteSingle(value, CancellationToken.None).ConfigureAwait(false);

            var result = BitConverter.ToSingle(mem.ToArray(), 0);
            Assert.Equal(value, result);
        }
        
        [Fact]
        public async Task WriteDouble_Value_Bytes()
        {
            const double value = double.MaxValue;
            var mem = new MemoryStream();

            await new AsyncBinaryWriter(mem).WriteDouble(value, CancellationToken.None).ConfigureAwait(false);

            var result = BitConverter.ToDouble(mem.ToArray(), 0);
            Assert.Equal(value, result);
        }
        
        [Fact]
        public async Task Write_Bytes_Bytes()
        {
            var mem = new MemoryStream();

            var buf = Enumerable.Range(1, 10).Select(x => (byte)x).ToArray();
            await new AsyncBinaryWriter(mem).Write(buf, 4, 2, CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(2, mem.Length);
            Assert.Equal(5, mem.ToArray()[0]);
            Assert.Equal(6, mem.ToArray()[1]);
        }

        [Fact]
        public void Seek_Offset_Position()
        {
            var mem = new MemoryStream(Enumerable.Range(1, 10).Select(x => (byte)x).ToArray());

            new AsyncBinaryReader(mem).Seek(5, SeekOrigin.Begin);

            Assert.Equal(5, mem.Position);
        }
    }
}
