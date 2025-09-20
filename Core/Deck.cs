using System;
using CrazyRisk.DataStructures;

namespace CrazyRisk.Core
{
    public class Deck
    {
        private Queue<Card> cards;
        
        public Deck()
        {
            cards = new Queue<Card>();
            InitializeDeck();
        }
        
        private void InitializeDeck()
        {
            // Crear cartas para cada tipo
            for (int i = 0; i < 14; i++)
            {
                cards.Enqueue(new Card { Type = CardType.Infantry });
            }
            for (int i = 0; i < 14; i++)
            {
                cards.Enqueue(new Card { Type = CardType.Cavalry });
            }
            for (int i = 0; i < 14; i++)
            {
                cards.Enqueue(new Card { Type = CardType.Artillery });
            }
            
            Shuffle();
        }
        
        public Card DrawCard()
        {
            if (cards.IsEmpty)
            {
                InitializeDeck();
            }
            return cards.Dequeue();
        }
        
        public void Shuffle()
        {
            Card[] cardArray = new Card[cards.Count];
            int index = 0;
            
            while (!cards.IsEmpty)
            {
                cardArray[index++] = cards.Dequeue();
            }
            
            Random random = new Random();
            for (int i = cardArray.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                Card temp = cardArray[i];
                cardArray[i] = cardArray[j];
                cardArray[j] = temp;
            }
            
            foreach (Card card in cardArray)
            {
                cards.Enqueue(card);
            }
        }
    }
}