using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MySqlConnector;
using SportsHome.Core.Interfaces;
using SportsHome.Core.Interfaces.Repositories;
using SportsHome.IL.Repository.EF.Repositories;

namespace SportsHome.IL.Repository.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SportsHomeContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(SportsHomeContext context)
        {
            _context = context;

            Ligas = new LigasRepository(context);
        }

        public ILigasRepository Ligas { get; private set; }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction != null)
                throw new InvalidOperationException("Ya existe una transacción activa.");

            _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction == null)
                return;

            try
            {
                await _transaction.CommitAsync(cancellationToken);
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction == null)
                return;

            try
            {
                await _transaction.RollbackAsync(cancellationToken);
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task<int> Complete()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException is MySqlException sqlEx)
                {
                    switch (sqlEx.Number)
                    {
                        case 1062:
                            throw new InvalidOperationException("Registro duplicado en la base de datos", sqlEx);

                        case 1451:
                            throw new InvalidOperationException("No se puede eliminar, existen relaciones asociadas", sqlEx);
                    }
                }

                throw;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _transaction = null;

            _context.Dispose();
        }
    }
}