using System;
using System.Reflection;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using PostSharp.Extensibility;

namespace MyCompany.Services.Aspect
{
    [Serializable]
    [ProvideAspectRole(StandardRoles.Security)]
    [AspectRoleDependency(AspectDependencyAction.Order, AspectDependencyPosition.After, StandardRoles.Tracing)]
    public abstract class AbstractVerificationAttribute : OnMethodBoundaryAspect
    {
	#region compile-time validation
        public override bool CompileTimeValidate(MethodBase method)
        {
            var result = true;
            var methodInfo = method as MethodInfo;

            if (!typeof(AbstractService).IsAssignableFrom(method.DeclaringType))
            {
                result &= false;

                Message.Write(methodInfo, SeverityType.Error, "998", "Only class derived from Abstract Service allowed");
            }

            return result;
        }
	#endregion
	
	#region verify attributes
        public override void OnEntry(MethodExecutionArgs args)
        {
            var method = args.Method;
            var arguments = args.Arguments.ToArray();

            var accessKeyId = AspectUtility.GetArgument<string>(args, AspectUtility.GetArgumentPosition(args, "accessKeyId"));
            var signatureHash = AspectUtility.GetArgument<string>(args, AspectUtility.GetArgumentPosition(args, "signatureHash"));

            var callingService = args.Instance as AbstractService;

            if (!getVerificationDelegate(callingService)(accessKeyId, AbstractService.CalculateSignature(method, arguments), signatureHash, out callingService.AuthenticatedUserId))
            {
		// Note: stop processing if verification failed
                args.ReturnValue = GetReturnValue();
                args.FlowBehavior = FlowBehavior.Return;
            }
        }
	

        protected abstract object GetReturnValue();
        internal abstract AbstractService.VerificationDelegate getVerificationDelegate(AbstractService callingService);
	#endregion
    }
}
