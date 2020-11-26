using System;
using UnityEngine;

namespace MightyAttributes.Utilities
{
    public static class MaskUtilities
    {
        public static bool Contains(this LayerMask mask, int layer) => (1 << layer & mask) != 0;

        public static int ToBitMask(this int value) => 1 << value;

        public static byte GetBitIndex(this short value, bool offsetOnce = false)
        {       
            if (value == 0)
            {
                if (!offsetOnce)
                    throw new ArgumentOutOfRangeException();
                return 0;
            }
            
            byte index = 0;
            while (value != (short) (1 << index))
                if (++index > 14)
                    throw new ArgumentOutOfRangeException();

            return offsetOnce ? (byte) (index + 1) : index;
        }

        public static byte GetBitIndex(this ushort value, bool offsetOnce = false)
        {            
            if (value == 0)
            {
                if (!offsetOnce)
                    throw new ArgumentOutOfRangeException();
                return 0;
            }
            
            byte index = 0;
            while (value != (ushort) (1 << index))
                if (++index > 15)
                    throw new ArgumentOutOfRangeException();

            return offsetOnce ? (byte) (index + 1) : index;
        }

        public static byte GetBitIndex(this uint value, bool offsetOnce = false)
        {
            if (value == 0)
            {
                if (!offsetOnce)
                    throw new ArgumentOutOfRangeException();
                return 0;
            }

            byte index = 0;
            while (value != 1u << index)
                if (++index > 31)
                    throw new ArgumentOutOfRangeException();

            return offsetOnce ? (byte) (index + 1) : index;
        }

        public static byte GetBitIndex(this ulong value, bool offsetOnce = false)
        {
            if (value == 0)
            {
                if (!offsetOnce)
                    throw new ArgumentOutOfRangeException();
                return 0;
            }

            byte index = 0;
            while (value != 1UL << index)
                if (++index > 63)
                    throw new ArgumentOutOfRangeException();

            return offsetOnce ? (byte) (index + 1) : index;
        }

        public static bool MaskContains(this int mask, int flag) => (mask & flag) != 0;

        public static int AddFlag(this int mask, int flag) => mask | flag;
    }
}