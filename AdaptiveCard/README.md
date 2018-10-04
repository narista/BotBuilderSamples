# BotBuilderSamples
I would like to share the altenative for ImBack in AdaptiveCard.

## What is ImBack?
ImBack is the mechanism to send a message back to the bot when user clicked a button on the HeroCard like this picture.
![HeroCard.png](https://qiita-image-store.s3.amazonaws.com/0/245269/55be9e8e-fecc-3a17-d8a3-999080578399.png)

## ImBack in HeroCard
The code is here.

```C#:HeroCardSample.cs
// <copyright file="HeroCardSample.cs" company="Shunji Sumida">
// Copyright (c) Shunji Sumida. All rights reserved.
// </copyright>

namespace Shunji.AdaptiveCard.Cards
{
	using System.Collections.Generic;
	using Microsoft.Bot.Schema;

	/// <summary>
	/// The sample hero card.
	/// </summary>
	public class HeroCardSample : HeroCard
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="HeroCardSample"/> class.
		/// </summary>
		public HeroCardSample()
		{
			this.Title = "Hero Card";
			this.Text = "This is Hero Card. Which card do you want to show?";
			this.Images = new List<CardImage>
            		{
				new CardImage("http://adaptivecards.io/content/cats/2.png"),
			};
			this.Buttons = new List<CardAction>
            		{
				new CardAction(ActionTypes.ImBack, "Adaptive Card", value: "Adaptive"),
				new CardAction(ActionTypes.ImBack, "Hero Card", value: "Hero"),
			};
		}
	}
}
```

You can send the text embedded in ``value`` element when user clicked the button if you set ``ActionTypes.ImBack`` while creating the new instance with ``new CardAction()``

## ImBack in Adaptive Card
On the other hand, you need to set ``Action.Submit`` under ``actions`` element in the description file (JSON) of AdaptiveCard to send a text message to the bot.

```son:AdaptiveCardSample.json
{
  "$schema": "http://adaptivecards.io/schemas/adaptive-card.json",
  "type": "AdaptiveCard",
  "version": "1.0",
  "body": [
    {
      "type": "Image",
      "url": "http://adaptivecards.io/content/cats/2.png",
      "size": "",
      "spacing": "none"
    },
    {
      "type": "TextBlock",
      "text": "Adaptive Card",
      "size": "medium",
      "weight": "bolder"
    },
    {
      "type": "TextBlock",
      "text": "This is Adaptive Card. Which do you want to show?"
    }
  ],
  "actions": [
    {
      "type": "Action.Submit",
      "title": "Adaptive Card",
      "data": "Adaptive!"
    },
    {
      "type": "Action.Submit",
      "title": "HeroCard",
      "data": "Hero"
    }
  ]
}
```

However, it is just similar to the POST with Form of HTML. You need to set ``data`` under ``actions`` to the text that you want to send back to the bot as the input text by user as no message will be sent to the bot if there are no input eleements. After that, you can send the text message to the bot similar to the RichCard like HeroCard.

## Hanling in Bot Application
It is very important point that the AdaptiveCard sends the message as user entry if you set ``data`` element. The bot application will get null if AdaptiveCard deos not have ``data`` element as the message will be set to ```Activity.Text``` when user submit the button.

```C#:EchoWithCounterBot.cs
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
```

This AdaptiveCard send the text "Hero" when user clicked "HeroCard" button.

![AdaptiveCard.png](https://qiita-image-store.s3.amazonaws.com/0/245269/48786d93-a924-e311-f826-9f1e5e887aa6.png)

Called HeroCard.

![HeroCard.png](https://qiita-image-store.s3.amazonaws.com/0/245269/b916651b-4597-d7dc-5021-7b9d8c3dc88e.png)

After that, you can flip-flop the card many times.

## References
[Bot Builder SDK V4 サンプル集](https://github.com/Microsoft/BotBuilder-Samples/tree/master/samples/csharp_dotnetcore)

[Adaptive Card](http://adaptivecards.io/)
