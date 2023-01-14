using System;
using System.Threading;
using System.Threading.Tasks;
using Whist.Rules;

namespace Whist.Server;

public sealed class GameTaskManager : IAsyncDisposable
{
    private readonly IMovePrompter _movePrompter;
    private CancellationTokenSource? _cancellationTokenSource;
    private Task? _gameTask;

    public GameTaskManager(IMovePrompter movePrompter)
    {
        _movePrompter = movePrompter;
    }

    public async ValueTask DisposeAsync() => await StopGame();

    public void StartGame()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _gameTask = Task.Run(async () =>
        {
            var gameConductor = new GameConductor(_movePrompter);
            await gameConductor.ConductGame().WaitAsync(_cancellationTokenSource.Token).ConfigureAwait(false);
        });
    }

    public async Task StopGame()
    {
        _cancellationTokenSource?.Cancel();
        try
        {
            if (_gameTask != null)
                await _gameTask;
        }
        catch (OperationCanceledException)
        {
            // NOTE(jrgfogh): We know that the task has been cancelled.
        }
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;
    }
}