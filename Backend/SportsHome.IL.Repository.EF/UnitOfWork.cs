using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SportsHome.Core.Interfaces;
using SportsHome.IL.Repository.EF.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsHome.IL.Repository.EF
{
    public class UnitOfWork : IUnitOfWork 
    {
        private readonly SportsHomeContext _context;
        private readonly UserHelper _userHelper;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(SportsHomeContext context, UserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }




        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("Ya existe una transacción activa en el UnitOfWork.");
            }
            _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction is null)
            {
                throw new InvalidOperationException("No existe una transacción activa para confirmar.");
            }
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
            if (_transaction is null)
            {
                return;
            }
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
                if (e.InnerException.GetType().Equals(typeof(SqlException)))
                {
                    SqlException sqlEx = (SqlException)e.InnerException;
                    switch (sqlEx.Number) 
                    { 
                        case 547:
                            throw new InvalidOperationException("5000 - Se ha producido un conflicto relacionado con el registro", sqlEx);
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
