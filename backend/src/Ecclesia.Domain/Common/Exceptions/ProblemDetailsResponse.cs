namespace Ecclesia.Domain.Common;

public class ProblemDetailsResponse
{
    public int Status { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Instance { get; set; } = string.Empty;
    public bool HasError { get; set; }
    public string MsgError { get; set; } = string.Empty;
    public string? ErrorCode { get; set; }
    public IDictionary<string, string[]>? Errors { get; set; }
    public IEnumerable<object>? Violations { get; set; }
}