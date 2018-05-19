using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassComparator
{

    public interface ICustomComparator
    {
        IList<string> ListDifferenceOnObjects(object first, object second, IList<string> result = null, string firstName = "", string secondName = "");

        bool CompareObjects(object first, object second);
    }
}