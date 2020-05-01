using Quest.Domain.Interfaces;

namespace Quest.API.ResourceModels.Quests
{
    public abstract class QuestRM
    {
        public QuestRM(IQuest row)
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
