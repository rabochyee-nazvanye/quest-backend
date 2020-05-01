using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quest.Domain.Interfaces;
using Quest.Domain.Models;

namespace Quest.API.ViewModels.Quests
{
    public abstract class QuestDTO
    {
        public QuestDTO(IQuest row)
        {
            Id = row.Id;
            Name = row.Name;
            Description = row.Description;
            ImageUrl = row.ImageUrl;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        protected abstract string GetQuestType();
        public string Type => GetQuestType();
        protected abstract bool CheckIsInfinite();
        public bool IsInfinite => CheckIsInfinite();
    }
}
