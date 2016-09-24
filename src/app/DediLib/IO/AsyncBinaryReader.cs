using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DediLib.IO
{
    public class AsyncBinaryReader : IDisposable
    {
        private readonly Stream _stream;

        public AsyncBinaryReader(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (!stream.CanRead) throw new ArgumentException("Stream is not readable", nameof(stream));
            _stream = stream;
        }

        private readonly byte[] _buffer = new byte[8];

        public async Task<byte?> ReadByte(CancellationToken cancellationToken)
        {
            var read = await _stream.ReadAsync(_buffer, 0, sizeof(byte), cancellationToken).ConfigureAwait(false);
            if (read < sizeof(byte))
                return null;

            return _buffer[0];
        }

        public async Task<sbyte?> ReadSByte(CancellationToken cancellationToken)
        {
            var read = await _stream.ReadAsync(_buffer, 0, sizeof(sbyte), cancellationToken).ConfigureAwait(false);
            if (read < sizeof(sbyte))
                return null;

            return (sbyte)_buffer[0];
        }

        public async Task<short?> ReadInt16(CancellationToken cancellationToken)
        {
            var read = await _stream.ReadAsync(_buffer, 0, sizeof(short), cancellationToken).ConfigureAwait(false);
            if (read < sizeof(short))
                return null;

            return BitConverter.ToInt16(_buffer, 0);
        }

        public async Task<ushort?> ReadUInt16(CancellationToken cancellationToken)
        {
            var read = await _stream.ReadAsync(_buffer, 0, sizeof(ushort), cancellationToken).ConfigureAwait(false);
            if (read < sizeof(ushort))
                return null;

            return BitConverter.ToUInt16(_buffer, 0);
        }

        public async Task<int?> ReadInt32(CancellationToken cancellationToken)
        {
            var read = await _stream.ReadAsync(_buffer, 0, sizeof(int), cancellationToken).ConfigureAwait(false);
            if (read < sizeof(int))
                return null;

            return BitConverter.ToInt32(_buffer, 0);
        }

        public async Task<uint?> ReadUInt32(CancellationToken cancellationToken)
        {
            var read = await _stream.ReadAsync(_buffer, 0, sizeof(uint), cancellationToken).ConfigureAwait(false);
            if (read < sizeof(uint))
                return null;

            return BitConverter.ToUInt32(_buffer, 0);
        }

        public async Task<long?> ReadInt64(CancellationToken cancellationToken)
        {
            var read = await _stream.ReadAsync(_buffer, 0, sizeof(long), cancellationToken).ConfigureAwait(false);
            if (read < sizeof(long))
                return null;

            return BitConverter.ToInt64(_buffer, 0);
        }

        public async Task<ulong?> ReadUInt64(CancellationToken cancellationToken)
        {
            var read = await _stream.ReadAsync(_buffer, 0, sizeof(ulong), cancellationToken).ConfigureAwait(false);
            if (read < sizeof(ulong))
                return null;

            return BitConverter.ToUInt64(_buffer, 0);
        }

        public async Task<float?> ReadSingle(CancellationToken cancellationToken)
        {
            var read = await _stream.ReadAsync(_buffer, 0, sizeof(float), cancellationToken).ConfigureAwait(false);
            if (read < sizeof(float))
                return null;

            return BitConverter.ToSingle(_buffer, 0);
        }

        public async Task<double?> ReadDouble(CancellationToken cancellationToken)
        {
            var read = await _stream.ReadAsync(_buffer, 0, sizeof(double), cancellationToken).ConfigureAwait(false);
            if (read < sizeof(double))
                return null;

            return BitConverter.ToDouble(_buffer, 0);
        }

        public async Task<int> Read(byte[] buffer, int startIndex, int offset, CancellationToken cancellationToken)
        {
            return await _stream.ReadAsync(buffer, startIndex, offset, cancellationToken).ConfigureAwait(false);
        }

        public void Seek(long offset, SeekOrigin origin)
        {
            _stream.Seek(offset, origin);
        }

        public void Dispose()
        {
            _stream.Dispose();
        }
    }
}
