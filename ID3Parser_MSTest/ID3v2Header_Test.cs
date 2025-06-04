using Id3Parser.ID3v2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace Id3Parser.MSTest;

[TestClass]
public class ID3v2Header_Test
{
    [TestMethod]
    public void TestParseLength()
    {
        var h = new TagHeader();
        var t = typeof(TagHeader);
        var result = (int)t.InvokeMember(
            name: "ParseLength",
            invokeAttr: BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
            binder: null,
            target: h,
            args: new object[] { new byte[] { 0x00, 0x00, 0x07, 0x76 } }
        );
        Assert.AreEqual(1014, result, "Length not parsed correctly");
        Assert.ThrowsException<TargetInvocationException>(() =>
        {
            t.InvokeMember(
                 name: "ParseLength",
                 invokeAttr: BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
                 binder: null,
                 target: h,
                 args: new object[] { new byte[] { 0x00, 0x00, 0x00, 0x07, 0x76 } }
            );
        }, "Not throws ArgumentException with 5-bytes given");
    }

    [TestMethod]
    public void TestValidateHeader()
    {
        var h = new TagHeader();
        var t = typeof(TagHeader);
        Assert.IsTrue((bool)t.InvokeMember(
            name: "ValidateHeader",
            invokeAttr: BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
            binder: null,
            target: h,
            args: new object[] { new byte[] { 0x49, 0x44, 0x33, 0x03, 0x00, 0x00, 0x00, 0x00, 0x07, 0x76 } }
        ));
        Assert.IsFalse((bool)t.InvokeMember(
            name: "ValidateHeader",
            invokeAttr: BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
            binder: null,
            target: h,
            args: new object[] { new byte[] { 0x49, 0x45, 0x33, 0x03, 0x00, 0x00, 0x00, 0x00, 0x07, 0x76 } }
        ));
    }

    [TestMethod]
    public void ComplexTest()
    {
        var h = new TagHeader(new byte[] { 0x49, 0x44, 0x33, 0x03, 0x00, 0x00, 0x00, 0x00, 0x07, 0x76 });
        Assert.AreEqual(1014, h.Length);
        Assert.AreEqual(3, h.Version);
    }
}
