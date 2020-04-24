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
            foreach (var teamProgress in dto.TeamProgresses)
            {
                var teamInfo = new Dictionary<string, string> {{"teamname", teamProgress.Team.Name}};
                foreach (var taskScore in teamProgress.Scores
                    .OrderBy(x => x.Key.Id))
                {
                    teamInfo.Add(taskScore.Key.Id.ToString(), taskScore.Value.ToString());
                }
                Data.Add(teamInfo);
            }
           
            //populate first column data
            var firstHeading = new Dictionary<string, string>();
            firstHeading.Add("title", "Имя команды");
            firstHeading.Add("dataIndex", "teamname");
            firstHeading.Add("key", "teamname");
            firstHeading.Add("fixed", "left");

            var taskGroupHeadings = new List<object>();
            //populate other columns data
            foreach (var tasksOfGroup in dto.TasksByGroupName)
            {
                var childTasks = new List<Dictionary<string, string>>();
                foreach (var task in tasksOfGroup)
                {
                    var taskInfo = new Dictionary<string, string>();
                    taskInfo.Add("title", task.Name);
                    taskInfo.Add("dataIndex", task.Id.ToString());
                    taskInfo.Add("key", task.Id.ToString());
                    childTasks.Add(taskInfo);
                }

                taskGroupHeadings.Add(new
                {
                    title = tasksOfGroup.Key,
                    children = childTasks
                });
            }

            Columns = new List<object> {firstHeading};
            Columns.AddRange(taskGroupHeadings);
        }
        
        public List<object> Columns { get; set; }
        public List<Dictionary<string, string>> Data { get; set; }
    }
}