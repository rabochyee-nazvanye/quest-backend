using Quest.Domain.Models;

namespace Quest.Application.Services
{
    public class SoloInfiniteConstructorArgs : IQuestConstructorArgs
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string AuthorId { get; set; }
        public BaseResponse<bool> Validate()
        {
            var argsAreValid = !string.IsNullOrWhiteSpace(Name)
                   && !string.IsNullOrWhiteSpace(Description)
                   && !string.IsNullOrWhiteSpace(ImageUrl)
                   && !string.IsNullOrWhiteSpace(AuthorId);

            if (argsAreValid)
                return BaseResponse.Success(true, "Arguments are valid");

            return BaseResponse.Failure<bool>("Arguments are invalid");
        }
    }
}