using System;
using System.Collections.Generic;
using System.Text;

namespace PCARS2_SharedMemory
{
    public class ParticipantData
    {
        // 1 + 64 + 3 + 12 + 4 + 4 + 4 + 4 + 4 = 100  sizof (no idea why there must be +3 after name)
        private const int String_lenght_max = 64;
        private const int VEC_Max = 3;
        private int offset = 28;

        private MemoryController _mc;
        private int _index;

        public ParticipantData(int index)
        {
            _mc = MemoryController.GetInstance();
            _index = index;
            offset += 100 * index;
        }
        public bool IsActive => _mc.GetValueBool(offset);
        public string Name
        {
            get
            {
                string n = "";
                for(int i = 0; i < String_lenght_max; i++)
                {
                    n += (char)_mc.GetValueByte(offset + 1 + i);
                }
                return n.Trim('\0');
            }
        }
        public float[] WorldPosition
        {
            get
            {
                float[] s = new float[3];
                for(int i = 0; i < VEC_Max; i++)
                {
                    s[i] = _mc.GetValueSingle(offset + i*4 + 68);
                }
                return s;
            }
        }
        public float CurrentLapDistace => _mc.GetValueSingle(offset + 80);
        public uint RacePosition => _mc.GetValueUInt32(offset + 84);
        public uint LapsCompleted => _mc.GetValueUInt32(offset + 88);
        public uint CurrentLap => _mc.GetValueUInt32(offset + 92);
        public int CurrentSector => _mc.GetValueInt32(offset + 96);


        public float CurrentSector1Times => _mc.GetValueSingle(7408 + _index * 4);
        public float CurrentSector2Times => _mc.GetValueSingle(7676 + _index * 4);
        public float CurrentSector3Times => _mc.GetValueSingle(7932 + _index * 4);
        public float FastestSector1Times => _mc.GetValueSingle(8188 + _index * 4);
        public float FastestSector2Times => _mc.GetValueSingle(8444 + _index * 4);
        public float FastestSector3Times => _mc.GetValueSingle(8700 + _index * 4);
        public float FastestLapTime => _mc.GetValueSingle(8956 + _index * 4);
        public float LastLapTime => _mc.GetValueSingle(9212 + _index * 4);
        public bool IsLapIvalidated => _mc.GetValueBool(9468 + _index);
        public uint RaceState => _mc.GetValueUInt32(9532 + _index * 4);
        public uint PitMode => _mc.GetValueUInt32(9788 + _index * 4);
        public float[] Orientation
        {
            get
            {
                float[] s = new float[VEC_Max];
                for (int i = 0; i < VEC_Max; i++)
                {
                    s[i] = _mc.GetValueSingle(10044 + _index * 12 + i * 4);
                }
                return s;
            }
        }
        public float Speed => _mc.GetValueSingle(10800 + _index * 4);
        public string CarName
        {
            get
            {
                string n = "";
                for (int i = 0; i < String_lenght_max; i++)
                {
                    n += (char)_mc.GetValueByte(11056 + i + _index * String_lenght_max);
                }
                return n.Trim('\0');
            }
        }
        public string CarClassName
        {
            get
            {
                string n = "";
                for (int i = 0; i < String_lenght_max; i++)
                {
                    n += (char)_mc.GetValueByte(15152 + i + _index * String_lenght_max);
                }
                return n.Trim('\0');
            }
        }
        public uint PitSchedule => _mc.GetValueUInt32(19548 + _index * 4);
        public uint HighetsFlagColour => _mc.GetValueUInt32(19804 + _index * 4);
        public uint HighetsFlagReason => _mc.GetValueUInt32(20060 + _index * 4);
        public uint Nationalities => _mc.GetValueUInt32(20316 + _index * 4);

    }
}
