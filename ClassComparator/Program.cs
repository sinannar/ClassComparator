using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassComparator
{
    class Program
    {
        static void Main(string[] args)
        {
            var d1 = new DiffferenceDtos()
            {
                current = new Dto()
                {
                    primitiveInt = 1,
                    stringProperty = "123",
                    arrayProperty = new int[] { 1, 2, 3 },
                    listProperty = new List<int>() { 4, 7, 8 }
                },
                sent = new Dto()
                {
                    primitiveInt = 2,
                    stringProperty = "123",
                    arrayProperty = new int[] { 2, 3, 4 },
                    listProperty = new List<int>() { 1, 2, 3 }
                },
            };

            var _d1 = new DiffferenceDtos()
            {
                current = new Dto()
                {
                    primitiveInt = 1,
                    stringProperty = "123",
                    arrayProperty = new int[] { 1, 2, 3 },
                    listProperty = new List<int>() { 4, 7, 8 }
                },
                sent = new Dto()
                {
                    primitiveInt = 2,
                    stringProperty = "123",
                    arrayProperty = new int[] { 2, 3, 4 },
                    listProperty = new List<int>() { 1, 2, 3 }
                },
            };

            var d2 = new DiffferenceDtos()
            {
                current = new Dto()
                {
                    primitiveInt = 2,
                    stringProperty = "223",
                    arrayProperty = new int[] { 2, 2, 3 },
                    listProperty = new List<int>() { 3, 7, 8 }
                },
                sent = new Dto()
                {
                    primitiveInt = 4,
                    stringProperty = "223",
                    arrayProperty = new int[] { 2, 2, 4 },
                    listProperty = new List<int>() { 1, 3, 3 }
                },
            };

            Console.WriteLine(CompareObjects(d1, d2));
            Console.WriteLine(CompareObjects(d1, _d1));
            Console.ReadKey();
        }

        static bool CompareObjects(object first, object second)
        {
            var typeFirst = first.GetType();
            var typeSecond = second.GetType();
            var listFirst = first as System.Collections.ICollection;
            var listSecond = second as System.Collections.ICollection;

            if (Object.ReferenceEquals(first, second))
                return true;

            if (typeFirst != typeSecond)
                return false;

            if (listFirst != null)
            {
                if (listFirst.Count != listSecond.Count)
                    return false;

                var aEnumerator = listFirst.GetEnumerator();
                var bEnumerator = listSecond.GetEnumerator();

                while (aEnumerator.MoveNext() && bEnumerator.MoveNext())
                {
                    if (!CompareObjects(aEnumerator.Current, bEnumerator.Current))
                        return false;
                }
                return true;
            }

            if (typeFirst.IsPrimitive)
            {
                return first.Equals(second);
            }
            else
            {
                var properties = typeFirst.GetProperties().Where(x => x.GetMethod != null);
                foreach (var property in properties)
                {
                    if (!CompareObjects(property.GetValue(first), property.GetValue(second)))
                        return false;
                }

                return true;
            }

        }
    }


    class Dto
    {
        public int primitiveInt { get; set; }
        public string stringProperty { get; set; }
        public List<int> listProperty { get; set; }
        public int[] arrayProperty { get; set; }
    }

    class DiffferenceDtos
    {
        public Dto current { get; set; }
        public Dto sent { get; set; }

    }
}
