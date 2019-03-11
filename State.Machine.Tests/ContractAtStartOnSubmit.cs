using System;
using NUnit.Framework;
using State.Machine.Machines;
using Stateless;

namespace State.Machine.Tests
{
    [TestFixture]
    public class ContractAtStartOnSubmit : WhenTesting<Contract>
    {
        protected override void GivenThat()
        {
            var stateMachine = new StateMachine<ContractStates, ContractTriggers>(ContractStates.Start);
            var commandBus = new Messaging.CommandBus();
            UnitUnderTest = (Contract)Activator.CreateInstance(typeof(Contract), stateMachine, commandBus);
        }

        protected override void When()
        {
            UnitUnderTest.HandleSubmitEvent();
        }

        [Test]
        public void ItShouldNotTransitionToSubmitted()
        {
            Assert.AreNotEqual(ContractStates.Submitted, UnitUnderTest.CurrentState);
        }


        [Test]
        public void ItShouldRemainAsStart()
        {
            Assert.AreEqual(ContractStates.Start, UnitUnderTest.CurrentState);
        }
    }
}