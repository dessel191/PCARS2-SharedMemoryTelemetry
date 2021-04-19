using System;
using System.Threading;
using System.IO.MemoryMappedFiles;

namespace PCARS2_SharedMemory
{
    class MemoryController
    {
        private MemoryMappedFile _mmf;
        private string _FilePath;
        private MemoryMappedViewAccessor _viewAccessor;

        private MemoryController()
        {
            _FilePath = "$pcars2$";

            _mmf = MemoryMappedFile.OpenExisting(_FilePath);
            _viewAccessor = _mmf.CreateViewAccessor();
        }

        private static MemoryController _instance;

        private static readonly object _lock = new object();

        public static MemoryController GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new MemoryController();
                    }
                }
            }
            return _instance;
        }

        public void Dispose()
        {
            _viewAccessor.Dispose();
            _mmf.Dispose();
        }
        
        public UInt32 GetValueUInt32(long offset)
        {
            return _viewAccessor.ReadUInt32(offset);
        }
        public Int32 GetValueInt32(long offset)
        {
            return _viewAccessor.ReadInt32(offset);
        }
        public Single GetValueSingle(long offset)
        {
            return _viewAccessor.ReadSingle(offset);
        }
        public bool GetValueBool(long offset)
        {
            return _viewAccessor.ReadBoolean(offset);
        }
        public char GetValueChar(long offset)
        {
            return _viewAccessor.ReadChar(offset);
        }
        public byte GetValueByte(long offset)
        {
            return _viewAccessor.ReadByte(offset);
        }
    }
}
