namespace Exrin.Framework
{
    using Abstraction;
    using System.Collections.Generic;
    using System.Threading.Tasks;

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

        public static implicit operator Task<IList<IResult>>(Result result)
        {
            IList<IResult> list = new List<IResult>() { result };
            return Task.FromResult(list);
        }
    }

}
