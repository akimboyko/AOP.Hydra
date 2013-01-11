<Query Kind="Program">
  <NuGetReference>Castle.Core</NuGetReference>
  <Namespace>Castle.DynamicProxy</Namespace>
  <Namespace>System.Reflection.Emit</Namespace>
  <IncludePredicateBuilder>true</IncludePredicateBuilder>
</Query>

void Main()
{
    #region define proxy
    ProxyGenerator proxyGenerator = CreateProxyGenerator();
    
    IService proxy = 
        proxyGenerator
            .CreateClassProxy<Service>(new TracingInterceptorAspect());
    #endregion
            
    #region processing
    proxy.ProcessRequest();
    #endregion
   
    #region save proxy to reuse
    proxyGenerator.ProxyBuilder.ModuleScope.SaveAssembly(false);
    #endregion
}

#region service
public interface IService   
{
    void ProcessRequest();
}

public class Service : IService
{
    public virtual void ProcessRequest()
    {
        string.Format("{0} call ProcessRequest", GetType().FullName).Dump();
    }
}
#endregion

#region interceptor
public class TracingInterceptorAspect : IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        string.Format("Before {0} call ProcessRequest", invocation.TargetType).Dump();
        
        invocation.Proceed();
        
        string.Format("After {0} call ProcessRequest", invocation.TargetType).Dump();
    }
}
#endregion

#region create proxy generator
// How to persis proxy: http://kozmic.pl/2009/08/25/castle-dynamic-proxy-tutorial-part-xiv-persisting-proxies/
public static ProxyGenerator CreateProxyGenerator()
{
    var savePhysicalAssembly = true;
    var strongAssemblyName = ModuleScope.DEFAULT_ASSEMBLY_NAME;
    var strongModulePath = ModuleScope.DEFAULT_FILE_NAME;
    var weakAssemblyName = "Castle.Core.Tracing.Interceptor";
    var weakModulePath = "Castle.Core.Tracing.Interceptor.dll";
    
    var scope = new ModuleScope(savePhysicalAssembly, true, strongAssemblyName, strongModulePath, weakAssemblyName, weakModulePath);
    
    return new ProxyGenerator(new DefaultProxyBuilder(scope));
}
#endregion