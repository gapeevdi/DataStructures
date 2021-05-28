using System;
using NUnit.Framework;

namespace Tests.AVLTreeTests.Extensions
{
    public static class AssertExtensions
    {
        public static class AVL
        {
            private static bool CheckHeight(int numberOfNodes, int actualHeight) =>
                actualHeight >= Math.Floor(Math.Log2(numberOfNodes))
                && actualHeight <= Math.Floor(1.45 * Math.Log2(numberOfNodes + 2));

            public static void IsHeightValid(int numberOfNodes, int actualHeight) =>
                Assert.IsTrue(CheckHeight(numberOfNodes, actualHeight));
        }
    }
}