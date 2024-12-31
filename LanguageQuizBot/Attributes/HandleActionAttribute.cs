using LanguageQuizBot.Helpers;

namespace LanguageQuizBot.Attributes
{
    public class HandleActionAttribute : HandlerMetadataAttribute
    {
        public HandleActionAttribute(string value) : base(Metatags.HandleAction, value)
        {
        }
    }
}
