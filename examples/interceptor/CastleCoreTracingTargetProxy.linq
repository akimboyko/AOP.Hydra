<Query Kind="Program">
  <NuGetReference>Castle.Core</NuGetReference>
  <Namespace>Castle.DynamicProxy</Namespace>
  <Namespace>System.Reflection.Emit</Namespace>
  <IncludePredicateBuilder>true</IncludePredicateBuilder>
</Query>

void Main()
{
    ProxyGenerator proxyGenerator = CreateProxyGenerator();
    
    IService proxy = 
        proxyGenerator
            // http://kozmic.pl/2009/04/01/castle-dynamic-proxy-tutorial-part-ix-interface-proxy-with-target/
            .CreateInterfaceProxyWithTargetInterface(new Service() as IService, new TracingInterceptorAspect());
            
    proxy.ProcessRequest();
   
    proxyGenerator.ProxyBuilder.ModuleScope.SaveAssembly(false);
}

public interface IService   
{
    void ProcessRequest();
}

public sealed class Service : IService
{
    public void ProcessRequest()
    {
        string.Format("{0} call ProcessRequest", GetType().FullName).Dump();
    }
}

[Serializable]
public class TracingInterceptorAspect : IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        invocation.Dump();
    
        string.Format("Before {0} call ProcessRequest", invocation.TargetType).Dump();
        
        invocation.Proceed();
        
        string.Format("After {0} call ProcessRequest", invocation.TargetType).Dump();
        
    }
}

// How to persis proxy: http://kozmic.pl/2009/08/25/castle-dynamic-proxy-tutorial-part-xiv-persisting-proxies/
public static ProxyGenerator CreateProxyGenerator()
{
    var savePhysicalAssembly = true;
    var strongAssemblyName = ModuleScope.DEFAULT_ASSEMBLY_NAME;
    var strongModulePath = ModuleScope.DEFAULT_FILE_NAME;
    var weakAssemblyName = "Castle.Core.Tracing.Interceptor";
    var weakModulePath = "Castle.Core.Tracing.TargetProxy.dll";
    
    var scope = new ModuleScope(savePhysicalAssembly, true, strongAssemblyName, strongModulePath, weakAssemblyName, weakModulePath);
    
    return new ProxyGenerator(new DefaultProxyBuilder(scope));
}