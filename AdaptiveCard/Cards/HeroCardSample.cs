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
