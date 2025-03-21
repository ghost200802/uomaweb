using System;
using System.Threading.Tasks;

namespace UomaWeb
{
    public class TestManager
    {
        private readonly WebApiTest _webApiTest;
        private readonly GameHelperTest _gameHelperTest;

        public TestManager()
        {
            _webApiTest = new WebApiTest();
            _gameHelperTest = new GameHelperTest();
        }

        public async Task RunTest()
        {
            while (true)
            {
                Console.WriteLine("\n请选择测试类型：");
                Console.WriteLine("1. WebAPI测试");
                Console.WriteLine("2. GameHelper测试");
                Console.WriteLine("3. 退出");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await _webApiTest.RunTest();
                        break;

                    case "2":
                        await _gameHelperTest.RunTest();
                        break;

                    case "3":
                        return;

                    default:
                        Console.WriteLine("无效的选择");
                        break;
                }
            }
        }
    }
}