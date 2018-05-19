using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassComparator
{

    public static class CustomComparator
    {
        public static IList<string> ListDifferenceOnObjects(object first, object second, IList<string> result = null, string firstName = "", string secondName = "")
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

        public static bool CompareObjects(object first, object second)
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
}