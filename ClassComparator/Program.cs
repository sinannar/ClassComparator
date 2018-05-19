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

            // Console.ReadKey();
        }



        static IList<string> ListDifferenceOnObjects(object first, object second, IList<string> result = null, string firstName = "", string secondName = "")
        {
            var endline = false;
            var typeFirst = first.GetType();
            var typeSecond = second.GetType();
            var listFirst = first as System.Collections.ICollection;
            var listSecond = second as System.Collections.ICollection;

            // var members = (typeFirst).GetMembers();
            // foreach (var memberInfo in members.Where(p => (int)p.MemberType == 16))
            // {
            //     Console.WriteLine("Name: {0}", memberInfo.Name); // Name: MyField
            //     Console.WriteLine("Member Type: {0}", memberInfo.MemberType); // Member Type: Property
            // }

            if (result == null)
            {

                firstName = nameof(typeFirst);
                secondName = nameof(typeSecond);
            }
            else
            {
                firstName = $"{firstName}.{nameof(typeFirst)}";
                secondName = $"{secondName}.{nameof(typeSecond)}";
            }

            // Adding start line
            if (result == null)
            {
                result = new List<string>();
                endline = true;
                result.Add($"Beginning of comparing {first} and {second}");
            }

            // If primitive type
            if ((typeFirst.IsPrimitive || typeFirst == typeof(string))
                && !first.Equals(second))
            {
                if (typeFirst.IsEnum)
                {
                    //NEED TO FIX TO SHOW ENUM CHARACTERS OR WHATEVER
                    var f = typeFirst.GetEnumValues().GetValue((int)first).ToString();
                    var s = typeFirst.GetEnumValues().GetValue((int)second).ToString();
                    result.Add($"{firstName}: {f} expected but found {s}");
                }
                else
                {
                    result.Add($"{firstName}: {first} expected but found {second}");
                }
            }
            // If list
            else if (listFirst != null)
            {
                var aEnumerator = listFirst.GetEnumerator();
                var bEnumerator = listSecond.GetEnumerator();
                while (aEnumerator.MoveNext() && bEnumerator.MoveNext())
                {
                    var f = aEnumerator.Current;
                    var s = bEnumerator.Current;
                    ListDifferenceOnObjects(f, s, result, firstName, secondName);
                }
            }
            // If complex type
            else if (typeFirst.IsClass && typeFirst != typeof(string))
            {
                var properties = typeFirst.GetProperties().Where(x => x.GetMethod != null);
                foreach (var property in properties)
                {
                    var f = property.GetValue(first);
                    var s = property.GetValue(second);
                    ListDifferenceOnObjects(f, s, result, firstName, secondName);
                }
            }

            // Adding end line
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
                    E = Enm.A,
                    primitiveInt = 2,
                    stringProperty = "223",
                    arrayProperty = new int[] { 2, 2, 3 },
                    listProperty = new List<int>() { 3, 7, 8 }
                },
                sent = new Dto()
                {
                    E = Enm.B,
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
                    E = Enm.C,
                    primitiveInt = 1,
                    stringProperty = "123",
                    arrayProperty = new int[] { 1, 2, 3 },
                    listProperty = new List<int>() { 4, 7, 8 }
                },
                sent = new Dto()
                {
                    E = Enm.B,
                    primitiveInt = 2,
                    stringProperty = "123",
                    arrayProperty = new int[] { 2, 3, 4 },
                    listProperty = new List<int>() { 1, 2, 3 }
                },
            };
        }

    }

    enum Enm
    {
        A,
        B,
        C
    }

    class Dto
    {
        public Enm E { get; set; }
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
