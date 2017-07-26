using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace AnimeRecs.Utils.UnitTests
{
    public class WhenAllCancelOnFirstExceptionTests
    {
        [Fact]
        public void PassingNoTasksReturnsInstantlyWaitForCancellations()
        {
            Task task = AsyncUtils.WhenAllCancelOnFirstExceptionWaitForCancellations(new CancellableTask[0]);
            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
        }

        [Fact]
        public void PassingNoTasksReturnsInstantlyDontWaitForCancellations()
        {
            Task task = AsyncUtils.WhenAllCancelOnFirstExceptionDontWaitForCancellations(new CancellableTask[0]);
            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
        }

        [Fact]
        public void TypeOfExceptionWhenAwaitingIsNotAggregateException()
        {
            CancellableTask[] tasks = new CancellableTask[]
            {
                GetLateFaultingCancellableTask()
            };

            try
            {
                Task waitTask = AsyncUtils.WhenAllCancelOnFirstExceptionWaitForCancellations(tasks);
                Exception exceptionWhenAwaiting = GetExceptionWhenAwaiting(waitTask).ConfigureAwait(false).GetAwaiter().GetResult();
                Assert.IsType<Exception>(exceptionWhenAwaiting);
            }
            finally
            {
                foreach (CancellableTask task in tasks)
                {
                    task.CancellationTokenSource.Dispose();
                }
            }
        }

        private async Task<Exception> GetExceptionWhenAwaiting(Task waitTask)
        {
            try
            {
                await waitTask.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return ex;
            }
            throw new Exception("Task did not throw an exception.");
        }

        [Fact]
        public void AllCompleteSuccessfullyWaitForCancellations()
        {
            CancellableTask[] tasks = new CancellableTask[3]
            {
                GetSuccessfulCancellableTask(),
                GetSuccessfulCancellableTask(),
                GetSuccessfulCancellableTask()
            };

            try
            {
                Task waitTask = AsyncUtils.WhenAllCancelOnFirstExceptionWaitForCancellations(tasks);
                waitTask.ConfigureAwait(false).GetAwaiter().GetResult();
                Assert.All(tasks, task => Assert.Equal(TaskStatus.RanToCompletion, task.Task.Status));
            }
            finally
            {
                foreach (CancellableTask task in tasks)
                {
                    task.CancellationTokenSource.Dispose();
                }
            }
        }

        [Fact]
        public void AllCompleteSuccessfullyDontWaitForCancellations()
        {
            CancellableTask[] tasks = new CancellableTask[3]
            {
                GetSuccessfulCancellableTask(),
                GetSuccessfulCancellableTask(),
                GetSuccessfulCancellableTask()
            };

            try
            {
                Task waitTask = AsyncUtils.WhenAllCancelOnFirstExceptionDontWaitForCancellations(tasks);
                waitTask.ConfigureAwait(false).GetAwaiter().GetResult();
                Assert.All(tasks, task => Assert.Equal(TaskStatus.RanToCompletion, task.Task.Status));
            }
            finally
            {
                foreach (CancellableTask task in tasks)
                {
                    task.CancellationTokenSource.Dispose();
                }
            }
        }

        [Fact]
        public void FaultsAfterSuccessesWaitForCancellations()
        {
            // 2 tasks fault, others succeed, faults occur after successes, waitForCancellations true
            // -> AggregateException with both faults inside
            CancellableTask[] tasks = new CancellableTask[]
            {
                GetLateFaultingCancellableTask(),
                GetLateFaultingCancellableTask(),
                GetSuccessfulCancellableTask(),
                GetSuccessfulCancellableTask()
            };
            try
            {
                Task waitTask = AsyncUtils.WhenAllCancelOnFirstExceptionWaitForCancellations(tasks);

                Assert.Throws<Exception>(() => waitTask.ConfigureAwait(false).GetAwaiter().GetResult());
                Assert.Equal(2, waitTask.Exception.InnerExceptions.Count);
                Assert.All(waitTask.Exception.InnerExceptions, ex => Assert.Equal("Late Fault", ex.Message));

                Assert.Equal(TaskStatus.Faulted, tasks[0].Task.Status);
                Assert.Equal(TaskStatus.Faulted, tasks[1].Task.Status);
                Assert.Equal(TaskStatus.RanToCompletion, tasks[2].Task.Status);
                Assert.Equal(TaskStatus.RanToCompletion, tasks[3].Task.Status);
            }
            finally
            {
                foreach (CancellableTask task in tasks)
                {
                    task.CancellationTokenSource.Dispose();
                }
            }
        }

        [Fact]
        public void FaultsAfterSuccessesDontWaitForCancellations()
        {
            // 2 tasks fault, others succeed, faults occur after successes, waitForCancellations false
            // -> AggregateException with at least one fault inside
            CancellableTask[] tasks = new CancellableTask[]
            {
                GetLateFaultingCancellableTask(),
                GetLateFaultingCancellableTask(),
                GetSuccessfulCancellableTask(),
                GetSuccessfulCancellableTask()
            };
            try
            {
                Task waitTask = AsyncUtils.WhenAllCancelOnFirstExceptionDontWaitForCancellations(tasks);

                Assert.Throws<Exception>(() => waitTask.ConfigureAwait(false).GetAwaiter().GetResult());
                Assert.Equal(1, waitTask.Exception.InnerExceptions.Count);
                Assert.Equal("Late Fault", waitTask.Exception.InnerException.Message);

                TaskStatus task0Status = tasks[0].Task.Status;
                TaskStatus task1Status = tasks[1].Task.Status;
                int numFaulted = (task0Status == TaskStatus.Faulted ? 1 : 0) + (task1Status == TaskStatus.Faulted ? 1 : 0);
                numFaulted.Should().BeGreaterOrEqualTo(1);
                Assert.True(task0Status == TaskStatus.Faulted || task0Status == TaskStatus.Running || task0Status == TaskStatus.WaitingForActivation, string.Format("First task has status {0} instead of Faulted, Running, or WaitingForActivation.", task0Status));
                Assert.True(task1Status == TaskStatus.Faulted || task1Status == TaskStatus.Running || task1Status == TaskStatus.WaitingForActivation, string.Format("Second task has status {0} instead of Faulted, Running, or WaitingForActivation.", task1Status));
                Assert.Equal(TaskStatus.RanToCompletion, tasks[2].Task.Status);
                Assert.Equal(TaskStatus.RanToCompletion, tasks[3].Task.Status);
            }
            finally
            {
                foreach (CancellableTask task in tasks)
                {
                    task.CancellationTokenSource.Dispose();
                }
            }
        }

        [Fact]
        public void FaultsBeforeSuccessesWaitForCancellation()
        {
            // 2 tasks fault, others basically wait for cancellation and wait a bit after receiving cancellation,
            // fault occurs before successes, waitForCancellations true
            // -> AggregateException with faults inside, tasks in faulted state, other tasks are in canceled state
            CancellableTask[] tasks = new CancellableTask[]
            {
                GetEarlyFaultingCancellableTask(),
                GetEarlyFaultingCancellableTask(),
                GetSuccessfulCancellableTask(),
                GetSuccessfulCancellableTask()
            };

            try
            {
                Task waitTask = AsyncUtils.WhenAllCancelOnFirstExceptionWaitForCancellations(tasks);

                Assert.Throws<Exception>(() => waitTask.ConfigureAwait(false).GetAwaiter().GetResult());
                Assert.Equal(2, waitTask.Exception.InnerExceptions.Count);
                Assert.All(waitTask.Exception.InnerExceptions, ex => Assert.Equal("Early Fault", ex.Message));

                Assert.Equal(TaskStatus.Faulted, tasks[0].Task.Status);
                Assert.Equal(TaskStatus.Faulted, tasks[1].Task.Status);
                Assert.Equal(TaskStatus.Canceled, tasks[2].Task.Status);
                Assert.Equal(TaskStatus.Canceled, tasks[3].Task.Status);
            }
            finally
            {
                foreach (CancellableTask task in tasks)
                {
                    task.CancellationTokenSource.Dispose();
                }
            }
        }

        [Fact]
        public void FaultsBeforeSuccessesDontWaitForCancellation()
        {
            // 2 tasks fault, others basically wait for cancellation and wait a bit after receiving cancellation,
            // fault occurs before successes, waitForCancellations false
            // -> AggregateException with at least 1 fault inside, at least one of first two tasks in faulted state, other tasks are not completed yet
            CancellableTask[] tasks = new CancellableTask[]
            {
                GetEarlyFaultingCancellableTask(),
                GetEarlyFaultingCancellableTask(),
                GetSuccessfulCancellableTask(),
                GetSuccessfulCancellableTask()
            };

            try
            {
                Task waitTask = AsyncUtils.WhenAllCancelOnFirstExceptionDontWaitForCancellations(tasks);

                Assert.Throws<Exception>(() => waitTask.ConfigureAwait(false).GetAwaiter().GetResult());
                Assert.Equal(1, waitTask.Exception.InnerExceptions.Count);
                Assert.Equal("Early Fault", waitTask.Exception.InnerException.Message);

                TaskStatus task0Status = tasks[0].Task.Status;
                TaskStatus task1Status = tasks[1].Task.Status;
                TaskStatus task2Status = tasks[2].Task.Status;
                TaskStatus task3Status = tasks[3].Task.Status;

                int numFaulted = (task0Status == TaskStatus.Faulted ? 1 : 0) + (task1Status == TaskStatus.Faulted ? 1 : 0);
                numFaulted.Should().BeGreaterOrEqualTo(1);
                Assert.True(task0Status == TaskStatus.Faulted || task0Status == TaskStatus.Running || task0Status == TaskStatus.WaitingForActivation, string.Format("First task has status {0} instead of Faulted, Running, or WaitingForActivation.", task0Status));
                Assert.True(task1Status == TaskStatus.Faulted || task1Status == TaskStatus.Running || task1Status == TaskStatus.WaitingForActivation, string.Format("Second task has status {0} instead of Faulted, Running, or WaitingForActivation.", task1Status));
                Assert.True(task2Status == TaskStatus.Running || task2Status == TaskStatus.WaitingForActivation, string.Format("Third task has status {0} instead of Running or WaitingForActivation.", task2Status));
                Assert.True(task3Status == TaskStatus.Running || task3Status == TaskStatus.WaitingForActivation, string.Format("Fourth task has status {0} instead of Running or WaitingForActivation.", task3Status));
            }
            finally
            {
                foreach (CancellableTask task in tasks)
                {
                    task.CancellationTokenSource.Dispose();
                }
            }
        }

        [Fact]
        public void CancelAfterSuccessesWaitForCancellation()
        {
            // 1 task canceled, others succeed, cancel occurs after successes, waitForCancellations true
            // -> TaskCanceledException with CancellationToken == canceled task's cancellation token
            CancellableTask[] tasks = new CancellableTask[]
            {
                GetLateCancellingCancellableTask(),
                GetSuccessfulCancellableTask(),
                GetSuccessfulCancellableTask()
            };

            try
            {
                Task waitTask = AsyncUtils.WhenAllCancelOnFirstExceptionWaitForCancellations(tasks);

                bool taskCanceledCaught = false;
                try
                {
                    waitTask.ConfigureAwait(false).GetAwaiter().GetResult();
                }
                catch (TaskCanceledException ex)
                {
                    taskCanceledCaught = true;
                    ex.CancellationToken.Should().Be(tasks[0].CancellationTokenSource.Token);
                }

                Assert.True(taskCanceledCaught, "TaskCanceledException was not thrown.");
                Assert.Equal(TaskStatus.Canceled, tasks[0].Task.Status);
                Assert.Equal(TaskStatus.RanToCompletion, tasks[1].Task.Status);
                Assert.Equal(TaskStatus.RanToCompletion, tasks[2].Task.Status);
            }
            finally
            {
                foreach (CancellableTask task in tasks)
                {
                    task.CancellationTokenSource.Dispose();
                }
            }
        }

        [Fact]
        public void CancelAfterSuccessesDontWaitForCancellation()
        {
            // 1 task canceled, others succeed, cancel occurs after successes, waitForCancellations false
            // -> TaskCanceledException with CancellationToken == canceled task's cancellation token
            CancellableTask[] tasks = new CancellableTask[]
            {
                GetLateCancellingCancellableTask(),
                GetSuccessfulCancellableTask(),
                GetSuccessfulCancellableTask()
            };

            try
            {
                Task waitTask = AsyncUtils.WhenAllCancelOnFirstExceptionDontWaitForCancellations(tasks);
                bool taskCanceledCaught = false;
                try
                {
                    waitTask.ConfigureAwait(false).GetAwaiter().GetResult();
                }
                catch (TaskCanceledException ex)
                {
                    taskCanceledCaught = true;
                    ex.CancellationToken.Should().Be(tasks[0].CancellationTokenSource.Token);
                }

                Assert.True(taskCanceledCaught, "TaskCanceledException was not thrown.");
                Assert.Equal(TaskStatus.Canceled, tasks[0].Task.Status);
                Assert.Equal(TaskStatus.RanToCompletion, tasks[1].Task.Status);
                Assert.Equal(TaskStatus.RanToCompletion, tasks[2].Task.Status);
            }
            finally
            {
                foreach (CancellableTask task in tasks)
                {
                    task.CancellationTokenSource.Dispose();
                }
            }
        }

        [Fact]
        public void CancelBeforeOthersWaitForCancellation()
        {
            // 1 task canceled, others basically wait for cancellation and wait a bit after receiving cancellation,
            // cancel occurs before successes, waitForCancellation true
            // -> TaskCanceledException with CancellationToken == canceled task's cancellation token
            CancellableTask[] tasks = new CancellableTask[]
            {
                GetEarlyCancellingCancellableTask(),
                GetSuccessfulCancellableTask(),
                GetSuccessfulCancellableTask()
            };

            try
            {
                Task waitTask = AsyncUtils.WhenAllCancelOnFirstExceptionWaitForCancellations(tasks);
                bool taskCanceledCaught = false;
                try
                {
                    waitTask.ConfigureAwait(false).GetAwaiter().GetResult();
                }
                catch (TaskCanceledException ex)
                {
                    taskCanceledCaught = true;
                    ex.CancellationToken.Should().Be(tasks[0].CancellationTokenSource.Token);
                }

                Assert.True(taskCanceledCaught, "TaskCanceledException was not thrown.");
                Assert.Equal(TaskStatus.Canceled, tasks[0].Task.Status);
                Assert.Equal(TaskStatus.Canceled, tasks[1].Task.Status);
                Assert.Equal(TaskStatus.Canceled, tasks[2].Task.Status);
            }
            finally
            {
                foreach (CancellableTask task in tasks)
                {
                    task.CancellationTokenSource.Dispose();
                }
            }
        }

        [Fact]
        public void CancelBeforeOthersFaultAfterCancellationWaitForCancellation()
        {
            // 1 task canceled, others basically wait for cancellation and wait a bit after receiving cancellation,
            // 2 others fault after cancellation, cancel occurs before successes and faults, waitForCancellation true
            // -> AggregateException with faults inside, first task in canceled state, faulting tasks in faulted state, other tasks in canceled state

            CancellableTask[] tasks = new CancellableTask[]
            {
                GetEarlyCancellingCancellableTask(),
                GetLateFaultingCancellableTask(),
                GetLateFaultingCancellableTask(),
                GetSuccessfulCancellableTask(),
                GetSuccessfulCancellableTask()
            };

            try
            {
                Task waitTask = AsyncUtils.WhenAllCancelOnFirstExceptionWaitForCancellations(tasks);

                Assert.Throws<Exception>(() => waitTask.ConfigureAwait(false).GetAwaiter().GetResult());
                Assert.Equal(2, waitTask.Exception.InnerExceptions.Count);
                Assert.All(waitTask.Exception.InnerExceptions, ex => Assert.Equal("Late Fault", ex.Message));

                Assert.Equal(TaskStatus.Canceled, tasks[0].Task.Status);
                Assert.Equal(TaskStatus.Faulted, tasks[1].Task.Status);
                Assert.Equal(TaskStatus.Faulted, tasks[2].Task.Status);
                Assert.Equal(TaskStatus.Canceled, tasks[3].Task.Status);
                Assert.Equal(TaskStatus.Canceled, tasks[4].Task.Status);
            }
            finally
            {
                foreach (CancellableTask task in tasks)
                {
                    task.CancellationTokenSource.Dispose();
                }
            }
        }

        [Fact]
        public void CancelBeforeOthersDontWaitForCancellation()
        {
            // 1 task canceled, others basically wait for cancellation and wait a bit after receiving cancellation,
            // cancel occurs before successes, waitForCancellation false
            // -> TaskCanceledException with Task == canceled task, other tasks are not completed yet

            CancellableTask[] tasks = new CancellableTask[]
            {
                GetEarlyCancellingCancellableTask(),
                GetSuccessfulCancellableTask(),
                GetSuccessfulCancellableTask()
            };

            try
            {
                Task waitTask = AsyncUtils.WhenAllCancelOnFirstExceptionDontWaitForCancellations(tasks);
                bool taskCanceledCaught = false;
                try
                {
                    waitTask.ConfigureAwait(false).GetAwaiter().GetResult();
                }
                catch (TaskCanceledException ex)
                {
                    taskCanceledCaught = true;
                    ex.CancellationToken.Should().Be(tasks[0].CancellationTokenSource.Token);
                }

                Assert.True(taskCanceledCaught, "TaskCanceledException was not thrown.");
                TaskStatus task1Status = tasks[1].Task.Status;
                TaskStatus task2Status = tasks[2].Task.Status;
                Assert.Equal(TaskStatus.Canceled, tasks[0].Task.Status);
                Assert.True(task1Status == TaskStatus.Running || task1Status == TaskStatus.WaitingForActivation, string.Format("Second task has status {0} instead of Running or WaitingForActivation.", task1Status));
                Assert.True(task2Status == TaskStatus.Running || task2Status == TaskStatus.WaitingForActivation, string.Format("Third task has status {0} instead of Running or WaitingForActivation.", task2Status));
            }
            finally
            {
                foreach (CancellableTask task in tasks)
                {
                    task.CancellationTokenSource.Dispose();
                }
            }
        }

        [Fact]
        public void CancelBeforeOthersFaultAfterCancellationDontWaitForCancellation()
        {
            // 1 task canceled, others basically wait for cancellation and wait a bit after receiving cancellation,
            // 2 others fault after cancellation, cancel occurs before successes and faults, waitForCancellation false
            // -> TaskCanceledException with Task == canceled task, other tasks are not completed yet

            CancellableTask[] tasks = new CancellableTask[]
            {
                GetEarlyCancellingCancellableTask(),
                GetLateFaultingCancellableTask(),
                GetLateFaultingCancellableTask(),
                GetSuccessfulCancellableTask(),
                GetSuccessfulCancellableTask()
            };

            try
            {
                Task waitTask = AsyncUtils.WhenAllCancelOnFirstExceptionDontWaitForCancellations(tasks);
                bool taskCanceledCaught = false;
                try
                {
                    waitTask.ConfigureAwait(false).GetAwaiter().GetResult();
                }
                catch (TaskCanceledException ex)
                {
                    taskCanceledCaught = true;
                    ex.CancellationToken.Should().Be(tasks[0].CancellationTokenSource.Token);
                }

                TaskStatus task1Status = tasks[1].Task.Status;
                TaskStatus task2Status = tasks[2].Task.Status;
                TaskStatus task3Status = tasks[3].Task.Status;
                TaskStatus task4Status = tasks[4].Task.Status;

                Assert.True(taskCanceledCaught, "TaskCanceledException was not thrown.");
                Assert.Equal(TaskStatus.Canceled, tasks[0].Task.Status);
                Assert.True(task1Status == TaskStatus.Running || task1Status == TaskStatus.WaitingForActivation, string.Format("Second task has status {0} instead of Running or WaitingForActivation.", task1Status));
                Assert.True(task2Status == TaskStatus.Running || task2Status == TaskStatus.WaitingForActivation, string.Format("Third task has status {0} instead of Running or WaitingForActivation.", task2Status));
                Assert.True(task3Status == TaskStatus.Running || task3Status == TaskStatus.WaitingForActivation, string.Format("Fourth task has status {0} instead of Running or WaitingForActivation.", task3Status));
                Assert.True(task4Status == TaskStatus.Running || task4Status == TaskStatus.WaitingForActivation, string.Format("Fifth task has status {0} instead of Running or WaitingForActivation.", task4Status));
            }
            finally
            {
                foreach (CancellableTask task in tasks)
                {
                    task.CancellationTokenSource.Dispose();
                }
            }
        }

        private CancellableTask GetSuccessfulCancellableTask()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            return new CancellableTask(GetSuccessfulTask(cts.Token), cts);
        }

        private async Task GetSuccessfulTask(CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(200));
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Delay(TimeSpan.FromMilliseconds(200));
        }

        private CancellableTask GetLateFaultingCancellableTask()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            return new CancellableTask(GetLateFaultingTask(cts.Token), cts);
        }

        private async Task GetLateFaultingTask(CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(600));
            throw new Exception("Late Fault");
        }

        private CancellableTask GetEarlyFaultingCancellableTask()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            return new CancellableTask(GetEarlyFaultingTask(cts.Token), cts);
        }

        private async Task GetEarlyFaultingTask(CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            throw new Exception("Early Fault");
        }

        private CancellableTask GetLateCancellingCancellableTask()
        {
            CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
            return new CancellableTask(GetLateCancellingTask(cts.Token), cts);
        }

        private async Task GetLateCancellingTask(CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(600));
            cancellationToken.ThrowIfCancellationRequested();
        }

        private CancellableTask GetEarlyCancellingCancellableTask()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            cts.Cancel();
            return new CancellableTask(GetEarlyCancellingTask(cts.Token), cts);
        }

        private async Task GetEarlyCancellingTask(CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}

// Copyright (C) 2017 Greg Najda
//
// This file is part of AnimeRecs.Utils.UnitTests
//
// AnimeRecs.Utils.UnitTests is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.Utils.UnitTests is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.Utils.UnitTests.  If not, see <http://www.gnu.org/licenses/>.
