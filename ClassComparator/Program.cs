using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassComparator
{
    class Program
    {
        static void Main(string[] args)
        {
            var d1 = PrepareFirstExample();
            var _d1 = PrepareFirstExample();
            DiffferenceDtos d2 = PrepareSecondExample();

            Console.WriteLine(CompareObjects(d1, d2));
            Console.WriteLine(CompareObjects(d1, _d1));

            var diff1 = ListDifferenceOnObjects(d1, _d1);
            var diff2 = ListDifferenceOnObjects(d1, d2);

            foreach (var str in diff1)
            {
                Console.WriteLine(str);
            }

            foreach (var str in diff2)
            {
                Console.WriteLine(str);
            }

            Console.ReadKey();
        }

        static IList<string> ListDifferenceOnObjects(object first, object second, IList<string> result = null)
        {
            var endline = false;
            var typeFirst = first.GetType();
            var typeSecond = second.GetType();
            var listFirst = first as System.Collections.ICollection;
            var listSecond = second as System.Collections.ICollection;

            if (result == null)
            {
                result = new List<string>();
                endline = true;
                result.Add($"Beginning of comparing {first} and {second}");
            }

            if (Object.ReferenceEquals(first, second))
                return result;

            if (listFirst != null)
            {
                var aEnumerator = listFirst.GetEnumerator();
                var bEnumerator = listSecond.GetEnumerator();

                while (aEnumerator.MoveNext() && bEnumerator.MoveNext())
                {
                    var f = aEnumerator.Current;
                    var s = bEnumerator.Current;

                    ListDifferenceOnObjects(f, s, result);
                }
            }

            if (typeFirst.IsPrimitive)
            {
                if (!first.Equals(second))
                {
                    result.Add($"Expected {first} but found {second}");
                }
            }
            else
            {
                var properties = typeFirst.GetProperties().Where(x => x.GetMethod != null);
                foreach (var property in properties)
                {
                    var f = property.GetValue(first);
                    var s = property.GetValue(second);

                    ListDifferenceOnObjects(f, s, result);
                }

            }

            if (endline)
            {
                result.Add($"End of comparing {first} and {second}");
            }

            return result;
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

        private static DiffferenceDtos PrepareSecondExample()
        {
            return new DiffferenceDtos()
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
        }

        private static DiffferenceDtos PrepareFirstExample()
        {
            return new DiffferenceDtos()
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
