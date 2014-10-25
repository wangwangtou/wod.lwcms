using wod.lwcms.commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace wod.lwcms.coreTestProject
{
    
    
    /// <summary>
    ///这是 applyEditCommandTest 的测试类，旨在
    ///包含所有 applyEditCommandTest 单元测试
    ///</summary>
    [TestClass()]
    public class applyEditCommandTest
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
        ///applyEdit 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("wod.lwcms.core.dll")]
        public void applyEditTest()
        {
            applyEditCommand_Accessor target = new applyEditCommand_Accessor(); // TODO: 初始化为适当的值
            List<wod.lwcms.models.wodsite> obj = new List<wod.lwcms.models.wodsite> { new wod.lwcms.models.wodsite() { siteName = "siteName", title = "title" }, new wod.lwcms.models.wodsite() { siteName = "siteName1", title = "title1" } }; // TODO: 初始化为适当的值
            obj[1].navis.Add("aaa", new List<models.siteNavi>() {  });
            
            List<applyEditCommand.edit> edit = new List<applyEditCommand.edit>() {
                new applyEditCommand.edit{ type= applyEditCommand.editType.edit, dataexp="[0].siteName", value="abc"},
                new applyEditCommand.edit{ type= applyEditCommand.editType.insert, dataexp="[1]", value=""},
                new applyEditCommand.edit{ type= applyEditCommand.editType.edit, dataexp="[1].siteName", value="aaa"},
                new applyEditCommand.edit{ type= applyEditCommand.editType.edit, dataexp="[2].title", value="baa"},
                new applyEditCommand.edit{ type= applyEditCommand.editType.insert, dataexp="[2].navis[aaa].[0]", value=""},
                new applyEditCommand.edit{ type= applyEditCommand.editType.edit, dataexp="[2].navis[aaa].[0].name", value="navname"},
                new applyEditCommand.edit{ type= applyEditCommand.editType.insert, dataexp="[2].navis[aaa].[1]", value=""},
            }; // TODO: 初始化为适当的值
            target.applyEdit(obj, edit);
            Assert.AreEqual("abc", obj[0].siteName);
            Assert.AreEqual(3, obj.Count);
            Assert.AreEqual("aaa", obj[1].siteName);
            Assert.AreEqual("siteName1", obj[2].siteName);
            Assert.AreEqual("baa", obj[2].title);
            Assert.AreEqual("navname", obj[2].navis["aaa"][0].name);
            Assert.AreEqual(2, obj[2].navis["aaa"].Count);
        }
    }
}
