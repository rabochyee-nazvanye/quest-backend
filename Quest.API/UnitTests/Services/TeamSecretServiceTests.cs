using NUnit.Framework;
using Quest.API.Services;

namespace Quest.API.Tests.Services
{
    [TestFixture]
    public class TeamSecretService_Should
    {
        [Test]
        public void GenerateRandom_ReturnsRandomValues()
        {
            var result = TeamSecretService.GenerateTeamToken(6);
            var result2 = TeamSecretService.GenerateTeamToken(6);
            
            Assert.AreNotEqual(result, result2);   
        }
    }
}