using System;
using System.Collections.Generic;
using System.Text;
using Id3Parser.ID3v2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Id3Parser.MSTest;

[TestClass]
public class ID3v2FrameHeader_Test
{
        [TestMethod]
        public void ComplexTest()
        {
            var h = new FrameHeader(new byte[] { 0x50, 0x4f, 0x50, 0x4d, 0x00, 0x00, 0x00, 0x1f, 0x00, 0x00 });
            Assert.AreEqual(31, h.Length);
            Assert.AreEqual("POPM", h.FrameID);
        }
    }
