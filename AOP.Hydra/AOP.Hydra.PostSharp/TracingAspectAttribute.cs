using System;
using System.Reflection;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using PostSharp.Extensibility;

namespace AOP.Hydra.PostSharp
{
    [Serializable]
    [ProvideAspectRole(StandardRoles.Tracing)]
    [AspectRoleDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, StandardRoles.Validation)]
    public class TracingAspectAttribute : OnMethodBoundaryAspect
    {
        public Type AbstractFactoryType { get; set; }
        private ILogger Logger { get; set; }

        /// <summary>
        /// Compiles the time validate.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns>Is validation passed</returns>
        public override bool CompileTimeValidate(MethodBase method)
        {
            var result = true;

            var methodInfo = method as MethodInfo;

            if (AbstractFactoryType == null) // check that AbstractFactoryType is provided 
            {
                var message = string.Format("AbstractFactoryType is null. For aspect {0} applied to {1}", GetType().Name, method.DeclaringType);
                Message.Write(methodInfo, SeverityType.Error, "999", message);

                result = false;
            }
            else if (!(typeof(IAbstractFactory<ILogger>).IsAssignableFrom(AbstractFactoryType))) // check that AbstractFactoryType is proper type
            {
                var message = string.Format("Only abstract facory derived from IAbstractFactory<ILogger> allowed, {0} not implements IAbstractFactory<ILogger>. For aspect {1} applied to {2}", 
                                                AbstractFactoryType, GetType().Name, method.DeclaringType);

                Message.Write(methodInfo, SeverityType.Error, "999", message);

                result = false;
            }
            else if (AbstractFactoryType.IsAbstract || AbstractFactoryType.IsInterface) // check that instance of AbstractFactoryType could be created
            {
                var message = string.Format("Only concrete facory derived from IAbstractFactory<ILogger> allowed, {0} not implements IAbstractFactory<ILogger>", AbstractFactoryType);
                Message.Write(methodInfo, SeverityType.Error, "999", message);

                result = false;
            }

            return result;
        }

        /// <summary>
        /// Initializes the current aspect.
        /// </summary>
        /// <param name="method">Method to which the current aspect is applied.</param>
        public override void RuntimeInitialize(MethodBase method)
        {
            // create instance of Abstract Factory
            var abstractFactory = Activator.CreateInstance(AbstractFactoryType);

            // create an instance of dependency, only instances of IAbstractFactory<ILogger> could be here
            // proven by CompileTimeValidate method
            Logger = ((IAbstractFactory<ILogger>)abstractFactory).CreateInstance();
        }

        /// <summary>
        /// Method executed <b>before</b> the body of methods to which this aspect is applied.
        /// </summary>
        /// <param name="args">Event arguments specifying which method
        /// is being executed, which are its arguments, and how should the execution continue
        /// after the execution of <see cref="M:PostSharp.Aspects.IOnMethodBoundaryAspect.OnEntry(PostSharp.Aspects.MethodExecutionArgs)" />.</param>
        public override void OnEntry(MethodExecutionArgs args)
        {
            Logger.Log(string.Format("OnEntry {0}.{1}(...)", args.Method.DeclaringType.FullName, args.Method.Name));
        }

        /// <summary>
        /// Method executed <b>after</b> the body of methods to which this aspect is applied,
        /// even when the method exists with an exception (this method is invoked from
        /// the <c>finally</c> block).
        /// </summary>
        /// <param name="args">Event arguments specifying which method
        /// is being executed and which are its arguments.</param>
        public override void OnExit(MethodExecutionArgs args)
        {
            Logger.Log(string.Format("OnExit {0}.{1}(...)", args.Method.DeclaringType.FullName, args.Method.Name));
        }

        /// <summary>
        /// Method executed <b>after</b> the body of methods to which this aspect is applied,
        /// but only when the method successfully returns (i.e. when no exception flies out
        /// the method.).
        /// </summary>
        /// <param name="args">Event arguments specifying which method
        /// is being executed and which are its arguments.</param>
        public override void OnSuccess(MethodExecutionArgs args)
        {
            Logger.Log(string.Format("OnSuccess {0}.{1}(...)", args.Method.DeclaringType.FullName, args.Method.Name));
        }

        /// <summary>
        /// Method executed <b>after</b> the body of methods to which this aspect is applied,
        /// in case that the method resulted with an exception.
        /// </summary>
        /// <param name="args">Event arguments specifying which method
        /// is being executed and which are its arguments.</param>
        public override void OnException(MethodExecutionArgs args)
        {
            Logger.Log(string.Format("OnException {0}.{1}(...) Exception: {2}", args.Method.DeclaringType.FullName, args.Method.Name, args.Exception));
        }
    }
}