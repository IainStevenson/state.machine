using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using State.Machine.Messages;
using State.Machine.Messaging;

namespace State.Machine.Tests
{
    [ExcludeFromCodeCoverage]
    public static class TestExtensions
    {
        public static IEnumerable<Type> GetCommandsForTrigger(Assembly assembly, ContractTriggers trigger)
        {
            Type[] types = assembly.GetTypes();
            IEnumerable<Type> iCommandTypes = types.Where(x => typeof (ICommand).IsAssignableFrom(x) && x.IsClass);
            IEnumerable<Type> triggeringTypes =
                iCommandTypes.Where(x => x.GetCustomAttributes<ContractTriggerAttribute>().Any());
            IEnumerable<Type> thisTriggeringTypes =
                triggeringTypes.Where(x => x.GetCustomAttribute<ContractTriggerAttribute>().Trigger == trigger);

            return thisTriggeringTypes;
        }

        public static IEnumerable<Type> GetEventsForTrigger(Assembly assembly, ContractTriggers trigger)
        {
            Type[] types = assembly.GetTypes();
            IEnumerable<Type> iCommandTypes = types.Where(x => typeof(IEvent).IsAssignableFrom(x) && x.IsClass);
            IEnumerable<Type> triggeringTypes =
                iCommandTypes.Where(x => x.GetCustomAttributes<ContractTriggerAttribute>().Any());
            IEnumerable<Type> thisTriggeringTypes =
                triggeringTypes.Where(x => x.GetCustomAttribute<ContractTriggerAttribute>().Trigger == trigger);

            return thisTriggeringTypes;
        }
    }
}