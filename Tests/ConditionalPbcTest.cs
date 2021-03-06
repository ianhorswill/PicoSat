﻿#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PseudoSMT.cs" company="Ian Horswill">
// Copyright (C) 2018 Ian Horswill
//  
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in the
// Software without restriction, including without limitation the rights to use, copy,
// modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,
// and to permit persons to whom the Software is furnished to do so, subject to the
// following conditions:
//  
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
using CatSAT;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class ConditionalPbcTest
    {
        [TestMethod]
        public void ConditionalTest1()
        {
            var p = new Problem("MultipleCardinality");
            p.QuantifyIf("a", 2, 2, "a", "c");
            p.AddClause(1, 1, "b", "c");
            for (int i = 0; i < 1000; i++)
            {
                var m = p.Solve();
                Assert.IsTrue(m.Model.Equals("{a, c}") | m.Model.Equals("{b}") | m.Model.Equals("{c}"));
                Assert.IsFalse(m.Model.Equals("{b, c}"));
            }
        }

        [TestMethod]
        public void ConditionalTest2()
        {
            var p = new Problem("Compare with test 1");
            p.AddClause(2, 2, "a", "c");
            p.AddClause(1, 1, "b", "c");
            for (int i = 0; i < 1000; i++)
            {
                var m = p.Solve();
                Assert.IsFalse(m.IsTrue("b"));
            }
        }

        [TestMethod]
        public void ConditionalTest3()
        {
            var p = new Problem("SingleClause");
            p.QuantifyIf("a", 2, 2, "b", "c");
            for (int i = 0; i < 100; i++)
            {
                var m = p.Solve();
                Assert.IsFalse(m.IsTrue("a") & !m.IsTrue("b") & !m.IsTrue("c"));
            }
        }

        [TestMethod]
        public void ConditionalTest4()
        {
            var p = new Problem("SingleClause2");
            p.QuantifyIf("a", 1, 1, "b", "c");
            for (int i = 0; i < 100; i++)
            {
                var m = p.Solve();
                Assert.IsFalse(m.IsTrue("a") & !m.IsTrue("b") & !m.IsTrue("c"));
                Assert.IsFalse(m.IsTrue("a") & m.IsTrue("b") & m.IsTrue("c"));
                Assert.IsTrue(m.Model.Equals("{}") | m.Model.Equals("{a}") | m.Model.Equals("{b}") | m.Model.Equals("{c}") | m.Model.Equals("{a, b}") | m.Model.Equals("{a, c}") | m.Model.Equals("{b, c}"));
            }
        }

        [TestMethod]
        public void ConditionalTest5()
        {
            var p = new Problem("MultipleCardinality2");
            p.AddClause(10, 29, "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "ll", "mm", "nn", "oo");
            p.AddClause(20, 29, "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "p", "q", "r", "s", "t", "aa", "bb", "cc", "dd", "ee", "ff", "gg", "hh", "ii", "jj", "pp", "qq", "rr", "ss", "tt");
            p.AddClause(10, 29, "a", "b", "c", "d", "e", "f", "g", "h", "i", "t", "u", "v", "w", "x", "y", "z", "aa", "bb", "cc", "dd", "ee", "ff", "gg", "hh", "ii", "jj", "kk", "ll", "mm", "nn", "oo");
            p.AddClause(1, 1, "e");
            p.QuantifyIf("a", 2, 2, "a", "c");
            p.AddClause(1, 1, "b", "c");
            p.AddClause(1, 1, "d");
            for (int i = 0; i < 100; i++)
            {
                var m = p.Solve();
                Assert.IsTrue(m.IsTrue("e"));
                Assert.IsTrue(m.IsTrue("d"));
                Assert.IsFalse(m.Model.Equals("{e, c, b}"));
                Assert.IsFalse(m.Model.Equals("{e, c, b, d}"));
                Assert.IsFalse(m.Model.Equals("{e, a, b}"));
            }
        }


    }
}
