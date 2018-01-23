using AsyncExt;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    class EnumerableExtTests
    {
        // batchTimeout bigger than time of execution each particular task -> onSuccess +; onFail -; nextBatch -
        // one task's execution time bigger than timeout -> onSuccess (2 times) +; onFail -; nextBatch (1) + 
        // one task's execution time bigger than timeout and fails -> onSuccess (for all except for failed + one from new batch) +; onFail +; nextBatch (1) +
        // all failed
        // all failed after timeout
        // all successful

        [Test]
        public async Task BatchProcessing_execution_time_of_one_task_bigger_than_timeout()
        {
            // Arrange
            var batchTimeoutInMilliseconds = 1000;
            var tasks = TasksFactory.CreateTasks(500, 1200, 500);
            var counter = new Counter();

            // Act
            await EnumerableExt.BatchProcessing(
                self: tasks,
                batchTimeoutInMilliseconds: batchTimeoutInMilliseconds,
                onSuccess: counter.CountSucceeded,
                onFail: counter.CountFailed,
                nextBatch: counter.CountNextBatches);

            // Assert
            counter.Succeeded.Should().Be(5);
            counter.Failed.Should().Be(0);
            counter.NextBatch.Should().Be(tasks.Count - 1);
        }


        [Test]
        public async Task BatchProcessing_when_all_succeed_within_timeout_boundaries()
        {
            // Arrange
            var batchTimeoutInMilliseconds = 1000;
            var tasks = TasksFactory.CreateTasks(500, 500, 500);
            var counter = new Counter();

            // Act
            await EnumerableExt.BatchProcessing(
                self: tasks,
                batchTimeoutInMilliseconds: batchTimeoutInMilliseconds,
                onSuccess: counter.CountSucceeded,
                onFail: counter.CountFailed,
                nextBatch: counter.CountNextBatches);

            // Assert
            counter.Succeeded.Should().Be(tasks.Count);
            counter.Failed.Should().Be(0);
            counter.NextBatch.Should().Be(0);
        }

        [Test]
        public async Task BatchProcessing_when_all_fail_within_timeout_boundaries()
        {
            // Arrange
            var batchTimeoutInMilliseconds = 1000;
            var tasks = TasksFactory.CreateFailedTasks(500, 500, 500);
            var counter = new Counter();

            // Act
            await EnumerableExt.BatchProcessing(
                self: tasks,
                batchTimeoutInMilliseconds: batchTimeoutInMilliseconds,
                onSuccess: counter.CountSucceeded,
                onFail: counter.CountFailed,
                nextBatch: counter.CountNextBatches);

            // Assert
            counter.Succeeded.Should().Be(0);
            counter.Failed.Should().Be(tasks.Count);
            counter.NextBatch.Should().Be(0);
        }
    }

    class Counter
    {
        public int Succeeded { get; set; }
        public int Failed { get; set; }
        public int NextBatch { get; set; }

        public Task CountSucceeded(IEnumerable<int> items)
        {
            Succeeded += items.Count();
            return Task.CompletedTask;
        }

        public Task CountFailed(IEnumerable<int> items)
        {
            Failed += items.Count();
            return Task.CompletedTask;
        }

        public IEnumerable<Task<int>> CountNextBatches(int batchSize)
        {
            NextBatch += batchSize;
            return TasksFactory.CreateTasks(
                Enumerable.Repeat(0, batchSize).ToArray()
                );
        }
    }

    class TasksFactory
    {
        public static List<Task<int>> CreateTasks(params int[] delays) =>
           delays.Select(x => CreateTask(x)).ToList();

        public static List<Task<int>> CreateFailedTasks(params int[] delays) =>
           delays.Select(x => CreateTask(x, true)).ToList();

        private static async Task<int> CreateTask(int delay, bool isFail = false)
        {
            await Task.Delay(delay);
            if (isFail) throw new InvalidOperationException();
            return delay;
        }
    }
}
