using Exrin.Abstraction;
using Exrin.Framework;
using ExrinSampleMobileApp.Framework.Locator;
using ExrinSampleMobileApp.Framework.Locator.Views;
using ExrinSampleMobileApp.Logic.VisualState;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExrinSampleMobileApp.Logic.ViewModel
{
    public class AboutOperation : ISingleOperation
    {
		private readonly MainVisualState _state;
        public AboutOperation(MainVisualState state) { _state = state; }

        public Func<object, CancellationToken, Task<IList<IResult>>> Function
        {
            get
            {
                return (parameter, token) =>
                {
					_state.IsBusy = true;
                    return new NavigationResult(Stacks.Main, Main.About);
                };
            }
        }

        public bool ChainedRollback { get; private set; } = false;

        public Func<Task> Rollback { get { return null; } }
    }
}
