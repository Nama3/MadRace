using System;
using System.Collections.Generic;
using System.Linq;

namespace MightyAttributes.Utilities
{
    public static class ArrayUtilities
    {
        public static void ResizeArray(ref Array array, Type elementType, int length)
        {
            if (array == null)
            {
                array = Array.CreateInstance(elementType, length);
                return;
            }

            if (length == array.Length) return;

            while (length > array.Length) array = AddArrayElement(array, elementType);
            while (length < array.Length) array = RemoveArrayElement(array, elementType, length - 1);
        }

        public static Array AddArrayElement(Array array, Type elementType)
        {
            var length = array.Length;
            var newArray = Array.CreateInstance(elementType, length + 1);
            var i = 0;
            for (; i < length; i++) newArray.SetValue(array.GetValue(i), i);
            try
            {
                newArray.SetValue(Activator.CreateInstance(elementType), i);
            }
            catch
            {
                // ignored
            }

            return newArray;
        }

        public static Array RemoveArrayElement(Array array, Type elementType, int index)
        {
            var length = array.Length;
            if (length == 0) return array;

            var newArray = Array.CreateInstance(elementType, length + 1);
            var offset = 0;
            for (var i = 0; i < length - 1; i++)
            {
                if (i == index) offset = 1;
                newArray.SetValue(array.GetValue(i + offset), i);
            }

            return newArray;
        }

        public static void ResizeArray<T>(int length, ref T[] array)
        {
            if (array == null)
            {
                array = new T[length];
                return;
            }

            if (length == array.Length) return;

            while (length > array.Length) array = AddArrayElement(array);
            while (length < array.Length) array = RemoveArrayElement(array, length - 1);
        }

        public static T[] AddArrayElement<T>(T[] array, T value = default)
        {
            var length = array.Length;
            var newArray = new T[length + 1];
            var i = 0;
            for (; i < length; i++) newArray[i] = array[i];
            try
            {
                newArray[i] = value;
            }
            catch
            {
                // ignored
            }

            return newArray;
        }

        public static T[] InsertArrayElement<T>(T[] array, int index, T value = default)
        {
            var length = array.Length;
            var newArray = new T[length + 1];
            var offset = 0;
            for (var i = 0; i < length + 1; i++)
            {
                if (i == index)
                {
                    offset = 1;
                    try
                    {
                        newArray[i] = value;
                    }
                    catch
                    {
                        // ignored
                    }

                    continue;
                }

                newArray[i] = array[i - offset];
            }

            return newArray;
        }

        public static T[] RemoveArrayElement<T>(T[] array, int index)
        {
            var length = array.Length;
            if (length == 0) return array;
            var newArray = new T[length - 1];
            var offset = 0;
            for (var i = 0; i < length - 1; i++)
            {
                if (i == index)
                    offset = 1;

                newArray[i] = array[i + offset];
            }

            return newArray;
        }
        
        public static void Resize<T>(List<T> list, int size, T defaultValue = default)
        {
            if (size < 0) return;

            var count = list.Count;
            if (size == count) return;

            if (size < count)
            {
                list.RemoveRange(size, count - size);
                return;
            }

            if (size > list.Capacity)
                list.Capacity = size;

            list.AddRange(Enumerable.Repeat(defaultValue, size - count));
        }
        
        public static void ResizeIList<T>(int length, ref IList<T> list)
        {
            if (list == null) return;

            if (length == list.Count) return;

            while (length > list.Count) list = AddIListElement(list);
            while (length < list.Count) list = RemoveIListElement(list, length - 1);
        }

        public static IList<T> AddIListElement<T>(IList<T> list, T value = default)
        {
            var length = list.Count;
            var newArray = new T[length];
            var i = 0;
            for (; i < length; i++) newArray[i] = list[i];
            try
            {
                newArray[i] = value;
            }
            catch
            {
                // ignored
            }

            return newArray;
        }

        public static IList<T> RemoveIListElement<T>(IList<T> list, int index)
        {
            var length = list.Count;
            if (length == 0) return list;
            var newArray = new T[length - 1];
            var offset = 0;
            for (var i = 0; i < length - 1; i++)
            {
                if (i == index)
                    offset = 1;

                newArray[i] = list[i + offset];
            }

            return newArray;
        }
        
        public static bool HashSetEquals<T>(ICollection<T> firstArray, ICollection<T> secondArray) =>
            firstArray != null && secondArray != null
                ? firstArray.Count == secondArray.Count && new HashSet<T>(firstArray).SetEquals(secondArray)
                : Equals(firstArray, secondArray);

        public static bool CompareAndSetArray<T>(ref T[] source, T[] toCompare)
        {
            if (source != null && HashSetEquals(toCompare, source)) return true;
            source = toCompare;
            return false;
        }

        public static bool SameArrayItems<T>(ref T[] array, T[] toCompare)
        {
            if (array == null)
            {
                if (toCompare == null)
                    return true;
                array = toCompare;
                return false;
            }

            var equals = true;
            for (var i = array.Length - 1; i >= 0; i--)
            {
                if (toCompare.Contains(array[i])) continue;
                array = RemoveArrayElement(array, i);
                equals = false;
            }

            foreach (var item in toCompare)
            {
                if (array.Contains(item)) continue;
                array = AddArrayElement(array, item);
                equals = false;
            }

            return equals;
        }

        public static int IndexOf<T>(IReadOnlyList<T> array, T value, int defaultValue = -1)
        {
            for (var i = 0; i < array.Count; i++)
                if (array[i].Equals(value))
                    return i;
            return defaultValue;
        }
    }
}