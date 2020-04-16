using NUnit.Framework;
using Quest.Domain.Services;

namespace Quest.UnitTests.Domain.Services
{
    [TestFixture]
    public class TeamService_Should
    {
        private readonly TeamService _teamService = new TeamService();
        [Test]
        public void GenerateRandom_ReturnsRandomValues()
        {
            var result = _teamService.GenerateTeamToken(6);
            var result2 = _teamService.GenerateTeamToken(6);
            
            Assert.AreNotEqual(result, result2);   
        }
    }
}