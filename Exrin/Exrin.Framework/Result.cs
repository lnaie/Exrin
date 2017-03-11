namespace Exrin.Framework
{
    using Abstraction;
    using System.Collections.Generic;

    public class Result : IResult
    {
        public IResultArgs Arguments { get; set; }

        public ResultType ResultAction { get; set; }

        public Result() { }

        public Result(ResultType resultAction, IResultArgs arguments)
        {
            ResultAction = resultAction;
            Arguments = arguments;
        }

        public static implicit operator List<IResult>(Result result)
        {
            return new List<IResult>() { result };
        }
    }
}
