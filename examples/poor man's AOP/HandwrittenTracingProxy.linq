<Query Kind="Program">
  <NuGetReference>NUnit</NuGetReference>
  <Namespace>NUnit.Framework</Namespace>
  <IncludePredicateBuilder>true</IncludePredicateBuilder>
</Query>

#region Step #2: usage examples
void Main()
{
    var toDecorate = false;

    ILogicImplementation example02 = new LogicImplementation();

    if(toDecorate)
    {
        example02 = new TraceProxy(example02);
    }
    
    Assert.That(example02, Is.InstanceOf<ILogicImplementation>());

    example02.SuccessfulCall();
    example02.ExceptionCall();
    example02.SuccessfulCallWithReturn(42).Dump();
    example02.ExceptionCallWithReturn(42).Dump();
}
#endregion

#region Step #1a: target intrface
public interface ILogicImplementation
{
    void SuccessfulCall();
    void ExceptionCall();
    string SuccessfulCallWithReturn(int input);
    string ExceptionCallWithReturn(int input);
}
#endregion

#region Step #1b: target implementation
public sealed class LogicImplementation : ILogicImplementation
{
    public void SuccessfulCall()
    {
    }
    
    public void ExceptionCall()
    {   
        throw new Exception();
    }
    
    public string SuccessfulCallWithReturn(int input)
    {
        return string.Format("ok: input {0}", input);
    }
    
    public  string ExceptionCallWithReturn(int input)
    {   
        throw new Exception();
    }
}
#endregion

#region Step #3: Handwritten proxy
public class TraceProxy : ILogicImplementation
{
    private readonly ILogicImplementation _instance;

    public TraceProxy(ILogicImplementation instance)
    {
        _instance = instance;
    }

    #region Step #2.1: overrides
    public void SuccessfulCall()
    {
        LogMethodName();
        LogException(() => _instance.SuccessfulCall());
    }
    
    public void ExceptionCall()
    {   
        LogMethodName();
        LogException(() => _instance.ExceptionCall());
    }
    
    public string SuccessfulCallWithReturn(int input)
    {
        LogMethodName();
        return LogException(() => _instance.SuccessfulCallWithReturn(input));
    }
    
    public string ExceptionCallWithReturn(int input)
    {   
        LogMethodName();
        return LogException(() => _instance.ExceptionCallWithReturn(input));
    }
    #endregion

    #region Step #2.2: tracing, but mostly without call context
    private static void LogMethodName()
    {
        var stackTrace = new StackTrace();
        var stackFrame = stackTrace.GetFrame(1);

        MethodBase currentMethodName = stackFrame.GetMethod();
        
        currentMethodName.Dump();
    }
    
    private static T LogException<T>(Func<T> func)
    {
        try
        {
            return func();
        }
        catch(Exception ex)
        {
            LogException(ex);
            
            return default(T);
        }    
    }
    
    private static void LogException(Action func)
    {
        try
        {
            func();
        }
        catch(Exception ex)
        {
            LogException(ex);
        }    
    }
    
    private static void LogException(Exception ex)
    {
        string.Format("Traced by TraceDecorator: {0}", ex.Message).Dump();
    }
    #endregion
}
#endregion