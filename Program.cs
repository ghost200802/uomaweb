using System;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using UomaWeb.Models;
using Newtonsoft.Json;
using UomaWeb;

#if !UNITY_2017_1_OR_NEWER

class Program
{
    static async Task Main(string[] args)
    {
        var testManager = new TestManager();
        await testManager.RunTest();
    }
}

#endif