
using System;
using System.Text;

namespace Quest.API.Services
{
    public class TeamSecretService
    {
        public static string GenerateTeamToken(int size)
        {
            var builder = new StringBuilder(); 
            builder.Append(RandomString(size / 2));
            builder.Append(RandomString(size / 2 + size % 2));  
            return builder.ToString();   
        }
        
        private static string RandomString(int size)  
        {  
            var builder = new StringBuilder();  
            var random = new Random();
            for (var i = 0; i < size; ++i)
            {
                //Todo: Remove Hardcoded values
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();  
        }
    }
}