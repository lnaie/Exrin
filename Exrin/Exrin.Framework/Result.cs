namespace Exrin.Framework
{
    using Abstraction;

    public class Result : IResult
    {
        public IResultArgs Arguments { get; set; }

        public ResultType ResultAction { get; set; }
    }
}
