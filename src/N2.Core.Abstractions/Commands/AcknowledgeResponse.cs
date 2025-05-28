namespace N2.Core.Commands;

public class AcknowledgeResponse : CommandResponse
{
    public AcknowledgeCodes Code { get; set; }
    public AcknowledgeResponse()
    {
    }
    public AcknowledgeResponse(AcknowledgeCodes code, string message) :
        base(ResponseStatus.Success, message)
    {
        Code = code;
    }
    public AcknowledgeResponse(AcknowledgeCodes code)
    {
        Code = code;
    }
}