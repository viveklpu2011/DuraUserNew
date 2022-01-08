
using System.Collections.Generic;
using DuraApp.Core.Helpers.Enums;

namespace DuraApp.Core.Models.Result
{
    /// <summary>
    /// Wrapper for response handling. Handles grouping data and errors together to make it easier to process
    /// </summary>
    public abstract class Result<T>
    {
        public abstract T Data { get; }
        public abstract List<string> Errors { get; }
        public abstract ResultType ResultType { get; }
    }
}
