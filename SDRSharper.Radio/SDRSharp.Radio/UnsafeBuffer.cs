using System;
using System.Runtime.InteropServices;

namespace SDRSharp.Radio
{
	public sealed class UnsafeBuffer : IDisposable
	{
        private readonly GCHandle _handle;
        private unsafe void* _ptr;
        private int _length;
        private Array _buffer;

        public unsafe void* Address
        {
            get
            {
                return this._ptr;
            }
        }   

        public int Length =>
            this._length;

        private unsafe UnsafeBuffer(Array buffer, int realLength, bool aligned)
		{
			this._buffer = buffer;
			this._handle = GCHandle.Alloc(this._buffer, GCHandleType.Pinned);
			this._ptr = (void*)this._handle.AddrOfPinnedObject();
			if (aligned)
			{
				this._ptr = (void*)((long)this._ptr + 15 & -16);
			}
			this._length = realLength;
		}

		~UnsafeBuffer()
		{
			this.Dispose();
		}

		public unsafe void Dispose()
		{
			if (this._handle.IsAllocated)
			{
				this._handle.Free();
			}
			this._buffer = null;
			this._ptr = null;
			this._length = 0;
			GC.SuppressFinalize(this);
		}

		public static unsafe implicit operator void*(UnsafeBuffer unsafeBuffer)
		{
			return unsafeBuffer.Address;
		}

        public static UnsafeBuffer Create(Array buffer) =>
            new UnsafeBuffer(buffer, buffer.Length, false);

        public static UnsafeBuffer Create(int size) =>
            Create(1, size, true);

        public static UnsafeBuffer Create(int length, int sizeOfElement) =>
            Create(length, sizeOfElement, true);

        public static UnsafeBuffer Create(int length, int sizeOfElement, bool aligned) =>
            new UnsafeBuffer(new byte[(length * sizeOfElement) + (aligned ? 0x10 : 0)], length, aligned);
    }
}
