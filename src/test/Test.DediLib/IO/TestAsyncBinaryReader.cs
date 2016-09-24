using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DediLib.IO;
using NSubstitute;
using Xunit;

namespace Test.DediLib.IO
{
    public class TestAsyncBinaryReader
    {
        [Fact]
        public void Constructor_StreamNotReadable_Throws()
        {
            var stream = Substitute.For<Stream>();
            stream.CanRead.Returns(false);

            Assert.Throws<ArgumentException>(() => new AsyncBinaryReader(stream));
        }

        [Fact]
        public async Task ReadByte_AllBytesAvailable_Value()
        {
            const byte value = byte.MaxValue;
            var mem = new MemoryStream(BitConverter.GetBytes(value));

            var result = await new AsyncBinaryReader(mem).ReadByte(CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(value, result);
        }

        [Fact]
        public async Task ReadByte_NotEnoughBytes_Null()
        {
            var mem = new MemoryStream();

            var result = await new AsyncBinaryReader(mem).ReadByte(CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(null, result);
        }

        [Fact]
        public async Task ReadSByte_AllBytesAvailable_Value()
        {
            const sbyte value = sbyte.MaxValue;
            var mem = new MemoryStream(BitConverter.GetBytes(value));

            var result = await new AsyncBinaryReader(mem).ReadSByte(CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(value, result);
        }

        [Fact]
        public async Task ReadSByte_NotEnoughBytes_Null()
        {
            var mem = new MemoryStream();

            var result = await new AsyncBinaryReader(mem).ReadSByte(CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(null, result);
        }

        [Fact]
        public async Task ReadInt16()
        {
            const short value = short.MaxValue;
            var mem = new MemoryStream(BitConverter.GetBytes(value));

            var result = await new AsyncBinaryReader(mem).ReadInt16(CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(value, result);
        }

        [Fact]
        public async Task ReadInt16_NotEnoughBytes_Null()
        {
            var mem = new MemoryStream(new byte[sizeof(short) - 1]);

            var result = await new AsyncBinaryReader(mem).ReadInt16(CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(null, result);
        }
        
        [Fact]
        public async Task ReadUInt16()
        {
            const ushort value = ushort.MaxValue;
            var mem = new MemoryStream(BitConverter.GetBytes(value));

            var result = await new AsyncBinaryReader(mem).ReadUInt16(CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(value, result);
        }

        [Fact]
        public async Task ReadUInt16_NotEnoughBytes_Null()
        {
            var mem = new MemoryStream(new byte[sizeof(ushort) - 1]);

            var result = await new AsyncBinaryReader(mem).ReadUInt16(CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(null, result);
        }

        [Fact]
        public async Task ReadInt32()
        {
            const int value = int.MaxValue;
            var mem = new MemoryStream(BitConverter.GetBytes(value));

            var result = await new AsyncBinaryReader(mem).ReadInt32(CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(value, result);
        }

        [Fact]
        public async Task ReadInt32_NotEnoughBytes_Null()
        {
            var mem = new MemoryStream(new byte[sizeof(int) - 1]);

            var result = await new AsyncBinaryReader(mem).ReadInt32(CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(null, result);
        }

        [Fact]
        public async Task ReadUInt32()
        {
            const uint value = uint.MaxValue;
            var mem = new MemoryStream(BitConverter.GetBytes(value));

            var result = await new AsyncBinaryReader(mem).ReadUInt32(CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(value, result);
        }

        [Fact]
        public async Task ReadUInt32_NotEnoughBytes_Null()
        {
            var mem = new MemoryStream(new byte[sizeof(uint) - 1]);

            var result = await new AsyncBinaryReader(mem).ReadUInt32(CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(null, result);
        }

        [Fact]
        public async Task ReadInt64()
        {
            const long value = long.MaxValue;
            var mem = new MemoryStream(BitConverter.GetBytes(value));

            var result = await new AsyncBinaryReader(mem).ReadInt64(CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(value, result);
        }

        [Fact]
        public async Task ReadInt64_NotEnoughBytes_Null()
        {
            var mem = new MemoryStream(new byte[sizeof(long) - 1]);

            var result = await new AsyncBinaryReader(mem).ReadInt64(CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(null, result);
        }

        [Fact]
        public async Task ReadUInt64()
        {
            const ulong value = ulong.MaxValue;
            var mem = new MemoryStream(BitConverter.GetBytes(value));

            var result = await new AsyncBinaryReader(mem).ReadUInt64(CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(value, result);
        }

        [Fact]
        public async Task ReadUInt64_NotEnoughBytes_Null()
        {
            var mem = new MemoryStream(new byte[sizeof(long) - 1]);

            var result = await new AsyncBinaryReader(mem).ReadUInt64(CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(null, result);
        }

        [Fact]
        public async Task ReadSingle_AllBytesAvailable_Value()
        {
            const float value = float.MaxValue;
            var mem = new MemoryStream(BitConverter.GetBytes(value));

            var result = await new AsyncBinaryReader(mem).ReadSingle(CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(value, result);
        }

        [Fact]
        public async Task ReadSingle_NotEnoughBytes_Null()
        {
            var mem = new MemoryStream(new byte[sizeof(float) - 1]);

            var result = await new AsyncBinaryReader(mem).ReadSingle(CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(null, result);
        }

        [Fact]
        public async Task ReadDouble_AllBytesAvailable_Value()
        {
            const double value = double.MaxValue;
            var mem = new MemoryStream(BitConverter.GetBytes(value));

            var result = await new AsyncBinaryReader(mem).ReadDouble(CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(value, result);
        }

        [Fact]
        public async Task ReadDouble_NotEnoughBytes_Null()
        {
            var mem = new MemoryStream(new byte[sizeof(double) - 1]);

            var result = await new AsyncBinaryReader(mem).ReadDouble(CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(null, result);
        }

        [Fact]
        public async Task Read_Bytes_Bytes()
        {
            var mem = new MemoryStream(Enumerable.Range(1, 10).Select(x => (byte)x).ToArray());

            var buf = new byte[10];
            var result = await new AsyncBinaryReader(mem).Read(buf, 4, 2, CancellationToken.None).ConfigureAwait(false);

            Assert.Equal(2, result);
            Assert.Equal(0, buf[3]);
            Assert.Equal(1, buf[4]);
            Assert.Equal(2, buf[5]);
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
