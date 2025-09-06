namespace Kasp.Test.Interfaces
{
    public interface IContentReplacementService
    {
        string ReplacePasswordPattern(string content);
        string ReplaceLicenseKeyPattern(string content);
        string ReplaceForbiddenWords(string content);
    }
}
