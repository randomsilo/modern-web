using System;
using Brash.Infrastructure;
using Serilog;
using LoyalGuard.Domain.Model;
using LoyalGuard.Domain.Service;
using LoyalGuard.Infrastructure.Utility;

namespace LoyalGuard.Infrastructure.Sqlite.Service
{
	public class AuthService : IAuthService
	{
		protected ILogger Logger { get; set; }
    protected LGAccountService _lGAccountService { get; set; }
    protected LGPrivilegeService _lGPrivilegeService { get; set; }
    protected LGFeatureService _lGFeatureService { get; set; }
    protected LGAbilityService _lGAbilityService { get; set; }
    protected LGRoleService _lGRoleService { get; set; }
    protected LGTokenService _lGTokenService { get; set; }

		public AuthService(
      LGAccountService lGAccountService
      , LGTokenService lGTokenService
      , LGPrivilegeService lGPrivilegeService
      , LGFeatureService lGFeatureService
      , LGAbilityService lGAbilityService
      , LGRoleService lGRoleService
      , ILogger logger)
		{
			Logger = logger;
      _lGAccountService = lGAccountService;
      _lGTokenService = lGTokenService;
      _lGPrivilegeService = _lGPrivilegeService;
      _lGFeatureService = lGFeatureService;
      _lGAbilityService = lGAbilityService;
      _lGRoleService = lGRoleService;

		}

		public BrashActionResult<AccountAccess> Authenticate(AccountSignin model)
    {
      BrashActionResult<AccountAccess> authResult = new BrashActionResult<AccountAccess>();

      authResult.Status = BrashActionStatus.UNKNOWN;
      authResult.Message = "";
      authResult.Model = new AccountAccess();

      var userName = model.UserName.RemoveSpecialCharacters();
      Logger.Information($"Authenticate -> USER: {model.UserName}, USER_STRIPPED: {userName}, PASS: {model.Password}");
      
      var findAccountQuery = _lGAccountService.FindWhere($"WHERE UserName = '{userName}'");

      if (findAccountQuery.Status == BrashQueryStatus.SUCCESS && findAccountQuery.Models.Count == 1)
      {
        var foundAccount = findAccountQuery.Models[0];

        // confirm password
        if (Hashing.ValidatePassword(model.Password, foundAccount.Password))
        {
          Logger.Information($"Authenticate -> USER: {model.UserName} SUCCESS!");
          
          // set account
          foundAccount.Password = null;

          authResult.Status = BrashActionStatus.SUCCESS;
          authResult.Message = "Authentication successful.";
          authResult.Model.Account = foundAccount;

          // set role name

          // transform priviledges into dictionary (string/list of strings)
          
          // create token
          LGToken token = new LGToken()
          {
            LGAccountId = authResult.Model.Account.LGAccountId,
            Token = $"{Guid.NewGuid().ToString()}-{Guid.NewGuid().ToString()}-{Guid.NewGuid().ToString()}",
            Created = DateTime.Now,
            Expires = DateTime.Now.AddHours(1),
            LastUsed = DateTime.Now
          };

          var createTokenResult =_lGTokenService.Create(token);
          if (createTokenResult.Status != BrashActionStatus.SUCCESS)
          {
            authResult.Status = BrashActionStatus.ERROR;
            authResult.Message = "Authentication service error (101).  Contact technical support.";
          }

          // set token
          authResult.Model.Token = token;
        }
        else
        {
          authResult.Status = BrashActionStatus.NOT_FOUND;
          authResult.Message = "Inccorect password.";
        }
      }
      else if(findAccountQuery.Status == BrashQueryStatus.NO_RECORDS)
      {
        authResult.Status = BrashActionStatus.NOT_FOUND;
        authResult.Message = "Account not found.";
      }
      else
      {
        authResult.Status = BrashActionStatus.ERROR;
        authResult.Message = "Authentication service error (100).  Contact technical support.";
      }

      return authResult;
    }

	}
}