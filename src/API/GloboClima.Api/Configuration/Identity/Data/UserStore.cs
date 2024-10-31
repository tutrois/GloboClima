using Microsoft.AspNetCore.Identity;
using AutoMapper;
using GloboClima.API.Configuration.Identity.Models;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
namespace GloboClima.API.Configuration.Identity.Data
{
    public class UserStore :
        IUserStore<ApplicationUser>, 
        IUserEmailStore<ApplicationUser>, 
        IUserPhoneNumberStore<ApplicationUser>,
        IUserTwoFactorStore<ApplicationUser>,
        IUserPasswordStore<ApplicationUser>,
        IUserRoleStore<ApplicationUser>
    {
        private readonly IMapper _mapper;
        private readonly IDynamoDBContext _context;

        public UserStore(
            IMapper mapper, IDynamoDBContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Verifica se já existe um usuário com o mesmo e-mail
            var existingUser = await FindByEmailAsync(user.Email.ToUpper(), cancellationToken);
            if (existingUser != null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "E-mail já está em uso." });
            }

            await _context.SaveAsync(user, cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _context.DeleteAsync<ApplicationUser>(user.PartitionKey, user.UserId, cancellationToken);

            return IdentityResult.Success;
        }

        public async Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var lista = await _context.QueryAsync<ApplicationUser>($"USER#{userId}", Amazon.DynamoDBv2.DocumentModel.QueryOperator.Equal, new object[] { "METADATA" })
             .GetRemainingAsync(cancellationToken);

            return lista.FirstOrDefault();
        }

        public async Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var scanConditions = new List<ScanCondition>
            {
                new ScanCondition("NormalizedUserName", ScanOperator.Equal, normalizedUserName)
            };

            var lista = await _context.ScanAsync<ApplicationUser>(scanConditions).GetRemainingAsync(cancellationToken);

            return lista.FirstOrDefault();
        }

        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public  Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserId.ToString());
        }

        public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.FromResult(0);
        }

        public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.FromResult(0);
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                await _context.SaveAsync(user, cancellationToken);
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = ex.Message });
            }
        }

        public Task SetEmailAsync(ApplicationUser user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public async Task<ApplicationUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var scanConditions = new List<ScanCondition>
            {
                new ScanCondition("NormalizedEmail", ScanOperator.Equal, normalizedEmail)
            };

            var lista = await _context.ScanAsync<ApplicationUser>(scanConditions).GetRemainingAsync(cancellationToken);

            return lista.FirstOrDefault();
        }

        public Task<string> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(ApplicationUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.FromResult(0);
        }

        public Task SetPhoneNumberAsync(ApplicationUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            user.PhoneNumber = phoneNumber;
            return Task.FromResult(0);
        }

        public Task<string> GetPhoneNumberAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled, CancellationToken cancellationToken)
        {
            user.TwoFactorEnabled = enabled;
            return Task.FromResult(0);
        }

        public Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public async Task AddToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                // Carrega o usuário existente do banco de dados
                var existingUser = await _context.LoadAsync<ApplicationUser>(user.PartitionKey, "METADATA", cancellationToken);
                if (existingUser == null)
                {
                    throw new InvalidOperationException("Usuário não encontrado.");
                }

                // Atualiza a função do usuário
                var role = new ApplicationRole()
                {
                    PartitionKey = existingUser.PartitionKey,
                    Name = roleName,
                    NormalizedName = roleName.ToUpper()
                };

                // Salva as alterações no DynamoDB
                await _context.SaveAsync(role, cancellationToken);
            }
            catch (Exception ex)
            {
                // Logue o erro ou trate conforme necessário
                throw new InvalidOperationException("Erro ao adicionar função ao usuário.", ex);
            }
        }

        public async Task RemoveFromRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                // Carrega o usuário existente do banco de dados
                var existingUser = await _context.LoadAsync<ApplicationUser>(user.PartitionKey, "METADATA", cancellationToken);
                if (existingUser == null)
                {
                    throw new InvalidOperationException("Usuário não encontrado.");
                }

                // Define a chave de partição e a chave de classificação para a associação
                var userRoleKey = new ApplicationRole
                {
                    PartitionKey = existingUser.PartitionKey,
                    Name = roleName
                };

                // Remove a associação da tabela UserRoles
                await _context.DeleteAsync(userRoleKey, cancellationToken);
            }
            catch (Exception ex)
            {
                // Logue o erro ou trate conforme necessário
                throw new InvalidOperationException("Erro ao remover função do usuário.", ex);
            }
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                var existingUser = await _context.LoadAsync<ApplicationUser>(user.PartitionKey, "METADATA", cancellationToken);
                if (existingUser == null)
                {
                    throw new InvalidOperationException("Usuário não encontrado.");
                }

                var roles = await _context.QueryAsync<ApplicationRole>(
                    user.PartitionKey.ToString(), 
                    Amazon.DynamoDBv2.DocumentModel.QueryOperator.Equal,
                    new object[] { "ROLE" }).GetRemainingAsync(cancellationToken);

                return roles.Select(r => r.Name).ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erro ao obter funções do usuário.", ex);
            }
        }

        public async Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                var existingUser = await _context.LoadAsync<ApplicationUser>(user.PartitionKey, "METADATA", cancellationToken);
                if (existingUser == null)
                {
                    throw new InvalidOperationException("Usuário não encontrado.");
                }

                var userRoleKey = new ApplicationRole
                {
                    PartitionKey = existingUser.PartitionKey.ToString(),
                    Name = roleName
                };

                var existingRole = await _context.LoadAsync<ApplicationRole>(userRoleKey.PartitionKey, userRoleKey.Name, cancellationToken);

                return existingRole != null;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erro ao verificar se o usuário está na função.", ex);
            }

        }

        public async Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                // Converte o nome da função para maiúsculas para comparação
                var normalizedRoleName = roleName.ToUpper();

                // Consulta para encontrar todos os usuários associados à função
                var roleUsers = await _context.QueryAsync<ApplicationRole>(
                    normalizedRoleName, // Usando o nome da função como chave de partição
                    Amazon.DynamoDBv2.DocumentModel.QueryOperator.Equal,
                    new object[] { "ROLE" }).GetRemainingAsync(cancellationToken);

                // Lista para armazenar os usuários encontrados
                var userIds = roleUsers.Select(ur => ur.PartitionKey).ToList();

                // Lista para armazenar os usuários completos
                var users = new List<ApplicationUser>();

                // Carrega os usuários a partir dos IDs
                foreach (var userId in userIds)
                {
                    var user = await _context.LoadAsync<ApplicationUser>(userId, "METADATA", cancellationToken);
                    if (user != null)
                    {
                        users.Add(user);
                    }
                }

                return users;
            }
            catch (Exception ex)
            {
                // Logue o erro ou trate conforme necessário
                throw new InvalidOperationException("Erro ao obter usuários na função.", ex);
            }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
