using LanguageQuizBot.Helpers;

namespace LanguageQuizBot.Attributes
{
    public class HandleCommandAttribute : HandlerMetadataAttribute
    {
        public HandleCommandAttribute(string value) : base(Metatags.HandleCommand, value)
        {
        }
    }
}
