using System;
using System.Collections.Generic;
using System.Linq;
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
      _lGPrivilegeService = lGPrivilegeService;
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
          var fetchRoleResult =_lGRoleService.Fetch(new LGRole() {
            LGRoleId = foundAccount.RoleIdRef
          });

          if (fetchRoleResult.Status == BrashActionStatus.SUCCESS)
          {
            authResult.Model.Role = fetchRoleResult.Model.ChoiceName;
          }
          else 
          {
            Logger.Error(fetchRoleResult.CaughtException, $"Failed getting the role.  Why? Check this: {fetchRoleResult.Message}");
            authResult.Model.Role = "Unknown";
          }

          // transform priviledges into dictionary (string/list of strings)
          authResult.Model.Privileges = new Dictionary<string, List<string>>();
          var getPrivledgesResult =_lGPrivilegeService.FindWhere($"WHERE LGAccountId = {foundAccount.LGAccountId}");
          if (getPrivledgesResult.Status == BrashQueryStatus.SUCCESS)
          {
            var allFeatures = _lGFeatureService.FindWhere("WHERE 1 = 1").Models;
            var allAbilities = _lGAbilityService.FindWhere("WHERE 1 = 1").Models;

            foreach( var priviledge in getPrivledgesResult.Models)
            {
              // get feature
              var feature = allFeatures.Where(f => f.LGFeatureId == priviledge.FeatureIdRef).FirstOrDefault();
              List<string> featureAbilityList = new List<string>();

              if (authResult.Model.Privileges.Keys.Contains(feature.ChoiceName))
              {
                featureAbilityList = authResult.Model.Privileges[feature.ChoiceName];
              }
              else
              {
                authResult.Model.Privileges.Add(feature.ChoiceName, featureAbilityList);
              }

              // get action
              var ability = allAbilities.Where(a => a.LGAbilityId == priviledge.AbilityIdRef).FirstOrDefault();

              // set values in response
              featureAbilityList.Add(ability.ChoiceName);
            }
          }
          else
          {
            Logger.Error(getPrivledgesResult.CaughtException, $"Failed getting user privledges.  Why? Check this: {getPrivledgesResult.Message}");
            authResult.Model.Privileges.Add("ERROR", new List<string>() { "GETTING_PRIVLEDGES" });
          }
          
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