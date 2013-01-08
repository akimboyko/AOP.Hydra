using System;
using System.Reflection;
using PostSharp.Aspects;
using PostSharp.Extensibility;

namespace AOP.Hydra.PostSharp
{
    public interface ITransaction
    {
        void Begin();
        void Commit();
        void Rollback();
    }

    public interface IService
    {
        ITransaction Transaction { get; set; }
    }

    [Serializable]
    public class InjectedAspectAttribute : MethodInterceptionAspect
    {
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