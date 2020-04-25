using System.Collections.Generic;
using System.Linq;
using Quest.API.ViewModels.Teams;
using Quest.Application.DTOs;

namespace Quest.API.ViewModels.Quests
{
    public class QuestProgressboardVM
    {
        public QuestProgressboardVM(QuestProgressboardDTO dto)
        {
            Data = new List<Dictionary<string, string>>();
            
            //populate teams scores
            foreach (var teamProgress in dto.TeamProgresses.OrderByDescending(x => 
                x.Scores.Select(x => x.Value).Sum()))
            {
                var teamInfo = new Dictionary<string, string> {{"teamname", teamProgress.Team.Name}};
                foreach (var taskScore in teamProgress.Scores
                    .OrderBy(x => x.Key.Id))
                {
                    teamInfo.Add(taskScore.Key.Id.ToString(), taskScore.Value.ToString());
                }
                teamInfo.Add("sum", teamProgress.Scores.Select(x => x.Value).Sum().ToString());
                Data.Add(teamInfo);
            }
           
            //populate team name column data
            var teamNameHeading = new
            {
                title = "Имя команды",
                dataIndex = "teamname",
                key = "teamname",
                @fixed = "left",
                width = 200
            };
            //populate team score sum column data
            var teamScoreHeading = new
            {
                title = "Сумма",
                dataIndex = "sum",
                key = "sum",
                width = 50
            };
            
            var taskGroupHeadings = new List<object>();
            //populate other columns data
            foreach (var tasksOfGroup in dto.TasksByGroupName)
            {
                var childTasks = new List<object>();
                foreach (var task in tasksOfGroup)
                {
                    var taskInfo = new
                    {
                        title = task.Name,
                        dataIndex = task.Id.ToString(),
                        key = task.Id.ToString(),
                        width = 50
                    };
                    childTasks.Add(taskInfo);
                }

                taskGroupHeadings.Add(new
                {
                    title = tasksOfGroup.Key,
                    children = childTasks
                });
            }

            Columns = new List<object> {teamNameHeading, teamScoreHeading};
            Columns.AddRange(taskGroupHeadings);
        }
        
        public List<object> Columns { get; set; }
        public List<Dictionary<string, string>> Data { get; set; }
    }
}