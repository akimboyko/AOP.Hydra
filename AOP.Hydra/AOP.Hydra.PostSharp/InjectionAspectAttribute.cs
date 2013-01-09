using System;
using System.Reflection;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using PostSharp.Extensibility;

namespace AOP.Hydra.PostSharp
{
    [Serializable]
    [ProvideAspectRole(StandardRoles.TransactionHandling)]
    public class InjectedAspectAttribute : MethodInterceptionAspect
    {
        /// <summary>
        /// Compiles the time validate.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns></returns>
        public override bool CompileTimeValidate(MethodBase method)
        {
            var result = true;
            var methodInfo = method as MethodInfo;

            if (!typeof(IService).IsAssignableFrom(method.DeclaringType))
            {
                var message = string.Format("Only class derived from IService allowed, {0} not implements IService", method.DeclaringType);
                Message.Write(methodInfo, SeverityType.Error, "999", message);

                result = false;
            }

            return result;
        }

        /// <summary>
        /// Method invoked <i>instead</i> of the method to which the aspect has been applied.
        /// </summary>
        /// <param name="args">Advice arguments.</param>
        public override void OnInvoke(MethodInterceptionArgs args)
        {
            args.Proceed();

            if (args.Instance is IService)
            {
                (args.Instance as IService).Transaction.Commit();
            }
        }
    }
}