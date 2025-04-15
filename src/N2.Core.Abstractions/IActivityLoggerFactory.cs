namespace N2.Core;

public interface IActivityLoggerFactory
{
    IActivityLogger StartActivity(string name);
    IActivityLogger StartActivity<T>();
}
