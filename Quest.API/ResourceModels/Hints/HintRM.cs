using Quest.Domain.Models;

namespace Quest.API.ResourceModels.Hints
{
    public class HintRM
    {
        public HintRM(Hint row)
        {
            Secret = row.Secret;
            Number = row.Sorting;
        }
        
        public string Secret { get; set; }
        public int Number { get; set; }
    }
}