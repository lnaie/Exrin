namespace Exrin.Framework
{
    using Abstraction;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;
    
    public static partial class Process
    {
		public static IRelayCommand ViewModelExecute(this IExecution sender, Func<object, CancellationToken, Task<IList<IResult>>> operation, int timeout = 10000, [CallerMemberName] string name = "")
		{
			var operationList = new List<IBaseOperation>()
			{
				new SingleOperation() { Function = operation }
			};

			var execute = new BaseViewModelExecute(operationList);

			return new RelayCommand(async (parameter) =>
			{
				await ViewModelExecute(sender,
										operations: execute.Operations,
										handleTimeout: sender.HandleTimeout,
										handleUnhandledException: sender.HandleUnhandledException,
										insights: sender.Insights,
										notifyActivityFinished: sender.NotifyActivityFinished,
										notifyOfActivity: sender.NotifyOfActivity,
										timeoutMilliseconds: timeout,
										name: name,
										parameter: parameter);
			})
			{ Timeout = timeout, Function= operation };
		}
		
		public static IRelayCommand ViewModelExecute(this IExecution sender, List<IResult> result, [CallerMemberName] string name = "")
        {
            return ViewModelExecute(sender, new BaseViewModelExecute(new List<IBaseOperation>() { new SingleOperation() { Function = (p, t) => {
                IList<IResult> list = result;
                return Task.FromResult(list);
                } } }), null, name);
        }

        public static IRelayCommand ViewModelExecute(this IExecution sender, List<IResult> result, int timeout, [CallerMemberName] string name = "")
        {
            return ViewModelExecute(sender, new BaseViewModelExecute(new List<IBaseOperation>() { new SingleOperation() { Function = (p, t) => {
                IList<IResult> list = result;         
                return Task.FromResult(list);
                } } }), timeout, null, name);
        }



        public static IRelayCommand ViewModelExecute(this IExecution sender, IBaseOperation execute, [CallerMemberName] string name = "")
        {
            return ViewModelExecute(sender, new BaseViewModelExecute(new List<IBaseOperation>() { execute }), null, name);
        }

        public static IRelayCommand ViewModelExecute(this IExecution sender, IBaseOperation execute, int timeout, [CallerMemberName] string name = "")
        {
            return ViewModelExecute(sender, new BaseViewModelExecute(new List<IBaseOperation>() { execute }), timeout, null, name);
        }

        public static IRelayCommand ViewModelExecute(this IExecution sender, IViewModelExecute execute, [CallerMemberName] string name = "")
        {
            return ViewModelExecute(sender, execute, -1, null, name);
        }



        public static IRelayCommand ViewModelExecute(this IExecution sender, List<IResult> result, Func<object, bool> canExecute, [CallerMemberName] string name = "")
        {
            return ViewModelExecute(sender, new BaseViewModelExecute(new List<IBaseOperation>() { new SingleOperation() { Function = (p, t) => {
                IList<IResult> list = result;
                return Task.FromResult(list);
                } } }), canExecute, name);
        }

        public static IRelayCommand ViewModelExecute(this IExecution sender, List<IResult> result, int timeout, Func<object, bool> canExecute, [CallerMemberName] string name = "")
        {
            return ViewModelExecute(sender, new BaseViewModelExecute(new List<IBaseOperation>() { new SingleOperation() { Function = (p, t) => {
                IList<IResult> list = result;
                return Task.FromResult(list);
                } } }), timeout, canExecute, name);
        }

        public static IRelayCommand ViewModelExecute(this IExecution sender, IBaseOperation execute, Func<object, bool> canExecute, [CallerMemberName] string name = "")
        {
            return ViewModelExecute(sender, new BaseViewModelExecute(new List<IBaseOperation>() { execute }), canExecute, name);
        }

        public static IRelayCommand ViewModelExecute(this IExecution sender, IBaseOperation execute, int timeout, Func<object, bool> canExecute, [CallerMemberName] string name = "")
        {
            return ViewModelExecute(sender, new BaseViewModelExecute(new List<IBaseOperation>() { execute }), timeout, canExecute, name);
        }

        public static IRelayCommand ViewModelExecute(this IExecution sender, IViewModelExecute execute, Func<object, bool> canExecute, [CallerMemberName] string name = "")
        {
            return ViewModelExecute(sender, execute, -1, canExecute, name);
        }
                      
        public static IRelayCommand ViewModelExecute(this IExecution sender, IViewModelExecute execute, int timeout, Func<object, bool> canExecute, [CallerMemberName] string name = "")
        {
            return new RelayCommand(async (parameter) =>
            {
                await ViewModelExecute(sender,
                                        operations: execute.Operations,
                                        handleTimeout: sender.HandleTimeout,
                                        handleUnhandledException: sender.HandleUnhandledException,
                                        insights: sender.Insights,
                                        notifyActivityFinished: sender.NotifyActivityFinished,
                                        notifyOfActivity: sender.NotifyOfActivity,
                                        timeoutMilliseconds: timeout == -1 ? execute.TimeoutMilliseconds : timeout,
                                        name: name,
                                        parameter: parameter);
            }, canExecute)
            { Timeout = execute.TimeoutMilliseconds };

        }

        private readonly static IDictionary<object, bool> _status = new Dictionary<object, bool>();

        private static async Task ViewModelExecute(IExecution sender,
                 List<IBaseOperation> operations,
                 Func<Task> notifyOfActivity = null,
                 Func<Task> notifyActivityFinished = null,
                 Func<Exception, Task<bool>> handleUnhandledException = null,
                 Func<Task> handleTimeout = null,
                 int timeoutMilliseconds = 0,
                 IApplicationInsights insights = null,
                 string name = "",
                 object parameter = null)
        {
            // If currently executing, ignore the latest request
            lock (sender)
            {
                if (!_status.ContainsKey(sender))
                    _status.Add(sender, true);
                else if (_status[sender])
                    return;
                else
                    _status[sender] = true;
            }

            if (sender == null)
                throw new Exception($"The {nameof(IExecution)} sender can not be null");

            if (notifyOfActivity == null)
                throw new Exception($"{nameof(notifyOfActivity)} is null: You must notify the user that something is happening");

            await notifyOfActivity();
			
            // Background thread
            var insightTask = Task.Run(() =>
            {
                try
                {
                    if (insights != null)
                        insights.TrackEvent(name, $"User activated {name}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"insights.TrackEvent({name}) {ex.Message}");
                }
            }).ConfigureAwait(false);

			if (sender.PreCheck != null)
			{
				var result = await Task.Run(async () =>
				{
					return await sender.PreCheck();
				});

				if (result == false)
					return; // Exit execution
			}

            await Task.Run(async () =>
            {
                sender.Result = null;

                List<Func<Task>> rollbacks = new List<Func<Task>>();
                bool transactionRunning = false;

                // Setup Cancellation of Tasks if long running
                var task = new CancellationTokenSource();

                var exceptionState = new PropertyArgs() { Value = false };

                if (timeoutMilliseconds > 0)
                {
                    if (handleTimeout != null)
                        task.Token.Register(async (state) => { if (!(bool)((PropertyArgs)state).Value) await handleTimeout(); }, exceptionState);
                    else if (handleUnhandledException != null)
                        task.Token.Register(async (state) => { if (!(bool)((PropertyArgs)state).Value) await handleUnhandledException(new TimeoutException()); }, exceptionState);
                    else
                        throw new Exception($"You must specify either {nameof(handleTimeout)} or {nameof(handleUnhandledException)} to handle a timeout.");

                    task.CancelAfter(timeoutMilliseconds);
                }

                IList<IResult> results = new List<IResult>();

                try
                {
                    // Transaction Block
                    transactionRunning = true;

                    foreach (var operation in operations)
                    {
                        if (operation.Rollback != null)
                            rollbacks.Add(operation.Rollback);

                        if (operation is IOperation || operation is IChainedOperation)
                        {
                            var op = operation as IOperation;
                            if (op.Function != null)
                            {
                                try
                                {
									await op.Function(results, parameter, task.Token);
                                }
                                catch
                                {
                                    exceptionState.Value = true; // Stops registered cancel function from running, since this is exception not timeout              
                                    task?.Cancel(); // Cancel all tasks
                                    throw; // Go to unhandled exception
                                }
                            }

                        }
                        else
                        {
                            var op = operation as ISingleOperation;
                            try
                            {
                                if (op.Function != null)
                                {
									var resultList = await op.Function(parameter, task.Token);
									if (resultList != null)
                                        foreach (var result in resultList)
                                            results.Add(result);
                                }
                            }
                            catch
                            {
                                exceptionState.Value = true; // Stops registered cancel function from running, since this is exception not timeout              
                                task?.Cancel(); // Cancel all tasks
                                throw; // Go to unhandled exception
                            }

                        }


                        if (!operation.ChainedRollback)
                            rollbacks.Remove(operation.Rollback);
                    }

                    rollbacks.Clear();
                    transactionRunning = false;
                    // End of Transaction Block

                }
                catch (Exception e)
                {
                    if (handleUnhandledException == null)
                        throw;

                    var handled = await handleUnhandledException(e);
                    if (!handled)
                        throw;
                }
                finally
                {
                    try
                    {
                        task?.Dispose();

                        if (transactionRunning)
                        {
                            rollbacks.Reverse(); // Do rollbacks in reverse order
                            foreach (var rollback in rollbacks)
                                await rollback();
                        }

                        // Set final result
                        sender.Result = results;

                        // Handle the result
                        await sender.HandleResult(sender.Result);
                    }
                    finally
                    {
                        if (notifyActivityFinished == null)
                            throw new Exception($"{nameof(notifyActivityFinished)} is null: You need to specify what happens when the operations finish");

                        try
                        {
                            await notifyActivityFinished();
                        }
                        catch (Exception e)
                        {
							if (handleUnhandledException == null)
								throw;

							var handled = await handleUnhandledException(e);
                            if (!handled)
                                throw;
                        }
                        finally
                        {
                            _status.Remove(sender);
                        }
                    }
                }
            });
        }

    }
}
