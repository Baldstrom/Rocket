/// <summary>
/// Module Name: ArgParserTests.cs
/// Project: RocketSimulator
/// Nikobotics Software
///
/// Holds structure for testing ArgParser validity
/// 
/// This source is subject to the Apache License 2.0.
/// See https://www.apache.org/licenses/LICENSE-2.0.
/// </summary>
namespace RocketSimulator.CommandLine.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RocketSimulator.CommandLine;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TestClass]
    #pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class ArgParserTests
    {
        private bool TestDelegateHelperFlag;

        [TestMethod]
        public void GetActionsTest()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void InvokeActionsTest()
        {
            TestDelegateHelperFlag = false;
            ArgParser dut = new ArgParser();

            dut.AddDelegate(CLIActionType.ArgDebug, TestDelegate);
            CLIAction testAction = new CLIAction(CLIActionType.ArgDebug);

            dut.InvokeActions(new List<CLIAction>() { testAction });
            Assert.IsTrue(TestDelegateHelperFlag);
            // Reset the flag
            TestDelegateHelperFlag = false;
        }

        [TestMethod]
        public void InvokeActions_StringInput()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void AddDelegateTest()
        {
            TestDelegateHelperFlag = false;
            ArgParser dut = new ArgParser();

            dut.AddDelegate(CLIActionType.CSVSetup, EmptyTestDelegate);
            dut.AddDelegate(CLIActionType.ArgDebug, TestDelegate);

            CLIAction testArgDebugAct = new CLIAction(CLIActionType.ArgDebug);
            CLIAction testCSVSetupAct = new CLIAction(CLIActionType.CSVSetup);

            dut.InvokeActions(new List<CLIAction>() { testArgDebugAct });
            Assert.IsTrue(TestDelegateHelperFlag);
            // Reset the flag
            TestDelegateHelperFlag = false;

            dut.InvokeActions(new List<CLIAction>() { testCSVSetupAct });
            // Assert that the invkoing of the other action does not conflict with the flag
            Assert.IsFalse(TestDelegateHelperFlag);

            dut.InvokeActions(new List<CLIAction>() { testArgDebugAct, testCSVSetupAct });
            Assert.IsTrue(TestDelegateHelperFlag);
            TestDelegateHelperFlag = false;

            dut.InvokeActions(new List<CLIAction>() { testCSVSetupAct, testArgDebugAct });
            Assert.IsTrue(TestDelegateHelperFlag);
            TestDelegateHelperFlag = false;
        }

        /// <summary>
        /// When called, sets a flag to true.
        /// </summary>
        /// <param name="delegateAction"> Any action. Ignored. </param>
        private void TestDelegate(CLIAction delegateAction)
        {
            TestDelegateHelperFlag = true;
        }

        /// <summary> Does-nothing delegate. </summary>
        /// <param name="delegateAction"> Ignored. </param>
        private void EmptyTestDelegate(CLIAction delegateAction) { /* NOP */ }
    }
    #pragma warning restore CS1591
}