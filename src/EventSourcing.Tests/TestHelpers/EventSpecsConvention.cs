using System;
using System.Linq;
using System.Reflection;
using EventSourcing.Tests.Utils;
using Fixie;

namespace EventSourcing.Tests.TestHelpers
{
    public class EventSpecsConvention:Convention
    {
        public EventSpecsConvention()
        {
            Classes.NameEndsWith("Tests");
            Methods.Where(m => m.IsPublic && m.IsVoid());
            ClassExecution.CreateInstancePerCase();
            CaseExecution.Wrap<RunAfterEachCase>();
        }
    }

    public class RunAfterEachCase: CaseBehavior
    {
        public void Execute(Case context, Action next)
        {
            next();
            var typeToCheck = context.Class;
            MethodInfo runMethod =
                typeToCheck.GetMethodsWithInheritance("Run",
                    BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault(m => !m.GetParameters().Any() && m.ReturnType == typeof (void));
            if(runMethod == null) throw new Exception("Couldn't find 'Run' method on this fixture.");

            runMethod.Invoke(context.Fixture.Instance, null);

        }
    }
}