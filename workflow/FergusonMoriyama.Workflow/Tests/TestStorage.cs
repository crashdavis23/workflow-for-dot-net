﻿using Moriyama.Workflow.Infrastructure;
using Moriyama.Workflow.Interfaces;
using Moriyama.Workflow.Interfaces.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Moriyama.Workflow.Tests
{
    [TestClass]
    public class TestStorage
    {
        [TestMethod]
        public void TestWrite()
        {
            IStorage f = TempFileStorage.Instance;

            var writer = f.GetWriter("1");
            writer.WriteLine("Hello world");
            writer.Close();
        }

        [TestMethod]
        public void TestRead()
        {
            IStorage f = TempFileStorage.Instance;

            var writer = f.GetWriter("2");
            writer.WriteLine("Hello world");
            writer.Close();

            var reader = f.GetReader("2");
            var s = reader.ReadToEnd();
            reader.Close();

            Assert.IsTrue(s.Length > 0);
        }

        public void TestDelete()
        {
            IStorage f = TempFileStorage.Instance;

            var writer = f.GetWriter("2");
            writer.WriteLine("Hello world");
            writer.Close();

           f.Delete("2");
        }

    }
}
