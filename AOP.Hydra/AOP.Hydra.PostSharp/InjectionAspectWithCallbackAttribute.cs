using System;
using PostSharp.Aspects;

namespace AOP.Hydra.PostSharp
{
    // More about aspect life time
    // http://www.sharpcrafters.com/blog/post/Day-10-Aspect-Lifetime-Scope-Part-2.aspx
    [Serializable]
    public class InjectionAspectWithCallbackAttribute : OnMethodBoundaryAspect, IInstanceScopedAspect
    {
        private ILogger Logger { get; set; }
        public static Func<ILogger> InjectLogger { get; set; }

        #region OnMethodBoundaryAspect overrides

        public override void OnEntry(MethodExecutionArgs args)
        {
            Logger.Log("On Entry");
        }

        #endregion

        #region IInstanceScopedAspect Members

        public object CreateInstance(AdviceArgs adviceArgs)
        {
            return MemberwiseClone();
        }

        public void RuntimeInitializeInstance()
        {
            if (InjectLogger == null)
            {
                throw new Exception("CreateSomeDependency is null");
            }

            Logger = InjectLogger();
        }

        #endregion
    }
}
