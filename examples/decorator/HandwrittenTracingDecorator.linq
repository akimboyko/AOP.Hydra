<Query Kind="Program">
  <IncludePredicateBuilder>true</IncludePredicateBuilder>
</Query>

void Main()
{
    TraceDecorator.Aspect(() => StaticLogic.SuccessfulCall());
    TraceDecorator.Aspect(() => StaticLogic.ExceptionCall());
    TraceDecorator.Aspect(() => StaticLogic.SuccessfulCallWithReturn(42)).Dump();
    TraceDecorator.Aspect(() => StaticLogic.ExceptionCallWithReturn(42)).Dump();
}

public static class TraceDecorator
{
    public static T Aspect<T>(Func<T> func)
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
    
    public static void Aspect(Action func)
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
        Console.WriteLine("Traced by TraceDecorator: {0}", ex);
    }
}

public static class StaticLogic
{
    public static void SuccessfulCall()
    {
    }
    
    public static void ExceptionCall()
    {   
        throw new Exception();
    }
    
    public static string SuccessfulCallWithReturn(int input)
    {
        return string.Format("ok: input {0}", input);
    }
    
    public static string ExceptionCallWithReturn(int input)
    {   
        throw new Exception();
    }
}