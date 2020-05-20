using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plugins.FirstDLLClasses;
using Plugins.SecondDLLClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Plugins.Tests
{
    [TestClass]
    public class Tests
    {
        private string path = Directory.GetCurrentDirectory() + @"\.." + @"\.." + @"\.." + @"\.." + @"\DLLFiles\";

        private bool Comparator(Type a, Type b)
        {
            bool isEqual = true;

            string aFullName = a.FullName;
            string bFullName = b.FullName;
            if (!(aFullName.Equals(bFullName)))
                isEqual = false;

            var aFields = a.GetFields();
            var bFields = b.GetFields();
            if (aFields.Length != bFields.Length)
                isEqual = false;
            else
                for (int i = 0; i < aFields.Length; i++)
                {
                    string aFieldName = aFields[i].Name;
                    string bFieldName = bFields[i].Name;
                    string aFieldType = aFields[i].FieldType.FullName;
                    string bFieldType = bFields[i].FieldType.FullName;
                    if (!(aFieldName.Equals(bFieldName) || aFieldType.Equals(bFieldType)))
                    {
                        isEqual = false;
                        break;
                    }
                }

            var aMethods = a.GetMethods();
            var bMethods = b.GetMethods();
            if (aMethods.Length != bMethods.Length)
                isEqual = false;
            else
                for (int i = 0; i < aMethods.Length; i++)
                {
                    string aMethodName = aMethods[i].Name;
                    string bMethodName = bMethods[i].Name;
                    string aMethodReturnValue = aMethods[i].ReturnType.FullName;
                    string bMethodReturnValue = bMethods[i].ReturnType.FullName;
                    if (!(aMethodName.Equals(bMethodName) || aMethodReturnValue.Equals(bMethodReturnValue)))
                    {
                        isEqual = false;
                        break;
                    }

                    ParameterInfo[] aMethodParams = aMethods[i].GetParameters();
                    ParameterInfo[] bMethodParams = bMethods[i].GetParameters();
                    if (aMethodParams.Length != bMethodParams.Length)
                    {
                        isEqual = false;
                        break;
                    }
                    else
                        for (int j = 0; j < aMethodParams.Length; j++)
                        {
                            string aParameter = aMethodParams[j].ParameterType.FullName;
                            string bParameter = bMethodParams[j].ParameterType.FullName;
                            if (!(aParameter.Equals(bParameter)))
                            {
                                isEqual = false;
                                break;
                            }
                        }
                }

            return isEqual;
        }

        [TestMethod]
        public void TestTypes()
        {
            LibreryAnalysis libreryAnalysis = new LibreryAnalysis(path);
            List<Type> temp = libreryAnalysis.ShowTypesInFiles();

            foreach (var o in temp)
            {
                if (!(Comparator(o, typeof(IceCream)) || Comparator(o, typeof(Cake))))
                    Assert.Fail();
            }
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void TestLenght()
        {
            LibreryAnalysis libreryAnalysis = new LibreryAnalysis(path);

            int expected = 2;
            int actual = libreryAnalysis.Length();

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void CreationTesting()
        {
            LibreryAnalysis libreryAnalysis = new LibreryAnalysis(path);

            for (int i = 0; i < libreryAnalysis.Length(); i++)
            {
                var o = libreryAnalysis.Instantiation(i);
                Assert.IsNotNull(o);
            }
        }
    }
}
