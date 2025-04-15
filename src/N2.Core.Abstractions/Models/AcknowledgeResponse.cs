namespace N2.Core.Models;

public class AcknowledgeResponse
{
    public AcknowledgeCodes Code { get; set; }
    public string Message { get; set; } = string.Empty;
    public AcknowledgeResponse()
    {
    }
    public AcknowledgeResponse(AcknowledgeCodes code)
    {
        Code = code;
    }
}