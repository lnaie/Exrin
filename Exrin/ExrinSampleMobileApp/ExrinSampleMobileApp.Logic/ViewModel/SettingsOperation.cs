using Exrin.Abstraction;
using Exrin.Framework;
using ExrinSampleMobileApp.Framework.Locator;
using ExrinSampleMobileApp.Framework.Locator.Views;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExrinSampleMobileApp.Logic.ViewModel
{
    public class SettingsOperation : ISingleOperation
    {
        public SettingsOperation() { }

        public Func<object, CancellationToken, Task<IList<IResult>>> Function
        {
            get
            {
                return (parameter, token) =>
                {
                    return new NavigationResult(Stacks.Main, Main.Settings);
                };
            }
        }

        public bool ChainedRollback { get; private set; } = false;

        public Func<Task> Rollback { get { return null; } }
    }
}
