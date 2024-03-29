namespace N2.Core;

public interface IStdOut
{
    bool Silent { get; set; }
    void Write(string message);
    void WriteLine(string message);
}