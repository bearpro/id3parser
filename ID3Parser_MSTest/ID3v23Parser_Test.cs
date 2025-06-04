using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Id3Parser.MSTest;

[TestClass]
public class ID3v23Parser_Test
{
    [TestMethod]
    public void ComplexTest1()
    {
        var path = @"C:\Users\BearPro\source\Workspaces\Id3Parser\Id3Parser\example\2017.11.13 _ Ботанический Сад Петра Великого — Что-то не так.mp3";
        using FileStream f = File.OpenRead(path);
        var m = V23Parser.Parse(f);
        Assert.AreEqual(m.Title, "﻿Что-то не так");
        Console.WriteLine($"\"{m.Title}\" parsed without exceptions.");
    }

    [TestMethod]
    public void ComplexTest2()
    {
        var path = @"C:\Users\BearPro\source\Workspaces\Id3Parser\Id3Parser\example\2018.01.05 _ The Clash — I Fought the Law.mp3";
        using FileStream f = File.OpenRead(path);
        var m = V23Parser.Parse(f);
        Console.WriteLine($"\"{m.Title}\" parsed without exceptions.");
    }

    [TestMethod]
    public void ComplexTest3()
    {
        var path = @"C:\Users\BearPro\source\Workspaces\Id3Parser\Id3Parser\example\2018.01.05 _ Useless ID — Land of Idiocracy.mp3";
        using FileStream f = File.OpenRead(path);
        var m = V23Parser.Parse(f);
        Console.WriteLine($"\"{m.Title}\" parsed without exceptions.");
    }

    [TestMethod]
    public void ComplexTest4()
    {
        var path = @"C:\Users\BearPro\source\Workspaces\Id3Parser\Id3Parser\example\2018.11.15 _ Пурген — 1984-1988.mp3";
        using FileStream f = File.OpenRead(path);
        var m = V23Parser.Parse(f);
        Console.WriteLine($"\"{m.Title}\" parsed without exceptions.");
    }
}
