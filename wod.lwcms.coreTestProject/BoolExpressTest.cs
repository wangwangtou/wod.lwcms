using wod.lwcms.commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace wod.lwcms.coreTestProject
{
    
    
    /// <summary>
    ///这是 BoolExpressTest 的测试类，旨在
    ///包含所有 BoolExpressTest 单元测试
    ///</summary>
    [TestClass()]
    public class BoolExpressTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        // 
        //编写测试时，还可使用以下特性:
        //
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //使用 TestInitialize 在运行每个测试前先运行代码
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //使用 TestCleanup 在运行完每个测试后运行代码
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion
        
        /// <summary>
        ///CreateBoolExpress 的测试
        ///</summary>
        [TestMethod()]
        public void GetJsString()
        {
            string fileName = @"F:\Vs\历史项目\freesite\game56\hex.txt";
            string jsFile = @"G:\codes\smallhtmlcodes\freelancer_modifies\game5\ss.js";

            string[] hex = System.IO.File.ReadAllLines(fileName);
            string js = System.IO.File.ReadAllText(jsFile);
            foreach (var item in hex)
            {
                var i = item.Split(':');
                if (i.Length > 1) {
                    System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("\\[_0x6332\\[" + i[0] + "\\]\\]");
                    js = reg.Replace(js, "." + i[1]);
                    System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex("_0x6332\\[" + i[0] + "\\]");
                    js = reg1.Replace(js, "\"" + i[1].Replace("\"","\\\"")+"\"");
                }
            }
            Assert.Fail();
        }

        /// <summary>
        ///CreateBoolExpress 的测试
        ///</summary>
        [TestMethod()]
        public void CreateBoolExpressTest()
        {
            string exp = "@a == b"; // TODO: 初始化为适当的值
            BoolExpress actual;
            actual = BoolExpress.CreateBoolExpress(exp);
            var parameter = new System.Collections.Generic.Dictionary<string, object>() {
                {"a","b"}
            };
            Assert.IsTrue(actual.GetExpressResult(parameter));
            parameter["a"] = "c";
            Assert.IsFalse(actual.GetExpressResult(parameter));

            exp = "@a == true"; // TODO: 初始化为适当的值
            actual = BoolExpress.CreateBoolExpress(exp);
            parameter["a"] = true;
            Assert.IsTrue(actual.GetExpressResult(parameter));
            parameter["a"] = false;
            Assert.IsFalse(actual.GetExpressResult(parameter));


            exp = "@a == true && @b == 1"; // TODO: 初始化为适当的值
            parameter["a"] = true;
            parameter.Add("b", 1);
            actual = BoolExpress.CreateBoolExpress(exp);
            Assert.IsTrue(actual.GetExpressResult(parameter));
            parameter["b"] = 2;
            Assert.IsFalse(actual.GetExpressResult(parameter));


            exp = "@a < 5  && @b >= 3"; // TODO: 初始化为适当的值
            actual = BoolExpress.CreateBoolExpress(exp);
            parameter["a"] = 2;
            parameter["b"] = 3;
            Assert.IsTrue(actual.GetExpressResult(parameter));
            parameter["a"] = 3;
            parameter["b"] = 4;
            Assert.IsTrue(actual.GetExpressResult(parameter));
            parameter["a"] = 5;
            parameter["b"] = 3;
            Assert.IsFalse(actual.GetExpressResult(parameter));
            parameter["a"] = 5;
            parameter["b"] = 4;
            Assert.IsFalse(actual.GetExpressResult(parameter));



            exp = "@a < 5  || @b >= 3"; // TODO: 初始化为适当的值
            actual = BoolExpress.CreateBoolExpress(exp);
            parameter["a"] = 2;
            parameter["b"] = 3;
            Assert.IsTrue(actual.GetExpressResult(parameter));
            parameter["a"] = 5;
            parameter["b"] = 3;
            Assert.IsTrue(actual.GetExpressResult(parameter));
            parameter["a"] = 5;
            parameter["b"] = 4;
            Assert.IsTrue(actual.GetExpressResult(parameter));


            exp = "(@a < 5  || @b >= 3) && @c == false"; // TODO: 初始化为适当的值
            parameter.Add("c", false);
            actual = BoolExpress.CreateBoolExpress(exp);
            parameter["a"] = 2;
            parameter["b"] = 3;
            Assert.IsTrue(actual.GetExpressResult(parameter));
            parameter["a"] = 5;
            parameter["b"] = 3;
            Assert.IsTrue(actual.GetExpressResult(parameter));
            parameter["a"] = 5;
            parameter["b"] = 4;
            Assert.IsTrue(actual.GetExpressResult(parameter));
        }
    }
}
