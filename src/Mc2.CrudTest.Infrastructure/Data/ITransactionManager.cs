using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

namespace Mc2.CrudTest.Infrastructure.Data;

public interface ITransactionManager
{
    Task<ITransactionScope> BeginTransactionAsync();
}

public interface ITransactionScope : IDisposable
{
    Task CommitAsync();
    Task RollbackAsync();
}

public class TransactionManager : ITransactionManager
{
    private readonly ApplicationDbContext _context;

    public TransactionManager(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ITransactionScope> BeginTransactionAsync()
    {
        if (_context.Database.IsInMemory())
        {
            return new NoOpTransactionScope();
        }
        
        var transaction = await _context.Database.BeginTransactionAsync();
        return new DatabaseTransactionScope(transaction);
    }
}

public class DatabaseTransactionScope : ITransactionScope
{
    private readonly IDbContextTransaction _transaction;
    private bool _disposed;

    public DatabaseTransactionScope(IDbContextTransaction transaction)
    {
        _transaction = transaction;
    }

    public async Task CommitAsync()
    {
        await _transaction.CommitAsync();
    }

    public async Task RollbackAsync()
    {
        await _transaction.RollbackAsync();
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _transaction?.Dispose();
            _disposed = true;
        }
    }
}

public class NoOpTransactionScope : ITransactionScope
{
    public Task CommitAsync() => Task.CompletedTask;
    public Task RollbackAsync() => Task.CompletedTask;
    public void Dispose() { }
}