<Query Kind="Program">
  <NuGetReference>NUnit</NuGetReference>
  <Namespace>NUnit.Framework</Namespace>
  <IncludePredicateBuilder>true</IncludePredicateBuilder>
</Query>

#region Step #2: usage examples
void Main()
{
    var toDecorate = false;

    ILogicImplementation example01 = toDecorate
        ?  new TraceDecorator()
        :  new LogicImplementation();

    Assert.That(example01, Is.InstanceOf<ILogicImplementation>());

    example01.SuccessfulCall();
    example01.ExceptionCall();
    example01.SuccessfulCallWithReturn(42).Dump();
    example01.ExceptionCallWithReturn(42).Dump();
}
#endregion

#region Step #1a: target interface
public interface ILogicImplementation
{
    void SuccessfulCall();
    void ExceptionCall();
    string SuccessfulCallWithReturn(int input);
    string ExceptionCallWithReturn(int input);
}
#endregion

#region Step #1b: target implementation
public class LogicImplementation : ILogicImplementation
{
    public virtual void SuccessfulCall()
    {
    }
    
    public virtual void ExceptionCall()
    {   
        throw new Exception();
    }
    
    public virtual string SuccessfulCallWithReturn(int input)
    {
        return string.Format("ok: input {0}", input);
    }
    
    public virtual string ExceptionCallWithReturn(int input)
    {   
        throw new Exception();
    }
}
#endregion

#region Step #3: Handwritten decorator
public class TraceDecorator : LogicImplementation
{
    #region Step #2.1: overrides
    public override void SuccessfulCall()
    {
        LogMethodName();
        LogException(() => base.SuccessfulCall());
    }
    
    public override void ExceptionCall()
    {   
        LogMethodName();
        LogException(() => base.ExceptionCall());
    }
    
    public override string SuccessfulCallWithReturn(int input)
    {
        LogMethodName();
        return LogException(() => base.SuccessfulCallWithReturn(input));
    }
    
    public override string ExceptionCallWithReturn(int input)
    {   
        LogMethodName();
        return LogException(() => base.ExceptionCallWithReturn(input));
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