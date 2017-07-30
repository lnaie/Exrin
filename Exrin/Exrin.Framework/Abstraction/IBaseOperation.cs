namespace Exrin.Abstraction
{
    using System;
    using System.Threading.Tasks;

    public interface IBaseOperation
    {
        /// <summary>
        /// If the Function fails, this function performs a rollback if necessary
        /// </summary>
        Func<Task> Rollback { get; }
        /// <summary>
        /// If a Function fails later on in the list, do you want to perform this rollback?
        /// </summary>
        bool ChainedRollback { get; }

    }
}
