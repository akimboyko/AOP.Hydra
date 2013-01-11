<Query Kind="Program">
  <NuGetReference>Ninject</NuGetReference>
  <NuGetReference>Ninject.Extensions.Interception</NuGetReference>
  <NuGetReference>Ninject.Extensions.Interception.DynamicProxy</NuGetReference>
  <NuGetReference>NUnit</NuGetReference>
  <Namespace>Ninject.Extensions.Interception</Namespace>
  <Namespace>Ninject.Extensions.Interception.Infrastructure.Language</Namespace>
  <Namespace>Ninject</Namespace>
  <IncludePredicateBuilder>true</IncludePredicateBuilder>
</Query>

void Main()
{
    // create kernel
    using(var kernel = new StandardKernel(new InterceptAllModule()))
    {
        // resolve dependency
        var interceptedServiceInstance = kernel.Get<IService>();
        
        // processing...
        interceptedServiceInstance.Process(Guid.NewGuid().ToString());
    }
}

#region Service interface and implementation
public interface IService
{
    void Process(string orderId);
}

public class SomeService : IService
{
    public void Process(string orderId)
    {
        string.Format("Processing order with Id {0}...", orderId).Dump();
    }
}
#endregion

#region Ninject kerbel configuration and interception
public class InterceptAllModule : InterceptionModule
{
    public override void Load()
    {
        // LinqPad fix
        Kernel.Load<Ninject.Extensions.Interception.DynamicProxyModule>();
        
        // interception
        Kernel.Intercept(p => (true)).With(new TracingInterceptor());
        
        // binnding
        Kernel.Bind<IService>().To<SomeService>();
    }
}
#endregion

#region Interception
public class TracingInterceptor : SimpleInterceptor
{
    private readonly Stopwatch _stopwatch = new Stopwatch();
    
    protected override void BeforeInvoke(IInvocation invocation)
    {
        string.Format("Before Invoke method: {0}.{1}", 
            invocation.Request.Method.DeclaringType, invocation.Request.Method).Dump();
            
        _stopwatch.Start();
    }
 
    protected override void AfterInvoke(IInvocation invocation)
    {
        string.Format("After Invoke method: {0}.{1}",
            invocation.Request.Method.DeclaringType, invocation.Request.Method).Dump();
    
        _stopwatch.Stop();
        string.Format("Execution of {0}.{1} took {2}",
                        invocation.Request.Method.DeclaringType, invocation.Request.Method,
                        _stopwatch.Elapsed).Dump();
        _stopwatch.Reset();
    }
}
#endregion