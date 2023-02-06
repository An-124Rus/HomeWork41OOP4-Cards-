using System;
using System.Collections.Generic;

namespace HomeWork41OOP4_Cards_
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Crupier crupier = new Crupier();

            crupier.StartNewGame();
        }
    }

    class CardHolder
    {
        protected List<Card> Hand = new List<Card>();

        protected Dictionary<string, int> CardsValue = new Dictionary<string, int>()
        {
            ["2"] = 2,
            ["3"] = 3,
            ["4"] = 4,
            ["5"] = 5,
            ["6"] = 6,
            ["7"] = 7,
            ["8"] = 8,
            ["9"] = 9,
            ["10"] = 10,
            ["J"] = 2,
            ["Q"] = 3,
            ["K"] = 4,
            ["A"] = 11
        };

        public void TakeCard(Card card)
        {
            if (card != null)
                Hand.Add(card);
        }

        public void ShowInfo()
        {
            foreach (Card card in Hand)
            {
                card.ShowInfo();
            }
        }

        public int GetTotalPoints()
        {
            int value = 0;

            foreach (Card card in Hand)
            {
                value += CardsValue[card.Value];
            }

            return value;
        }
    }

    class Card
    {
        public Card(string suit, string value)
        {
            Suit = suit;
            Value = value;
        }

        public string Suit { get; private set; }
        public string Value { get; private set; }

        public void ShowInfo()
        {
            Console.Write($"{Value}{Suit} ");
        }
    }

    class Player : CardHolder
    {
    }

    class Crupier : CardHolder
    {
        private Deck _deck;
        private Player _player;

        public void StartNewGame()
        {
            const string TakeCardCommand = "1";
            const string OpenCardCommand = "2";
            const string StopGameExitCommand = "3";

            bool IsWorking = true;

            Hand = new List<Card>();
            _player = new Player();
            _deck = new Deck();

            while (IsWorking)
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("    Добро пожаловать в игру Двадцать одно    ");
                Console.WriteLine(new string('_', 45));
                Console.WriteLine($"\nВзять карту нажмите ------------- {TakeCardCommand}");
                Console.WriteLine($"\nВскрытие карт нажмите ----------- {OpenCardCommand}");
                Console.WriteLine($"\nЗакончить игру и выйти нажмите -- {StopGameExitCommand}");
                Console.WriteLine(new string('_', 45));

                Console.Write($"\nКарты игрока: ");

                _player.ShowInfo();

                int currentPlayerCount = _player.GetTotalPoints();
                Console.Write($"набрано - {currentPlayerCount} очко/очков");

                Console.Write("\n\nВаш выбор: ");
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case TakeCardCommand:
                        TransferCardToPlayer();
                        break;

                    case OpenCardCommand:
                        RestartGame();
                        break;

                    case StopGameExitCommand:
                        IsWorking = false;
                        break;

                    default:
                        break;
                }
            }
        }

        private void TransferCardToPlayer()
        {
            Card card = _deck.GiveCard();

            _player.TakeCard(card);
        }

        private void RestartGame()
        {
            int totalSelfCount = 0;
            int stepValue = 17;
            int winValue = 21;

            while (totalSelfCount < stepValue)
            {
                Card card = _deck.GiveCard();

                TakeCard(card);

                totalSelfCount = GetTotalPoints();
            }

            int totalPlayerCount = _player.GetTotalPoints();

            ShowResult(totalSelfCount, totalPlayerCount);

            FindWinner(totalSelfCount, winValue, totalPlayerCount);

            Console.WriteLine("\nНажмите любую клавишу");
            Console.ReadKey();

            StartNewGame();
        }

        private void ShowResult(int totatSelfCount, int totalPlayerCount)
        {
            Console.Write($"\nУ игрока карты ");

            _player.ShowInfo();

            Console.Write($"набрали - {totalPlayerCount} очков");
            Console.Write($"\nУ крупье карты ");

            ShowInfo();

            Console.Write($"набрали - {totatSelfCount} очков");
        }

        private void FindWinner(int totalSelfCount, int winValue, int totalPlayerCount)
        {
            if (OnPlayerWon(totalSelfCount, winValue, totalPlayerCount) == true)
                Console.WriteLine("\n\nИгрок выиграл. Поздравляем!");
            else if (OnCrupierWon(totalSelfCount, winValue, totalPlayerCount) == true)
                Console.WriteLine("\n\nКазино выиграло. Сожалеем.");
            else if (OnDraw(totalSelfCount, winValue, totalPlayerCount) == true)
                Console.WriteLine("\n\n");
        }

        private bool OnDraw(int totalSelfCount, int winValue, int totalPlayerCount)
        {
            bool isDraw = true;

            if (totalPlayerCount == totalSelfCount) { }
            else if (totalPlayerCount > winValue && totalSelfCount > winValue) { }
            else
            {
                isDraw = false;
            }

            return isDraw;
        }

        private bool OnCrupierWon(int totalSelfCount, int winValue, int totalPlayerCount)
        {
            bool isCrupierWon = true;

            if (totalPlayerCount < totalSelfCount && totalSelfCount <= winValue) { }
            else if (totalPlayerCount > totalSelfCount && totalPlayerCount > winValue) { }
            else
            {
                isCrupierWon = false;
            }

            return isCrupierWon;
        }

        private bool OnPlayerWon(int totalSelfCount, int winValue, int totalPlayerCount)
        {
            bool isPlayerWon = true;

            if (totalPlayerCount > totalSelfCount && totalPlayerCount <= winValue) { }
            else if (totalPlayerCount < totalSelfCount && totalSelfCount > winValue) { }
            else
            {
                isPlayerWon = false;
            }

            return isPlayerWon;
        }
    }

    class Deck
    {
        private List<Card> _cards = new List<Card>();

        public Deck()
        {
            Create();
            Shuffle();
        }

        public Card GiveCard()
        {
            Card card = null;

            if (_cards.Count > 0)
            {
                card = _cards[0];
                _cards.Remove(card);
            }
            else
            {
                Console.WriteLine("В колоде нет карт");
            }

            return card;
        }

        private void Create()
        {
            string[] suits = new string[] { "♥", "♣", "♠", "♦" };
            string[] values = new string[] { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

            for (int i = 0; i < suits.Length; i++)
            {
                for (int j = 0; j < values.Length; j++)
                {
                    _cards.Add(new Card(suits[i], values[j]));
                }
            }
        }

        private void Shuffle()
        {
            Random random = new Random();

            for (int i = 0; i < _cards.Count; i++)
            {
                int randomIndex = random.Next(_cards.Count);

                Card card = _cards[randomIndex];
                _cards[randomIndex] = _cards[i];
                _cards[i] = card;
            }
        }
    }
}
