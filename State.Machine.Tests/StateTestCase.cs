using System;
using Messaging;
using State.Machine.Machines;

namespace State.Machine.Tests
{
    public class StateTestCase
    {
        public ContractStates Inital { get; set; }
        public ICommand Command { get; set; }
        public ContractStates Final { get; set; }
        public Type Event { get; set; }
    }
}