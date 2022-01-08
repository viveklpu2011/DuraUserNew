
using System.Collections.Generic;
using DuraApp.Core.Helpers.Enums;

namespace DuraApp.Core.Models.Result
{
    /// <summary>
    /// Unexpected result.
    /// </summary>
    public class UnexpectedResult<T> : Result<T>
    {
        public override ResultType ResultType => ResultType.Unexpected;

        public override List<string> Errors => new List<string> { "There was an unexpected problem" };

        public override T Data => default(T);
    }
}
