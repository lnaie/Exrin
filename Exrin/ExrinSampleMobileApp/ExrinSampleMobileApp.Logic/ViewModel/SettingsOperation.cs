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
    public class SettingsOperation : ISingleOperation
    {
		private readonly IVisualState _visualState;
        public SettingsOperation(IVisualState visualState) {
			_visualState = visualState;
		}

        public Func<object, CancellationToken, Task<IList<IResult>>> Function
        {
            get
            {
                return (parameter, token) =>
                {
					((AboutVisualState)_visualState).MyProperty = null;
					return new NavigationResult(Stacks.Main, Main.Settings);
                };
            }
        }

        public bool ChainedRollback { get; private set; } = false;

        public Func<Task> Rollback { get { return null; } }
    }
}
