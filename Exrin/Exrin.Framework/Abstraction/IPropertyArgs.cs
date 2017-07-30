namespace Exrin.Abstraction
{
    public interface IPropertyArgs: IResultArgs
    {
        string Name { get; set; }

        object Value { get; set; }
    }
}
