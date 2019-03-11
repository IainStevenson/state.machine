using System;
using NUnit.Framework;
using State.Machine.Machines;
using Stateless;

namespace State.Machine.Tests
{
    [TestFixture]
    public class ContractAtStartOnHandleCreateEvent : WhenTesting<Contract>
    {
        protected override void GivenThat()
        {
            var stateMachine = new StateMachine<ContractStates, ContractTriggers>(ContractStates.Start);
            var commandBus = new Messaging.CommandBus();
            UnitUnderTest = (Contract) Activator.CreateInstance(typeof (Contract), stateMachine, commandBus);
        }

        protected override void When()
        {
            UnitUnderTest.HandleCreateEvent();
        }

        [Test]
        public void ItShouldTransitionToCreated()
        {
            Assert.AreEqual(ContractStates.Created, UnitUnderTest.CurrentState);
        }
    }
}