using System;
using System.Text.RegularExpressions;

namespace LoyalGuard.Infrastructure.Utility
{
	public class Hashing
  {
    private static string GetRandomSalt()
    {
        return BCrypt.Net.BCrypt.GenerateSalt(12);
    }

    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, GetRandomSalt());
    }

    public static bool ValidatePassword(string password, string correctHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, correctHash);
    }

    public static bool isHashed(string possiblyHashedString)
    {
      Regex rx = new Regex(@"\A\$2a?\$\d\d\$[./0-9A-Za-z] {53}", RegexOptions.Compiled);
      MatchCollection matches = rx.Matches(possiblyHashedString);
      return matches.Count > 0;
    }
  }
}