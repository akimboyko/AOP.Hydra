using System;
using System.Reflection;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;

namespace AOP.Hydra.PostSharp
{
    #region Aspect deinition
    [Serializable]
    [ProvideAspectRole(StandardRoles.Tracing)]
    [AspectRoleDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, StandardRoles.Validation)]
    #endregion
    public class ExampleAspectAttribute
        #region Aspect type
        : OnMethodBoundaryAspect
        #endregion
    {
        #region Compiles the time validate.
        public override bool CompileTimeValidate(MethodBase method)
        {
            return true;
        }
        #endregion

        #region Initializes the current aspect.
        public override void RuntimeInitialize(MethodBase method)
        {
        }
        #endregion

        #region Method executed before the body of methods to which this aspect is applied.
        public override void OnEntry(MethodExecutionArgs args)
        {
        }
        #endregion

        #region Method executed after the body of methods to which this aspect is applied,
        public override void OnExit(MethodExecutionArgs args)
        {
        }
        #endregion

        #region Method executed after the body of methods to which this aspect is applied,
        public override void OnSuccess(MethodExecutionArgs args)
        {
        }
        #endregion

        #region Method executed after the body of methods to which this aspect is applied,
        public override void OnException(MethodExecutionArgs args)
        {
        }
        #endregion
    }
}
