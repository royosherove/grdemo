using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace PivotIndex
{
    /*
     Write a method that returns the "pivot" index of a list of integers.

    1) We define the pivot index as the index where the sum of the numbers on
        the left is equal to the sum of the numbers on the right. Given [1, 4,
        6, 3, 2], the method should return 2, since the sum of the numbers to
        the left of index 2 is equal to the sum of numbers to the right of
        index 2 (1 + 4 = 3 + 2). 
      
     2) If no such index exists, it should return -1.

     3) If there are multiple pivots, you can return the left-most pivot.
    For example: [3,0,0,3] index: 1 and 2: 3 = 0+3 and 3+0=3


You can write the method in any language. Make sure that the method:
   • runs successfully
   • handles all edge cases
   • is as efficient as you can make it!

A successful answer will fulfill the above criteria.
     */

    [TestFixture]
    public class PivtoTests
    {
        [TestCase(1,4,6,3,2 ,2)]
        [TestCase(3,3,1,1,1 ,1)]
        [TestCase(1,4,0,3,2 ,2)]
        [TestCase(1,4,1,3,2 ,2)]
        [TestCase(1,4,7,3,2 ,2)]
        [TestCase(1,4,1     ,1)]
        public void GetPivot_Simple_FindsPivot(params int[] theinputWithExpected)
        {
            int[] input = theinputWithExpected.Take(theinputWithExpected.Length-1).ToArray();
            int expected = theinputWithExpected.Last();

            int result = Finder.GetPivotFast(input);

            Assert.AreEqual(expected,result);
        }

        [TestCase(3,0,0,3     ,1)]
        [TestCase(3,1,1,1,0,0,6     ,4)]
        public void GetPivot_Multiple_ReturnsLeftMost(params int[] theinputWithExpected)
        {
            int[] input = theinputWithExpected.Take(theinputWithExpected.Length-1).ToArray();
            int expected = theinputWithExpected.Last();

            int result = Finder.GetPivotFast(input);

            Assert.AreEqual(expected,result);
        }

        [TestCase(1,4,0,3   ,-1)]
        [TestCase(1,4,0     ,-1)]
        [TestCase(1,4       ,-1)]
        [TestCase(1         ,-1)]
        public void GetPivot_NonExists_ReturnsdefaultValue(params int[] theinputWithExpected)
        {
            int[] input = theinputWithExpected.Take(theinputWithExpected.Length-1).ToArray();
            int expected = theinputWithExpected.Last();

            int result = Finder.GetPivotFast(input);

            Assert.AreEqual(expected,result);
        }
    }

    public class Finder
    {
        class ItemDetail
        {
            public int SumBefore;
            public int SumAfter;
        }
        public static int GetPivotFast(int[] input)
        {
            var occurences = new Dictionary<int, ItemDetail>(input.Length);

            int sum=0;
            int sumReverse=0;

            for (int fwIndex = 0; fwIndex < input.Length; fwIndex++)
            {
                int reverseIndex = input.Length - fwIndex-1;
                sum += input[fwIndex];
                sumReverse += input[reverseIndex];

                var bwPivotIndex = reverseIndex - 1;
                if (!occurences.ContainsKey(bwPivotIndex))
                {
                    occurences.Add(bwPivotIndex, new ItemDetail {SumAfter = sumReverse});
                }
                else
                {
                    var detail = occurences[bwPivotIndex];
                    detail.SumAfter = sumReverse;
                    if (detail.SumBefore == detail.SumAfter)
                    {
                        return bwPivotIndex;
                    }
                }

                var fwPivotIndex = fwIndex + 1;
                if (!occurences.ContainsKey(fwPivotIndex))
                {
                    occurences.Add(fwPivotIndex, new ItemDetail() {SumBefore = sum});
                }
                else
                {
                    var detail = occurences[fwPivotIndex];
                    detail.SumBefore = sum;
                    if (detail.SumBefore == detail.SumAfter)
                    {
                        return fwPivotIndex;
                    }
                }
            }
            return -1;
        }

        public static int GetPivotSlow(int[] input)
        {
            if (input.Length<3)
            {
                return -1;
            }

            for (int i = 0; i < input.Length; i++)
            {
                int sumBefore = input.Take(i).Sum();
                int sumAfter = input.Skip(i + 1).Sum();

                if (sumBefore == sumAfter)
                {
                    return i;
                }
            }

            return -1;
        }

    }
}
