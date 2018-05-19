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

            Console.WriteLine(CustomComparator.CompareObjects(d1, d2));
            Console.WriteLine(CustomComparator.CompareObjects(d1, _d1));

            var diff1 = CustomComparator.ListDifferenceOnObjects(d1, _d1);
            var diff2 = CustomComparator.ListDifferenceOnObjects(d1, d2);

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
