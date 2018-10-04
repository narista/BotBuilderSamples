// <copyright file="EchoWithCounterBot.cs" company="Shunji Sumida">
// Copyright (c) Shunji Sumida. All rights reserved.
// </copyright>

namespace Shunji.AdaptiveCard
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Schema;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Shunji.AdaptiveCard.Cards;

	/// <summary>
	/// Represents a bot that processes incoming activities.
	/// For each user interaction, an instance of this class is created and the OnTurnAsync method is called.
	/// This is a Transient lifetime service.  Transient lifetime services are created
	/// each time they're requested. For each Activity received, a new instance of this
	/// class is created. Objects that are expensive to construct, or have a lifetime
	/// beyond the single turn, should be carefully managed.
	/// For example, the <see cref="MemoryStorage"/> object and associated
	/// <see cref="IStatePropertyAccessor{T}"/> object are created with a singleton lifetime.
	/// </summary>
	/// <seealso ref="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.1"/>
    public class EchoWithCounterBot : IBot
	{
		private readonly EchoBotAccessors accessors;
		private readonly ILogger logger;

		/// <summary>
		/// Initializes a new instance of the <see cref="EchoWithCounterBot"/> class.
		/// </summary>
		/// <param name="accessors">A class containing <see cref="IStatePropertyAccessor{T}"/> used to manage state.</param>
		/// <param name="loggerFactory">A <see cref="ILoggerFactory"/> that is hooked to the Azure App Service provider.</param>
		/// <seealso ref="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-2.1#windows-eventlog-provider"/>
		public EchoWithCounterBot(EchoBotAccessors accessors, ILoggerFactory loggerFactory)
		{
			if (loggerFactory == null)
			{
				throw new System.ArgumentNullException(nameof(loggerFactory));
			}

			this.logger = loggerFactory.CreateLogger<EchoWithCounterBot>();
			this.logger.LogTrace("EchoBot turn start.");
			this.accessors = accessors ?? throw new System.ArgumentNullException(nameof(accessors));
		}

		/// <summary>
		/// Every conversation turn for our Echo Bot will call this method.
		/// There are no dialogs used, since it's "single turn" processing, meaning a single
		/// request and response.
		/// </summary>
		/// <param name="context">A <see cref="ITurnContext"/> containing all the data needed
		/// for processing this conversation turn. </param>
		/// <param name="token">(Optional) A <see cref="CancellationToken"/> that can be used by other objects
		/// or threads to receive notice of cancellation.</param>
		/// <returns>A <see cref="Task"/> that represents the work queued to execute.</returns>
		/// <seealso cref="BotStateSet"/>
		/// <seealso cref="ConversationState"/>
		/// <seealso cref="IMiddleware"/>
		public async Task OnTurnAsync(ITurnContext context, CancellationToken token = default(CancellationToken))
		{
			if (context.Activity.Type == ActivityTypes.Message)
			{
				if (!string.IsNullOrWhiteSpace(context.Activity.Text))
				{
					Activity reply = context.Activity.CreateReply();
					reply.Attachments = new List<Attachment>();
					switch (context.Activity.Text)
					{
						case "Adaptive":
							reply.Attachments.Add(this.CreateAdaptiveCardAttachment());
							break;
						case "Hero":
							reply.Attachments.Add(new HeroCardSample().ToAttachment());
							break;
						default:
							break;
					}

					await context.SendActivityAsync(reply);
				}
			}
		}

		private Attachment CreateAdaptiveCardAttachment()
		{
			var adaptiveCardJson = File.ReadAllText(@".\Cards\AdaptiveCardSample.json");
			var attachement = new Attachment()
			{
				ContentType = "application/vnd.microsoft.card.adaptive",
				Content = JsonConvert.DeserializeObject(adaptiveCardJson),
			};
			return attachement;
		}
	}
}
