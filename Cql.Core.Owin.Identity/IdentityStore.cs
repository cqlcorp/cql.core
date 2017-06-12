using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

using Cql.Core.Owin.Identity.Repositories;
using Cql.Core.Owin.Identity.Types;

using Microsoft.AspNet.Identity;

namespace Cql.Core.Owin.Identity
{
    public class IdentityStore : IIdentityStore
    {
        private readonly Lazy<IClaimRepository> _claimRepository;
        private readonly Lazy<IClientRepository> _clientRepository;
        private readonly Lazy<IRefreshTokenRepository> _refreshTokenRepository;
        private readonly Lazy<IUserAccessRepository> _userAccessRepository;
        private readonly Lazy<IUserLoginRepository> _userLoginRepository;
        private readonly Lazy<IUserRepository> _userRepository;
        private readonly Lazy<IUserRoleRepository> _userRoleRepository;

        public IdentityStore(
            Lazy<IClaimRepository> claimRepository,
            Lazy<IClientRepository> clientRepository,
            Lazy<IRefreshTokenRepository> refreshTokenRepository,
            Lazy<IUserAccessRepository> userAccessRepository,
            Lazy<IUserLoginRepository> userLoginRepository,
            Lazy<IUserRepository> userRepository,
            Lazy<IUserRoleRepository> userRoleRepository)
        {
            _claimRepository = claimRepository;
            _clientRepository = clientRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _userAccessRepository = userAccessRepository;
            _userLoginRepository = userLoginRepository;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
        }

        public virtual IQueryable<IdentityUser> Users => _userRepository.Value.Users;

        public virtual Task AddClaimAsync(IdentityUser user, Claim claim)
        {
            return _claimRepository.Value.AddClaimAsync(user.Id, claim);
        }

        public virtual Task AddLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            return _userLoginRepository.Value.AddLoginAsync(user, login);
        }

        public virtual Task<bool> AddRefreshToken(RefreshToken token)
        {
            return _refreshTokenRepository.Value.AddRefreshToken(token);
        }

        public virtual Task AddToRoleAsync(IdentityUser user, string roleName)
        {
            return _userRoleRepository.Value.AddToRoleAsync(user, roleName);
        }

        public virtual Task CreateAsync(IdentityUser user)
        {
            return _userRepository.Value.CreateAsync(user);
        }

        public virtual Task DeleteAsync(IdentityUser user)
        {
            return _userRepository.Value.DeleteAsync(user);
        }

        public virtual void Dispose()
        {
        }

        public virtual Task<IdentityUser> FindAsync(UserLoginInfo login)
        {
            return _userLoginRepository.Value.FindAsync(login);
        }

        public virtual Task<IdentityUser> FindByEmailAsync(string email)
        {
            return _userRepository.Value.FindByEmailAsync(email);
        }

        public virtual Task<IdentityUser> FindByIdAsync(int userId)
        {
            return _userRepository.Value.FindByIdAsync(userId);
        }

        public virtual Task<IdentityUser> FindByNameAsync(string userName)
        {
            return _userRepository.Value.FindByNameAsync(userName);
        }

        public virtual Task<Client> FindClientByIdAsync(string clientId)
        {
            return _clientRepository.Value.FindClientByIdAsync(clientId);
        }

        public virtual Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            return _refreshTokenRepository.Value.FindRefreshToken(refreshTokenId);
        }

        public virtual Task<int> GetAccessFailedCountAsync(IdentityUser user)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        public virtual Task<IList<RefreshToken>> GetAllRefreshTokens()
        {
            return _refreshTokenRepository.Value.GetAllRefreshTokens();
        }

        public virtual Task<IList<Claim>> GetClaimsAsync(IdentityUser user)
        {
            return _claimRepository.Value.GetClaimsAsync(user.Id);
        }

        public virtual Task<string> GetEmailAsync(IdentityUser user)
        {
            return Task.FromResult(user.Email);
        }

        public virtual Task<bool> GetEmailConfirmedAsync(IdentityUser user)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public virtual Task<bool> GetLockoutEnabledAsync(IdentityUser user)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        public virtual Task<DateTimeOffset> GetLockoutEndDateAsync(IdentityUser user)
        {
            return Task.FromResult(user.LockoutEndDate.GetValueOrDefault());
        }

        public virtual Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser user)
        {
            return _userLoginRepository.Value.GetLoginsAsync(user);
        }

        public virtual Task<string> GetPasswordHashAsync(IdentityUser user)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public virtual Task<string> GetPhoneNumberAsync(IdentityUser user)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public virtual Task<bool> GetPhoneNumberConfirmedAsync(IdentityUser user)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public virtual Task<IList<string>> GetRolesAsync(IdentityUser user)
        {
            return _userRoleRepository.Value.GetRolesAsync(user);
        }

        public virtual Task<string> GetSecurityStampAsync(IdentityUser user)
        {
            return Task.FromResult(user.SecurityStamp);
        }

        public virtual Task<bool> GetTwoFactorEnabledAsync(IdentityUser user)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public virtual async Task<IdentityResult> GrantAccess(IPrincipal currentUser, int userId)
        {
            await _userAccessRepository.Value.GrantAccess(userId);

            return IdentityResult.Success;
        }

        public virtual Task<bool> HasPasswordAsync(IdentityUser user)
        {
            var hasPassword = !string.IsNullOrWhiteSpace(user.PasswordHash);

            return Task.FromResult(hasPassword);
        }

        public virtual Task<int> IncrementAccessFailedCountAsync(IdentityUser user)
        {
            user.AccessFailedCount++;
            return Task.FromResult(user.AccessFailedCount);
        }

        public virtual Task<bool> IsInRoleAsync(IdentityUser user, string roleName)
        {
            return _userRoleRepository.Value.IsInRoleAsync(user, roleName);
        }

        public virtual Task RemoveClaimAsync(IdentityUser user, Claim claim)
        {
            return _claimRepository.Value.RemoveClaimAsync(user.Id, claim);
        }

        public virtual Task RemoveFromRoleAsync(IdentityUser user, string roleName)
        {
            return _userRoleRepository.Value.RemoveFromRoleAsync(user, roleName);
        }

        public virtual Task RemoveLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            return _userLoginRepository.Value.RemoveLoginAsync(user, login);
        }

        public virtual Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            return _refreshTokenRepository.Value.RemoveRefreshToken(refreshTokenId);
        }

        public virtual Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            return _refreshTokenRepository.Value.RemoveRefreshToken(refreshToken);
        }

        public virtual Task ResetAccessFailedCountAsync(IdentityUser user)
        {
            user.AccessFailedCount = 0;

            return Task.CompletedTask;
        }

        public virtual async Task<IdentityResult> RevokeAccess(IPrincipal currentUser, int userId)
        {
            var currentUserId = currentUser.Identity.GetUserId<int>();

            await _userAccessRepository.Value.RevokeAccess(userId, currentUserId);

            return IdentityResult.Success;
        }

        public virtual Task SetEmailAsync(IdentityUser user, string email)
        {
            user.Email = email;

            return Task.CompletedTask;
        }

        public virtual Task SetEmailConfirmedAsync(IdentityUser user, bool confirmed)
        {
            user.EmailConfirmed = confirmed;

            return Task.CompletedTask;
        }

        public virtual Task SetLockoutEnabledAsync(IdentityUser user, bool enabled)
        {
            user.LockoutEnabled = enabled;

            return Task.CompletedTask;
        }

        public virtual Task SetLockoutEndDateAsync(IdentityUser user, DateTimeOffset lockoutEnd)
        {
            user.LockoutEndDate = lockoutEnd;

            return Task.CompletedTask;
        }

        public virtual Task SetPasswordHashAsync(IdentityUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;

            return Task.CompletedTask;
        }

        public virtual Task SetPhoneNumberAsync(IdentityUser user, string phoneNumber)
        {
            user.PhoneNumber = phoneNumber;

            return Task.CompletedTask;
        }

        public virtual Task SetPhoneNumberConfirmedAsync(IdentityUser user, bool confirmed)
        {
            user.PhoneNumberConfirmed = confirmed;

            return Task.CompletedTask;
        }

        public virtual Task SetSecurityStampAsync(IdentityUser user, string stamp)
        {
            user.SecurityStamp = stamp;

            return Task.CompletedTask;
        }

        public virtual Task SetTwoFactorEnabledAsync(IdentityUser user, bool enabled)
        {
            user.TwoFactorEnabled = enabled;

            return Task.CompletedTask;
        }

        public virtual Task UpdateAsync(IdentityUser user)
        {
            return _userRepository.Value.UpdateAsync(user);
        }
    }
}
