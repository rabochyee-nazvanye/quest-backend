using Quest.Domain.Models;

namespace Quest.API.ViewModels.Hints
{
    public class HintVM
    {
        public HintVM(Hint row)
        {
            Secret = row.Secret;
            Number = row.Sorting;
        }
        
        public string Secret { get; set; }
        public int Number { get; set; }
    }
}