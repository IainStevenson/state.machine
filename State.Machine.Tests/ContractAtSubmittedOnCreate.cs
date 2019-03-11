using System;
using NUnit.Framework;
using State.Machine.Machines;
using Stateless;

namespace State.Machine.Tests
{
    [TestFixture]
    public class ContractAtSubmittedOnCreate : WhenTesting<Contract>
    {
        protected override void GivenThat()
        {
            var stateMachine = new StateMachine<ContractStates, ContractTriggers>(ContractStates.Submitted);
            var commandBus = new Messaging.CommandBus();
            UnitUnderTest = (Contract)Activator.CreateInstance(typeof(Contract), stateMachine, commandBus);
        }

        protected override void When()
        {
            UnitUnderTest.HandleCreateEvent();
        }

        [Test]
        public void ItShouldNotTransitionToCreated()
        {
            Assert.AreNotEqual(ContractStates.Created, UnitUnderTest.CurrentState);
        }
        
        [Test]
        public void ItShouldRemainAsSubmitted()
        {
            Assert.AreEqual(ContractStates.Submitted, UnitUnderTest.CurrentState);
        }
    }
}