using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DediLib.IO
{
    public sealed class AsyncBinaryWriter : IDisposable
    {
        private readonly Stream _stream;

        public AsyncBinaryWriter(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (!stream.CanWrite) throw new ArgumentException("Stream is not writable", nameof(stream));
            _stream = stream;
        }

        private readonly byte[] _buffer = new byte[8];
        public async Task WriteByte(byte value, CancellationToken cancellationToken)
        {
            _buffer[0] = value;
            await _stream.WriteAsync(_buffer, 0, sizeof(byte), cancellationToken).ConfigureAwait(false);
        }

        public async Task WriteSByte(sbyte value, CancellationToken cancellationToken)
        {
            _buffer[0] = (byte)value;
            await _stream.WriteAsync(_buffer, 0, sizeof(sbyte), cancellationToken).ConfigureAwait(false);
        }

        public async Task WriteInt16(short value, CancellationToken cancellationToken)
        {
            var buffer = BitConverter.GetBytes(value);
            await _stream.WriteAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false);
        }

        public async Task WriteUInt16(ushort value, CancellationToken cancellationToken)
        {
            var buffer = BitConverter.GetBytes(value);
            await _stream.WriteAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false);
        }

        public async Task WriteInt32(int value, CancellationToken cancellationToken)
        {
            var buffer = BitConverter.GetBytes(value);
            await _stream.WriteAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false);
        }

        public async Task WriteUInt32(uint value, CancellationToken cancellationToken)
        {
            var buffer = BitConverter.GetBytes(value);
            await _stream.WriteAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false);
        }

        public async Task WriteInt64(long value, CancellationToken cancellationToken)
        {
            var buffer = BitConverter.GetBytes(value);
            await _stream.WriteAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false);
        }

        public async Task WriteUInt64(ulong value, CancellationToken cancellationToken)
        {
            var buffer = BitConverter.GetBytes(value);
            await _stream.WriteAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false);
        }

        public async Task WriteSingle(float value, CancellationToken cancellationToken)
        {
            var buffer = BitConverter.GetBytes(value);
            await _stream.WriteAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false);
        }

        public async Task WriteDouble(double value, CancellationToken cancellationToken)
        {
            var buffer = BitConverter.GetBytes(value);
            await _stream.WriteAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false);
        }

        public async Task Write(byte[] buffer, int startIndex, int offset, CancellationToken cancellationToken)
        {
            await _stream.WriteAsync(buffer, startIndex, offset, cancellationToken).ConfigureAwait(false);
        }

        public void Seek(long offset, SeekOrigin origin)
        {
            _stream.Seek(offset, origin);
        }

        public async Task Flush(CancellationToken cancellationToken)
        {
            await _stream.FlushAsync(cancellationToken).ConfigureAwait(false);
        }

        public void Dispose()
        {
            _stream.Dispose();
        }
    }
}
