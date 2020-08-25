﻿using System;
using System.Collections.Generic;
using Vouzamo.Cards.Core;

namespace Vouzamo.Cards.App
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorSize = 24;

            ConsoleKeyInfo key = Console.ReadKey();

            do
            {
                Console.Clear();

                var deck = new Deck();
                deck.Shuffle();

                if (deck.TryDeal(2, out var player1) && deck.TryDeal(2, out var player2))
                {
                    if (deck.TryDeal(3, out var flop) && deck.TryDeal(1, out var turn) && deck.TryDeal(1, out var river))
                    {
                        var communityCards = new List<Card>();
                        communityCards.AddRange(flop);
                        communityCards.AddRange(turn);
                        communityCards.AddRange(river);

                        var player1hand = new List<Card>();
                        player1hand.AddRange(player1);
                        player1hand.AddRange(communityCards);

                        var player2hand = new List<Card>();
                        player2hand.AddRange(player2);
                        player2hand.AddRange(communityCards);

                        Console.WriteLine("Community Cards:");
                        foreach (var card in communityCards)
                        {
                            OutputCard(card);
                        }
                        Console.WriteLine();

                        var player1evaluatedHand = HandEvaluator.Evaluate(player1hand);
                        var player2evaluatedHand = HandEvaluator.Evaluate(player2hand);

                        Console.WriteLine("Player One:");
                        foreach(var card in player1)
                        {
                            OutputCard(card);
                        }
                        Console.WriteLine($"{player1evaluatedHand.Ranking} ({player1evaluatedHand.Score})");

                        Console.WriteLine("Player Two:");
                        foreach (var card in player2)
                        {
                            OutputCard(card);
                        }
                        Console.WriteLine($"{player2evaluatedHand.Ranking} ({player2evaluatedHand.Score})");

                        if(player1evaluatedHand.Score > player2evaluatedHand.Score)
                        {
                            Console.WriteLine("Player 1 wins.");
                        }
                        else if(player1evaluatedHand.Score < player2evaluatedHand.Score)
                        {
                            Console.WriteLine("Player 2 wins.");
                        }
                        else
                        {
                            Console.WriteLine("Tie");
                        }
                        Console.WriteLine();
                    }
                }

                Console.Write("Press ESC to exit or any other key to deal again... ");

                key = Console.ReadKey(true);
            }
            while (key.Key != ConsoleKey.Escape);
        }

        public static void OutputCard(Card card)
        {
            switch(card.Suit)
            {
                case Suits.Spades:
                case Suits.Clubs:
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case Suits.Hearts:
                case Suits.Diamonds:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }

            Console.Write(card);

            Console.ForegroundColor = ConsoleColor.Black;

            Console.Write(" ");
        }
    }
}
