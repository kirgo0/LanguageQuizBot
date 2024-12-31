using Autofac;
using LanguageQuizBot.Attributes;
using LanguageQuizBot.UpdateHandlers.Abstractions;
using System.Reflection;
using Telegram.Bot.Polling;

namespace LanguageQuizBot.Modules
{
    public class HandlersModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DefaultUpdateHandler>();

            var assembly = Assembly.GetExecutingAssembly();

            var handlerTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.GetCustomAttributes<HandlerMetadataAttribute>().Any());

            foreach (var handlerType in handlerTypes)
            {
                var metadata = handlerType.GetCustomAttribute<HandlerMetadataAttribute>();
                builder.RegisterType(handlerType)
                    .As<UpdateHandler>()
                    .WithMetadata(metadata.Key, metadata.Value);
            }

        }
    }

}
