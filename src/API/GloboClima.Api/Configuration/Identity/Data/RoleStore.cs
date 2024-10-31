using Amazon.DynamoDBv2.DataModel;
using GloboClima.API.Configuration.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace GloboClima.API.Configuration.Identity.Data
{
    public class RoleStore : IRoleStore<ApplicationRole>
    {
     
        private readonly IDynamoDBContext _context;


        public RoleStore(IDynamoDBContext context)
        {
            _context = context;
        }


        public async Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                await _context.SaveAsync(role, cancellationToken);

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                // Logue o erro ou trate conforme necessário
                return IdentityResult.Failed(new IdentityError { Description = $"Erro ao criar função: {ex.Message}" });
            }
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                // Utiliza a chave de partição e a chave de classificação do objeto role
                await _context.DeleteAsync<ApplicationRole>(role.PartitionKey, role.SortKey, cancellationToken);

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                // Logue o erro ou trate conforme necessário
                return IdentityResult.Failed(new IdentityError { Description = $"Erro ao excluir a função: {ex.Message}" });
            }
        }

        public async void Dispose()
        {
            //throw new NotImplementedException();
        }

        public async Task<ApplicationRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                string partitionKey = $"USER#{roleId}"; 
                string sortKey = "ROLE"; 

                var role = await _context.LoadAsync<ApplicationRole>(partitionKey, sortKey, cancellationToken);

                return role; 
            }
            catch (Exception ex)
            {
                // Logue o erro ou trate conforme necessário
                throw new InvalidOperationException("Erro ao buscar função.", ex);
            }
        }

        public async Task<ApplicationRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                // Realiza a consulta na tabela com base na chave de partição e valor normalizado
                var roles = await _context.QueryAsync<ApplicationRole>(
                    $"USER#{normalizedRoleName.ToUpper()}", // Chave de partição com o nome normalizado
                    Amazon.DynamoDBv2.DocumentModel.QueryOperator.Equal,
                    new object[] { "ROLE" }).GetRemainingAsync(cancellationToken);

                return roles.FirstOrDefault(); // Retorna o primeiro papel encontrado ou null
            }
            catch (Exception ex)
            {
                // Logue o erro ou trate conforme necessário
                throw new InvalidOperationException("Erro ao buscar função pelo nome normalizado.", ex);
            }
        }

        public Task<string> GetNormalizedRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public Task<string> GetRoleIdAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.PartitionKey.ToString());
        }

        public Task<string> GetRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(ApplicationRole role, string normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            return Task.FromResult(0);
        }

        public Task SetRoleNameAsync(ApplicationRole role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.FromResult(0);
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                await _context.SaveAsync(role, cancellationToken);

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                // Logue o erro ou trate conforme necessário
                return IdentityResult.Failed(new IdentityError { Description = $"Erro ao atualizar a função: {ex.Message}" });
            }
        }
    }
}
