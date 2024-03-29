namespace N2.Core;

public class ResponseData
{
    public IRequestResult? Result { get; set; }

    public static ResponseData<TValue> Create<TValue>(IRequestResult result, TValue? data = default)
    {
        return new ResponseData<TValue>
        {
            Result = result,
            Data = data
        };
    }
}

public class ResponseData<TValue>
{
    public IRequestResult? Result { get; set; }
    public TValue? Data { get; set; }
}