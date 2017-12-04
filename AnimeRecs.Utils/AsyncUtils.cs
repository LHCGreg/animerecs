using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace AnimeRecs.Utils
{
    public class AsyncUtils
    {
        /// <summary>
        /// Calls the passed in asynchronous function.
        /// If the function synchronously throws an exception, returns it wrapped in a task.
        /// </summary>
        /// <param name="asyncFunction"></param>
        /// <returns></returns>
        public static Task EnsureExceptionsWrapped(Func<Task> asyncFunction)
        {
            try
            {
                return asyncFunction();
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
        }

        /// <summary>
        /// Calls the passed in asynchronous function.
        /// If the function synchronously throws an exception, returns it wrapped in a task.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="asyncFunction"></param>
        /// <returns></returns>
        public static Task<T> EnsureExceptionsWrapped<T>(Func<Task<T>> asyncFunction)
        {
            try
            {
                return asyncFunction();
            }
            catch (Exception ex)
            {
                return Task.FromException<T>(ex);
            }
        }

        /// <summary>
        /// Starts tasks by calling each of the passed in functions. If any function synchronously throws,
        /// it is wrapped in a task. Returns CancellableTasks corresponding to each passed in function.
        /// </summary>
        /// <param name="asyncFunctions"></param>
        /// <returns></returns>
        public static CancellableTask[] StartTasksEnsureExceptionsWrapped(CancellableAsyncFunc[] asyncFunctions)
        {
            CancellableTask[] cancellableTasks = new CancellableTask[asyncFunctions.Length];
            for (int i = 0; i < asyncFunctions.Length; i++)
            {
                cancellableTasks[i] = asyncFunctions[i].StartTaskEnsureExceptionsWrapped();
            }
            return cancellableTasks;
        }

        /// <summary>
        /// Starts tasks by calling each of the passed in functions. If any function synchronously throws,
        /// it is wrapped in a task. Returns CancellableTasks corresponding to each passed in function.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="asyncFunctions"></param>
        /// <returns></returns>
        public static CancellableTask<T>[] StartTasksEnsureExceptionsWrapped<T>(CancellableAsyncFunc<T>[] asyncFunctions)
        {
            CancellableTask<T>[] cancellableTasks = new CancellableTask<T>[asyncFunctions.Length];
            for (int i = 0; i < asyncFunctions.Length; i++)
            {
                cancellableTasks[i] = asyncFunctions[i].StartTaskEnsureExceptionsWrapped();
            }
            return cancellableTasks;
        }

        /// <summary>
        /// Waits for all of the passed in tasks to complete successfully or for any to fault or be canceled.
        /// If any fault or are canceled, all the tasks are canceled and the returned task waits for all the tasks
        /// to enter a final state.
        /// If all complete successfully, the returned task is successful.
        /// If any task faults, the returned task's Exception property is an AggregateException that
        /// contains exceptions from all the tasks that faulted.
        /// If no task faulted but at least one was canceled, the returned task
        /// will be in the canceled state with the CancellationToken associated with the task.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static Task WhenAllCancelOnFirstExceptionWaitForCancellations<T>(IEnumerable<CancellableTask<T>> tasks)
        {
            List<ICancellableTask> interfaceTasks = tasks.Cast<ICancellableTask>().ToList();
            return WhenAllCancelOnFirstException(interfaceTasks, waitForCancellations: true);
        }

        /// <summary>
        /// Waits for all of the passed in tasks to complete successfully or for any to fault or be canceled.
        /// If any fault or are canceled, all the tasks are canceled and the returned task waits for all the tasks
        /// to enter a final state.
        /// If all complete successfully, the returned task is successful.
        /// If any task faults, the returned task's Exception property is an AggregateException that
        /// contains exceptions from all the tasks that faulted.
        /// If no task faulted but at least one was canceled, the returned task
        /// will be in the canceled state with the CancellationToken associated with the task.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static Task WhenAllCancelOnFirstExceptionWaitForCancellations<T>(params CancellableTask<T>[] tasks)
        {
            List<ICancellableTask> interfaceTasks = tasks.Cast<ICancellableTask>().ToList();
            return WhenAllCancelOnFirstException(interfaceTasks, waitForCancellations: true);
        }

        /// <summary>
        /// Waits for all of the passed in tasks to complete successfully or for any to fault or be canceled.
        /// If any fault or are canceled, all the tasks are canceled and the returned task waits for all the tasks
        /// to enter a final state.
        /// If all complete successfully, the returned task is successful.
        /// If any task faults, the returned task's Exception property is an AggregateException that
        /// contains exceptions from all the tasks that faulted.
        /// If no task faulted but at least one was canceled, the returned task
        /// will be in the canceled state with the CancellationToken associated with the task.
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static Task WhenAllCancelOnFirstExceptionWaitForCancellations(IEnumerable<CancellableTask> tasks)
        {
            List<ICancellableTask> interfaceTasks = tasks.Cast<ICancellableTask>().ToList();
            return WhenAllCancelOnFirstException(interfaceTasks, waitForCancellations: true);
        }

        /// <summary>
        /// Waits for all of the passed in tasks to complete successfully or for any to fault or be canceled.
        /// If any fault or are canceled, all the tasks are canceled and the returned task waits for all the tasks
        /// to enter a final state.
        /// If all complete successfully, the returned task is successful.
        /// If any task faults, the returned task's Exception property is an AggregateException that
        /// contains exceptions from all the tasks that faulted.
        /// If no task faulted but at least one was canceled, the returned task
        /// will be in the canceled state with the CancellationToken associated with the task.
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static Task WhenAllCancelOnFirstExceptionWaitForCancellations(params CancellableTask[] tasks)
        {
            List<ICancellableTask> interfaceTasks = tasks.Cast<ICancellableTask>().ToList();
            return WhenAllCancelOnFirstException(interfaceTasks, waitForCancellations: true);
        }

        /// <summary>
        /// Waits for all of the passed in tasks to complete successfully or for any to fault or be canceled.
        /// If any fault or are canceled, all the tasks are canceled and the returned task waits for all the tasks
        /// to enter a final state.
        /// If all complete successfully, the returned task is successful.
        /// If any task faults, the returned task's Exception property is an AggregateException that
        /// contains exceptions from all the tasks that faulted.
        /// If no task faulted but at least one was canceled, the returned task
        /// will be in the canceled state with the CancellationToken associated with the task.
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static Task WhenAllCancelOnFirstExceptionWaitForCancellations(IEnumerable<ICancellableTask> tasks)
        {
            return WhenAllCancelOnFirstException(tasks, waitForCancellations: true);
        }

        /// <summary>
        /// Waits for all of the passed in tasks to complete successfully or for any to fault or be canceled.
        /// If any fault or are canceled, all the tasks are canceled and the returned task waits for all the tasks
        /// to enter a final state.
        /// If all complete successfully, the returned task is successful.
        /// If any task faults, the returned task's Exception property is an AggregateException that
        /// contains exceptions from all the tasks that faulted.
        /// If no task faulted but at least one was canceled, the returned task
        /// will be in the canceled state with the CancellationToken associated with the task.
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static Task WhenAllCancelOnFirstExceptionWaitForCancellations(params ICancellableTask[] tasks)
        {
            return WhenAllCancelOnFirstException(tasks, waitForCancellations: true);
        }

        /// <summary>
        /// Waits for all of the passed in tasks to complete successfully or for any to fault or be canceled.
        /// If any fault or are canceled, all the tasks are canceled.
        /// If all complete successfully, the returned task is successful.
        /// If any task faults, the returned task's Exception property is an AggregateException that
        /// contains exceptions from the first task that faulted.
        /// After any fault or cancellation, all tasks are cancelled and the returned task immediately
        /// becomes faulted or canceled without waiting for the other tasks to go into their cancelled state or fault.
        /// With the first task canceled the returned task will be in the canceled
        /// state with the CancellationToken associated with the task.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static Task WhenAllCancelOnFirstExceptionDontWaitForCancellations<T>(IEnumerable<CancellableTask<T>> tasks)
        {
            List<ICancellableTask> interfaceTasks = tasks.Cast<ICancellableTask>().ToList();
            return WhenAllCancelOnFirstException(interfaceTasks, waitForCancellations: false);
        }

        /// <summary>
        /// Waits for all of the passed in tasks to complete successfully or for any to fault or be canceled.
        /// If any fault or are canceled, all the tasks are canceled.
        /// If all complete successfully, the returned task is successful.
        /// If any task faults, the returned task's Exception property is an AggregateException that
        /// contains exceptions from the first task that faulted.
        /// After any fault or cancellation, all tasks are cancelled and the returned task immediately
        /// becomes faulted or canceled without waiting for the other tasks to go into their cancelled state or fault.
        /// With the first task canceled the returned task will be in the canceled
        /// state with the CancellationToken associated with the task.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static Task WhenAllCancelOnFirstExceptionDontWaitForCancellations<T>(params CancellableTask<T>[] tasks)
        {
            List<ICancellableTask> interfaceTasks = tasks.Cast<ICancellableTask>().ToList();
            return WhenAllCancelOnFirstException(interfaceTasks, waitForCancellations: false);
        }

        /// <summary>
        /// Waits for all of the passed in tasks to complete successfully or for any to fault or be canceled.
        /// If any fault or are canceled, all the tasks are canceled.
        /// If all complete successfully, the returned task is successful.
        /// If any task faults, the returned task's Exception property is an AggregateException that
        /// contains exceptions from the first task that faulted.
        /// After any fault or cancellation, all tasks are cancelled and the returned task immediately
        /// becomes faulted or canceled without waiting for the other tasks to go into their cancelled state or fault.
        /// With the first task canceled the returned task will be in the canceled
        /// state with the CancellationToken associated with the task.
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static Task WhenAllCancelOnFirstExceptionDontWaitForCancellations(IEnumerable<CancellableTask> tasks)
        {
            List<ICancellableTask> interfaceTasks = tasks.Cast<ICancellableTask>().ToList();
            return WhenAllCancelOnFirstException(interfaceTasks, waitForCancellations: false);
        }

        /// <summary>
        /// Waits for all of the passed in tasks to complete successfully or for any to fault or be canceled.
        /// If any fault or are canceled, all the tasks are canceled.
        /// If all complete successfully, the returned task is successful.
        /// If any task faults, the returned task's Exception property is an AggregateException that
        /// contains exceptions from the first task that faulted.
        /// After any fault or cancellation, all tasks are cancelled and the returned task immediately
        /// becomes faulted or canceled without waiting for the other tasks to go into their cancelled state or fault.
        /// With the first task canceled the returned task will be in the canceled
        /// state with the CancellationToken associated with the task.
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static Task WhenAllCancelOnFirstExceptionDontWaitForCancellations(params CancellableTask[] tasks)
        {
            List<ICancellableTask> interfaceTasks = tasks.Cast<ICancellableTask>().ToList();
            return WhenAllCancelOnFirstException(interfaceTasks, waitForCancellations: false);
        }

        /// <summary>
        /// Waits for all of the passed in tasks to complete successfully or for any to fault or be canceled.
        /// If any fault or are canceled, all the tasks are canceled.
        /// If all complete successfully, the returned task is successful.
        /// If any task faults, the returned task's Exception property is an AggregateException that
        /// contains exceptions from the first task that faulted.
        /// After any fault or cancellation, all tasks are cancelled and the returned task immediately
        /// becomes faulted or canceled without waiting for the other tasks to go into their cancelled state or fault.
        /// With the first task canceled the returned task will be in the canceled
        /// state with the CancellationToken associated with the task.
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static Task WhenAllCancelOnFirstExceptionDontWaitForCancellations(IEnumerable<ICancellableTask> tasks)
        {
            return WhenAllCancelOnFirstException(tasks, waitForCancellations: false);
        }

        /// <summary>
        /// Waits for all of the passed in tasks to complete successfully or for any to fault or be canceled.
        /// If any fault or are canceled, all the tasks are canceled.
        /// If all complete successfully, the returned task is successful.
        /// If any task faults, the returned task's Exception property is an AggregateException that
        /// contains exceptions from the first task that faulted.
        /// After any fault or cancellation, all tasks are cancelled and the returned task immediately
        /// becomes faulted or canceled without waiting for the other tasks to go into their cancelled state or fault.
        /// With the first task canceled the returned task will be in the canceled
        /// state with the CancellationToken associated with the task.
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static Task WhenAllCancelOnFirstExceptionDontWaitForCancellations(params ICancellableTask[] tasks)
        {
            return WhenAllCancelOnFirstException(tasks, waitForCancellations: false);
        }

        /// <summary>
        /// Waits for all of the passed in tasks to complete successfully or for any to fault or be canceled.
        /// If any fault or are canceled, all the tasks are canceled.
        /// If all complete successfully, the returned task is successful.
        /// If any task faults, the returned task's Exception property is an AggregateException that
        /// contains exceptions from all the tasks that faulted. If waitForCancellations is false, it will only contain exceptions
        /// from the first faulting task.
        /// If waitForCancellations is true and no task faulted but at least one was canceled, the returned task
        /// will be in the canceled state with the CancellationToken associated with the task.
        /// If waitForCancellations is false, with the first task canceled the returned task will be in the canceled
        /// state with the CancellationToken associated with the task.
        /// </summary>
        /// <param name="tasks"></param>
        /// <param name="waitForCancellations">If true, wait for all tasks to complete one way or another,
        /// whether successfully, faulted, or canceled, before the returned task becomes successful, faulted, or canceled.
        /// If false, then after any fault or cancellation, all tasks are cancelled and the returned task immediately
        /// becomes faulted or canceled without waiting for the other tasks to go into their cancelled state or fault.</param>
        public static Task WhenAllCancelOnFirstException(IEnumerable<ICancellableTask> tasks, bool waitForCancellations = true)
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();

            List<ICancellableTask> tasksList = tasks.ToList();

            if (tasksList.Count == 0)
            {
                tcs.SetResult(null);
                return tcs.Task;
            }

            object continuationLock = new object();
            int tasksCompleteInAnyManner = 0;
            ICancellableTask firstTaskCanceled = null;
            List<Exception> exceptions = new List<Exception>();
            bool cancellationsIssued = false;
            bool waitTaskCompleted = false;

            foreach (ICancellableTask cancellableTask in tasks)
            {
                cancellableTask.Task.ContinueWith(task =>
                {
                    lock (continuationLock)
                    {
                        // Don't bother doing the bookkeeping if we've already signaled the task.
                        if (waitTaskCompleted)
                        {
                            return;
                        }

                        tasksCompleteInAnyManner++;

                        if (task.IsFaulted)
                        {
                            exceptions.AddRange(task.Exception.InnerExceptions);
                        }

                        if (task.IsCanceled && firstTaskCanceled == null)
                        {
                            for (int i = 0; i < tasksList.Count; i++)
                            {
                                if (tasksList[i].Task == task)
                                {
                                    firstTaskCanceled = tasksList[i];
                                }
                            }
                        }

                        if (tasksCompleteInAnyManner == tasksList.Count)
                        {
                            // If all tasks finished but some faulted, set the exceptions for the wait task
                            if (exceptions.Count > 0)
                            {
                                tcs.SetException(exceptions);
                                waitTaskCompleted = true;
                            }
                            // If all tasks finished, none faulted, but at least one was canceled, set wait task as canceled
                            else if (firstTaskCanceled != null)
                            {
                                tcs.TrySetCanceled(firstTaskCanceled.CancellationTokenSource.Token);
                                waitTaskCompleted = true;
                            }
                            // If everything succeeded, signal the wait task as successful
                            else
                            {
                                tcs.SetResult(null);
                                waitTaskCompleted = true;
                            }
                        }
                        else if (!waitForCancellations)
                        {
                            // If a task faulted and we're not waiting for all the tasks to finish their cancellation,
                            // set the exceptions for the wait task.
                            if (task.IsFaulted)
                            {
                                tcs.SetException(exceptions);
                                waitTaskCompleted = true;
                            }
                            // If a task canceled and we're not waiting for all the tasks to finish their cancellation,
                            // set the wait task as canceled
                            else if (task.IsCanceled)
                            {
                                tcs.TrySetCanceled(firstTaskCanceled.CancellationTokenSource.Token);
                                waitTaskCompleted = true;
                            }
                        }

                        // Cancel all tasks if any task faults or is canceled.
                        // Do this last because the continuation for the tasks we're cancelling can run
                        // synchronously if the task has not been started yet and is associated with a
                        // cancellation token.
                        if ((task.IsCanceled || task.IsFaulted) && !cancellationsIssued)
                        {
                            cancellationsIssued = true;

                            foreach (ICancellableTask taskToCancel in tasks)
                            {
                                taskToCancel.Cancel();
                            }
                        }
                    }
                }, TaskContinuationOptions.ExecuteSynchronously);
            }

            return tcs.Task;
        }
    }
}

// Copyright (C) 2017 Greg Najda
//
// This file is part of AnimeRecs.Utils
//
// AnimeRecs.Utils is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.Utils is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.Utils.  If not, see <http://www.gnu.org/licenses/>.