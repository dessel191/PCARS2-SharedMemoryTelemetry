using System;
using System.Collections.Generic;
using System.Text;

namespace PCARS2_SharedMemory
{
    public class PCARS2_SharedMemoryData
    {
        // const
        private const int Participants_Max = 64;
        private const int String_lenght_max = 64;
        private const int Tyre_Compound_Name_Lenght_Max = 40;
        private const int VEC_Max = 3;
        private const int Tyre_Max = 4;

        private MemoryController _mc;
        private readonly ParticipantData[] participantInfo;


        public enum Tyres
        {
            FrontLeft,
            FrontRight,
            RearLeft,
            RearRight,
            Max
        }
        public enum TyreFlags
        {
            Attached    = (1<<0),
            Inflated    = (1<<1),
            IsOnGround  = (1<<2)
        }
        public enum Vectors
        {
            X,
            Y,
            Z,
            Max
        }
        public enum GameStates
        {
            Exited,
            FrontEnd,
            InGame_Playing,
            InGame_Paused,
            InGame_InMenu_TimeTicking,
            InGame_Restarking,
            InGame_Replay,
            FrontEnd_Replay,
            Max
        }
        public enum SessionStates
        {
            Invalid,
            Practice,
            Test,
            Qualify,
            FormationLap,
            Race,
            TimeAttack,
            Max
        }
        public enum RaceStates
        {
            Invalid,
            NotStarted,
            Racing,
            Finished,
            Disqualified,
            Retired,
            DNF,
            Max
        }
        public enum FlagColours
        {
            None,
            Green,
            Blue,
            White_SlowCar,
            White_FinalLap,
            Red,
            Yellow,
            DoubleYellow,
            BlackWhite,
            BlackOrangeCircle,
            Black,
            Chequered,
            Max
        }
        public enum FlagReasons
        {
            None,
            SoloCrash,
            VehicleCrash,
            VehicleObstruction,
            Max
        }
        public enum PitModes
        {
            None,
            DrivingIntoPits,
            InPit,
            DrivingOutOfPits,
            InGarage,
            DrivingOutOfGarage,
            Max
        }
        public enum PitStopSchedules
        {
            None,
            PlayerRequested,
            EngeneerRequested,
            DamageRequested,
            Mandatory,
            DriveThrough,
            StopGo,
            PitStopOcupied,
            Max
        }
        public enum CarFlags
        {
            Headlight       = (1<<0),
            EngineActive    = (1<<1),
            EngineWarning   = (1<<2),
            SpeedLimiter    = (1<<3),
            ABS             = (1<<4),
            Handbrake       = (1<<5)
        }
        public enum CarDamageStates
        {
            None,
            Offtrack,
            LargeProp,
            Spinning,
            Rolling,
            Max
        }
        public enum TerrainMaterials
        {
            Road,
            LowGripRoad,
            BumpyRoad1,
            BumpyRoad2,
            BumpyRoad3,
            Marbles,
            GrassyBerms,
            Grass,
            Gravel,
            BumpyGravel,
            RumbleStrips,
            Drains,
            Tyrewalls,
            Cementwalls,
            Guardrails,
            Sand,
            BumpySand,
            Dirt,
            BumpyDirt,
            DirtRoad,
            BumpyDirtRoad,
            Pavement,
            DirtBank,
            Wood,
            DryVerge,
            ExitRumbleStrips,
            Grasscrete,
            LongGrass,
            SlopeGRass,
            Cobbles,
            SandRoad,
            BakedClay,
            AstroTurf,
            SnowHalf,
            SnowFull,
            DamagedRoad,
            TrainTrackRoad,
            BumpyCobbles,
            AriesOnly,
            OrionOnly,
            B1Rumbles,
            B2Rumbles,
            RoughSandMedium,
            RoughSandHeavy,
            SnowWalls,
            IceRoad,
            RunoffRoad,
            IllegalStrip,
            PaintConcrete,
            PaintConcreteIllegal,
            RallyTarmac,
            Max
        }

        // Version Number
        public uint Version => _mc.GetValueUInt32(0);
        public uint BuildVersionNumber => _mc.GetValueUInt32(4);
        public uint GameState => _mc.GetValueUInt32(8);

        // Game States
        public uint SessionState => _mc.GetValueUInt32(12);
        public uint RaceState => _mc.GetValueUInt32(16);

        // Participant Info
        public int ViewedParticipantIndex => _mc.GetValueInt32(20);
        public int NumParticipants => _mc.GetValueInt32(24);
        public ParticipantData[] Participants
        {
            get
            {
                return participantInfo;
            }
        }

        // Unfiltered Input
        public float UnfilteredThrottle => _mc.GetValueSingle(6428);
        public float UnfilteredBrake => _mc.GetValueSingle(6432);
        public float UnfilteredSteering => _mc.GetValueSingle(6436);
        public float UnfilteredClutch => _mc.GetValueSingle(6440);

        // Vehicle Information
        public string CarName
        {
            get
            {
                string n = "";
                for (int i = 0; i < String_lenght_max; i++)
                {
                    n += (char)_mc.GetValueByte(6444 + i);
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
                    n += (char)_mc.GetValueByte(6508 + i);
                }
                return n.Trim('\0');
            }
        }

        // Event Information
        public uint LapsInEvent => _mc.GetValueUInt32(6572);
        public string TrackLocation
        {
            get
            {
                string n = "";
                for (int i = 0; i < String_lenght_max; i++)
                {
                    n += (char)_mc.GetValueByte(6576 + i);
                }
                return n.Trim('\0');
            }
        }
        public string TrackVariation
        {
            get
            {
                string n = "";
                for (int i = 0; i < String_lenght_max; i++)
                {
                    n += (char)_mc.GetValueByte(6640 + i);
                }
                return n.Trim('\0');
            }
        }
        public float TrackLenght => _mc.GetValueSingle(6704);

        // Timings
        public int NumSectors => _mc.GetValueInt32(6708);       // no idea why size of bool is 4
        public bool IsLapInvalidated => _mc.GetValueBool(6712);
        public float BestLapTime => _mc.GetValueSingle(6716);
        public float LastLapTime => _mc.GetValueSingle(6720);
        public float CurrentTime => _mc.GetValueSingle(6724);
        public float SplitTimeAhead => _mc.GetValueSingle(6728);
        public float SplitTimeBehind => _mc.GetValueSingle(6732);
        public float SplitTime => _mc.GetValueSingle(6736);
        public float EventTimeRemaining => _mc.GetValueSingle(6740);
        public float PersonalFastestLapTime => _mc.GetValueSingle(6744);
        public float WorldFastestLapTime => _mc.GetValueSingle(6748);
        public float CurrentSector1Time => _mc.GetValueSingle(6752);
        public float CurrentSector2Time => _mc.GetValueSingle(6756);
        public float CurrentSector3Time => _mc.GetValueSingle(6760);
        public float FastestSector1Time => _mc.GetValueSingle(6764);
        public float FastestSector2Time => _mc.GetValueSingle(6768);
        public float FastestSector3Time => _mc.GetValueSingle(6772);
        public float PersonalFastestSector1Time => _mc.GetValueSingle(6776);
        public float PersonalFastestSector2Time => _mc.GetValueSingle(6780);
        public float PersonalFastestSector3Time => _mc.GetValueSingle(6784);
        public float WorldFastestSector1Time => _mc.GetValueSingle(6788);
        public float WorldFastestSector2Time => _mc.GetValueSingle(6792);
        public float WorldFastestSector3Time => _mc.GetValueSingle(6796);

        //Flags
        public uint HighestFlagColour => _mc.GetValueUInt32(6800);
        public uint HighestFlagReason => _mc.GetValueUInt32(6804);

        // Pit Info
        public uint PitMode => _mc.GetValueUInt32(6808);
        public uint PitSchedule => _mc.GetValueUInt32(6812);

        // Car State
        public uint CarFlag => _mc.GetValueUInt32(6816);
        public float OilTempCelsius => _mc.GetValueSingle(6820);
        public float OilPressureKPa => _mc.GetValueSingle(6824);
        public float WaterTempCelsius => _mc.GetValueSingle(6828);
        public float WaterPressureKPa => _mc.GetValueSingle(6832);
        public float FuelPressureKPa => _mc.GetValueSingle(6836);
        public float FuelLevel => _mc.GetValueSingle(6840);
        public float FuelCapacity => _mc.GetValueSingle(6844);
        public float Speed => _mc.GetValueSingle(6848);
        public float Rpm => _mc.GetValueSingle(6852);
        public float MaxRPM => _mc.GetValueSingle(6856);
        public float Brake => _mc.GetValueSingle(6860);
        public float Throttle => _mc.GetValueSingle(6864);
        public float Clutch => _mc.GetValueSingle(6868);
        public float Steering => _mc.GetValueSingle(6872);
        public int Gear => _mc.GetValueInt32(6876);
        public int NumGears => _mc.GetValueInt32(6880);
        public float OdometerKM => _mc.GetValueSingle(6884);
        public bool IsAntiLockActive => _mc.GetValueBool(6888);
        public int LastOpponentCollisionIndex => _mc.GetValueInt32(6892);
        public float LastOpponentCollisionMagnitude => _mc.GetValueSingle(6896);
        public bool BoostActive => _mc.GetValueBool(6900);
        public float BoostAmount => _mc.GetValueSingle(6904);

        // Motion & Device Related
        public float[] Orientation
        {
            get
            {
                float[] s = new float[3];
                for (int i = 0; i < VEC_Max; i++)
                {
                    s[i] = _mc.GetValueSingle(6908 + i*4);
                }
                return s;
            }
        }
        public float[] LocalVelocity
        {
            get
            {
                float[] s = new float[3];
                for (int i = 0; i < VEC_Max; i++)
                {
                    s[i] = _mc.GetValueSingle(6920 + i*4);
                }
                return s;
            }
        }
        public float[] WorldVelocity
        {
            get
            {
                float[] s = new float[3];
                for (int i = 0; i < VEC_Max; i++)
                {
                    s[i] = _mc.GetValueSingle(6932 + i*4);
                }
                return s;
            }
        }
        public float[] AngularVelocity
        {
            get
            {
                float[] s = new float[3];
                for (int i = 0; i < VEC_Max; i++)
                {
                    s[i] = _mc.GetValueSingle(6944 + i*4);
                }
                return s;
            }
        }
        public float[] LocalAcceleration
        {
            get
            {
                float[] s = new float[3];
                for (int i = 0; i < VEC_Max; i++)
                {
                    s[i] = _mc.GetValueSingle(6956 + i * 4);
                }
                return s;
            }
        }
        public float[] WorldAcceleration
        {
            get
            {
                float[] s = new float[3];
                for (int i = 0; i < VEC_Max; i++)
                {
                    s[i] = _mc.GetValueSingle(6968 + i * 4);
                }
                return s;
            }
        }
        public float[] ExtentsCentre
        {
            get
            {
                float[] s = new float[3];
                for (int i = 0; i < VEC_Max; i++)
                {
                    s[i] = _mc.GetValueSingle(6980 + i * 4);
                }
                return s;
            }
        }

        // Wheels & Tyres
        public uint[] TyreFlag
        {
            get
            {
                uint[] s = new uint[Tyre_Max];
                for (int i = 0; i < Tyre_Max; i++)
                {
                    s[i] = _mc.GetValueUInt32(6992 + i * 4);
                }
                return s;
            }
        }
        public uint[] Terrain
        {
            get
            {
                uint[] s = new uint[Tyre_Max];
                for (int i = 0; i < Tyre_Max; i++)
                {
                    s[i] = _mc.GetValueUInt32(7008 + i * 4);
                }
                return s;
            }
        }
        public float[] TyreY
        {
            get
            {
                float[] s = new float[Tyre_Max];
                for (int i = 0; i < Tyre_Max; i++)
                {
                    s[i] = _mc.GetValueSingle(7024 + i * 4);
                }
                return s;
            }
        }
        public float[] TyreRPS
        {
            get
            {
                float[] s = new float[Tyre_Max];
                for (int i = 0; i < Tyre_Max; i++)
                {
                    s[i] = _mc.GetValueSingle(7040 + i * 4);
                }
                return s;
            }
        }

        ///////////////////////////////////////
        ///         16 OBSOLETE BYTES       ///
        ///////////////////////////////////////

        public float[] TyreTemp
        {
            get
            {
                float[] s = new float[Tyre_Max];
                for (int i = 0; i < Tyre_Max; i++)
                {
                    s[i] = _mc.GetValueSingle(7072 + i * 4);
                }
                return s;
            }
        }

        ///////////////////////////////////////
        ///         16 OBSOLETE BYTES       ///
        ///////////////////////////////////////

        public float[] TyreHeightAboveGround
        {
            get
            {
                float[] s = new float[Tyre_Max];
                for (int i = 0; i < Tyre_Max; i++)
                {
                    s[i] = _mc.GetValueSingle(7104 + i * 4);
                }
                return s;
            }
        }

        ///////////////////////////////////////
        ///         16 OBSOLETE BYTES       ///
        ///////////////////////////////////////
        
        public float[] TyreWear
        {
            get
            {
                float[] s = new float[Tyre_Max];
                for (int i = 0; i < Tyre_Max; i++)
                {
                    s[i] = _mc.GetValueSingle(7136 + i * 4);
                }
                return s;
            }
        }
        public float[] BrakeDamage
        {
            get
            {
                float[] s = new float[Tyre_Max];
                for (int i = 0; i < Tyre_Max; i++)
                {
                    s[i] = _mc.GetValueSingle(7152 + i * 4);
                }
                return s;
            }
        }
        public float[] SuspensionDamage
        {
            get
            {
                float[] s = new float[Tyre_Max];
                for (int i = 0; i < Tyre_Max; i++)
                {
                    s[i] = _mc.GetValueSingle(7168 + i * 4);
                }
                return s;
            }
        }
        public float[] BrakeTempCelsius
        {
            get
            {
                float[] s = new float[Tyre_Max];
                for (int i = 0; i < Tyre_Max; i++)
                {
                    s[i] = _mc.GetValueSingle(7184 + i * 4);
                }
                return s;
            }
        }
        public float[] TyreThreadTemp
        {
            get
            {
                float[] s = new float[Tyre_Max];
                for (int i = 0; i < Tyre_Max; i++)
                {
                    s[i] = _mc.GetValueSingle(7200 + i * 4);
                }
                return s;
            }
        }
        public float[] TyreLayerTemp
        {
            get
            {
                float[] s = new float[Tyre_Max];
                for (int i = 0; i < Tyre_Max; i++)
                {
                    s[i] = _mc.GetValueSingle(7216 + i * 4);
                }
                return s;
            }
        }
        public float[] TyreCarcassTemp
        {
            get
            {
                float[] s = new float[Tyre_Max];
                for (int i = 0; i < Tyre_Max; i++)
                {
                    s[i] = _mc.GetValueSingle(7232 + i * 4);
                }
                return s;
            }
        }
        public float[] TyreRimTemp
        {
            get
            {
                float[] s = new float[Tyre_Max];
                for (int i = 0; i < Tyre_Max; i++)
                {
                    s[i] = _mc.GetValueSingle(7248 + i * 4);
                }
                return s;
            }
        }
        public float[] TyreInternalAirTemp
        {
            get
            {
                float[] s = new float[Tyre_Max];
                for (int i = 0; i < Tyre_Max; i++)
                {
                    s[i] = _mc.GetValueSingle(7264 + i * 4);
                }
                return s;
            }
        }

        // Car Damage
        public uint CrashState => _mc.GetValueUInt32(7280);
        public float AeroDamage => _mc.GetValueSingle(7284);
        public float EngineDamage => _mc.GetValueSingle(7288);

        // Weather
        public float AmbientTemperature => _mc.GetValueSingle(7292);
        public float TrackTemperature => _mc.GetValueSingle(7296);
        public float RainDensity => _mc.GetValueSingle(7300);
        public float WindSpeed => _mc.GetValueSingle(7304);
        public float WindDirectionX => _mc.GetValueSingle(7308);
        public float WindDirectionY => _mc.GetValueSingle(7312);
        public float CloudBrightness => _mc.GetValueSingle(7316);

        // Sequence Number
        public uint SequenceNumber => _mc.GetValueUInt32(7320);

        // Additional Car Variables
        public float[] WheelLocalPositionY
        {
            get
            {
                float[] s = new float[Tyre_Max];
                for (int i = 0; i < Tyre_Max; i++)
                {
                    s[i] = _mc.GetValueSingle(7324 + i * 4);
                }
                return s;
            }
        }
        public float[] SuspensionTravel
        {
            get
            {
                float[] s = new float[Tyre_Max];
                for (int i = 0; i < Tyre_Max; i++)
                {
                    s[i] = _mc.GetValueSingle(7340 + i * 4);
                }
                return s;
            }
        }
        public float[] SuspensionVelocity
        {
            get
            {
                float[] s = new float[Tyre_Max];
                for (int i = 0; i < Tyre_Max; i++)
                {
                    s[i] = _mc.GetValueSingle(7356 + i * 4);
                }
                return s;
            }
        }
        public float[] AirPressure
        {
            get
            {
                float[] s = new float[Tyre_Max];
                for (int i = 0; i < Tyre_Max; i++)
                {
                    s[i] = _mc.GetValueSingle(7372 + i * 4);
                }
                return s;
            }
        }
        public float EngineSpeed => _mc.GetValueSingle(7388);
        public float EngineTorque => _mc.GetValueSingle(7392);
        public float[] Wings
        {
            get
            {
                float[] s = new float[2];
                for (int i = 0; i < 2; i++)
                {
                    s[i] = _mc.GetValueSingle(7396 + i * 4);
                }
                return s;
            }
        }
        public float HandBrake => _mc.GetValueSingle(7404);
        public int EnforcedPitStopLap => _mc.GetValueInt32(19248);
        public string TranslatedTrackLocation
        {
            get
            {
                string n = "";
                for (int i = 0; i < String_lenght_max; i++)
                {
                    n += (char)_mc.GetValueByte(19252 + i);
                }
                return n.Trim('\0');
            }
        }
        public string TranslatedTrackVariation
        {
            get
            {
                string n = "";
                for (int i = 0; i < String_lenght_max; i++)
                {
                    n += (char)_mc.GetValueByte(19316 + i);
                }
                return n.Trim('\0');
            }
        }
        public float BrakeBias => _mc.GetValueSingle(19380);
        public float TurboBoostPressure => _mc.GetValueSingle(19384);
        public string[] TyreCompound
        {
            get
            {
                string[] n = {"","","","" };
                for (int j = 0; j < Tyre_Max; j++)
                {
                    for (int i = 0; i < Tyre_Compound_Name_Lenght_Max; i++)
                    {
                        n[j] += (char)_mc.GetValueByte(19388 + i + j * Tyre_Compound_Name_Lenght_Max);
                    }
                    n[j] = n[j].Trim('\0');
                }
                return n;
            }
        }
        public float SnowDensity => _mc.GetValueSingle(20572);
        public PCARS2_SharedMemoryData()
        {
            _mc = MemoryController.GetInstance();
            participantInfo = new ParticipantData[Participants_Max];
            for(int i = 0; i < Participants_Max; i++)
            {
                participantInfo[i] = new ParticipantData(i);
            }
        }
        public void Dispose()
        {
            _mc.Dispose();
        }
    }
}
