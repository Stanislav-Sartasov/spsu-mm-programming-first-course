using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.Factory;
using TestStack.White.UIItems.ListBoxItems;
using TestStack.White.UIItems.WindowItems;

namespace UnitTestCurve
{
	[TestClass]
	public class UnitTest
	{
        [TestMethod]
        public void TestWinForms()
        {
            Application app = Application.Launch("WindowsForms.exe");

            Window window = app.GetWindow("winForm", InitializeOption.NoCache);

            Button button = window.Get<Button>("buttonStart");
            Button buttonPlus = window.Get<Button>("plusSize");
            Button buttonMinus = window.Get<Button>("minusSize");
            Label label = window.Get<Label>("labelSize");

            ComboBox comboBox = window.Get<ComboBox>("comboBoxCurves");
            comboBox.Select(0);
            for (var i = 1; i < 7; i++)
            {
                buttonMinus.Click();
                button.Click();
            }

            comboBox.Select(1);
            for (var i = 1; i < 15; i++)
            {
                buttonPlus.Click();
                button.Click();
            }

            comboBox.Select(2);
            for (var i = 1; i < 10; i++)
            {
                buttonMinus.Click();
            }
            button.Click();
            buttonPlus.Click();
            button.Click();

            app.Close();
        }
        [TestMethod]
        public void TestWpf()
        {
            Application app = Application.Launch("Wpf.exe");
            Window window = app.GetWindow("Show_Curve", InitializeOption.NoCache);

            Button button = window.Get<Button>("buttonStart");
            Button buttonPlus = window.Get<Button>("buttonPlus");
            Button buttonMinus = window.Get<Button>("buttonMinus");
            Label label = window.Get<Label>("labelSize");

            ComboBox comboBox = window.Get<ComboBox>("comboBoxCurves");
            comboBox.Select(0);
            for (var i = 1; i < 7; i++)
            {
                buttonMinus.Click();
                button.Click();
            }

            comboBox.Select(1);
            for (var i = 1; i < 15; i++)
            {
                buttonPlus.Click();
                button.Click();
            }

            comboBox.Select(2);
            for (var i = 1; i < 10; i++)
            {
                buttonMinus.Click();
                button.Click();
            }

            app.Close();
        }
    }
}
