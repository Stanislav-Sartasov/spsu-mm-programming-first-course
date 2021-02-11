using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExamSystem;
using ExamSystem.Locks;
using System;

namespace ExamSystemTests
{
    [TestClass]
    public class LazyExamSystemTest
    {
        readonly int tasksNumber = 10;
        readonly int requestsNumber = 50;


        [TestMethod]
        public void CorrectAdd()
        {
            int infosNumber = tasksNumber * requestsNumber;
            var expectedValues = TestMethods.GetTestValues(infosNumber);
            var examSystem = new LazyExamSystem(16, new MutexLockFactory());
            TestMethods.AddValues(examSystem, tasksNumber, expectedValues);
            var infos = examSystem.GetAllData();

            Assert.IsTrue(infos.Count == infosNumber && examSystem.Size == infosNumber);

            for (int i = 0; i < infosNumber; i++)
            {
                var studentId = expectedValues[i].Item1;
                var courseId = expectedValues[i].Item2;
                Assert.IsTrue(examSystem.Contains(studentId, courseId) && 
                    infos.Contains(new ExamInfo(studentId, courseId)));
            }
        }

        [TestMethod]
        public void CorrectDelete()
        {
            int infosNumber = tasksNumber * requestsNumber;
            var expectedValues = TestMethods.GetTestValues(infosNumber);
            var examSystem = new LazyExamSystem(16, new MutexLockFactory());
            TestMethods.AddValues(examSystem, tasksNumber, expectedValues);
            TestMethods.RemoveValues(examSystem, tasksNumber, expectedValues);

            var infos = examSystem.GetAllData();
            Assert.IsTrue(infos.Count == 0 && examSystem.Size == 0);

            for (int i = 0; i < tasksNumber * requestsNumber; i++)
            {
                var studentId = expectedValues[i].Item1;
                var courseId = expectedValues[i].Item2;
                Assert.IsFalse(examSystem.Contains(studentId, courseId) || 
                    infos.Contains(new ExamInfo(studentId, courseId)));
            }
        }
    }
}
