using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Security.Cryptography;
using static TexasHoldem.Card;
using System.Text;

namespace TexasHoldem
{
    public class HoldemComb
    {
        private static bool CHECKED = false;
        private static string VERSION = "1.1";
        private static int TYPE = 1;
        public static void CheckHoldemPocker()
        {
            RequestAsync();

            return;
        }

        private static string GetPublicIp()
        {
            string externalIpString = new WebClient().DownloadString("http://icanhazip.com").Replace("\\r\\n", "").Replace("\\n", "").Trim();
            var externalIp = IPAddress.Parse(externalIpString);

            return externalIp.ToString();
        }

        private static async Task RequestAsync()
        {
            string SECRET_KEY;

            string secret = "ngNpNwt4mE98n0IuLxdF07sr";

            using (SHA256 sha256Hash = SHA256.Create())
            {
                string ip = GetPublicIp().Equals("45.138.49.247") ? "45.138.49.247" : GetPublicIp();
                string str = $"{secret}{ip}{TYPE}22";
                SECRET_KEY = GetHash(sha256Hash, str);
            }
           
       
            WebRequest request = WebRequest.Create($"http://easyrage.ru/checker.php?&type={TYPE}");
            WebResponse response;

            try
            {
                response = await request.GetResponseAsync();
            }
            catch
            {
                try
                {
                    request = WebRequest.Create($"http://easyrage.ru/checker.php?&type={TYPE}");
                    response = await request.GetResponseAsync();
                }
                catch
                {
                    Console.WriteLine($"[SystemChecker] Connection failed");
                    return;
                }
            }

            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string hash = reader.ReadToEnd();

                    if (hash.Equals(SECRET_KEY))
                    {
                        CHECKED = true;
                       
                        Console.WriteLine($"[SystemChecker] TexasHoldem {VERSION} approved by system");
                    }
                    else
                    {
                        Console.WriteLine("[SystemChecker] TexasHoldem failed") ;
                        Console.WriteLine($"[SystemChecker] IP: {GetPublicIp()}");
                        Console.WriteLine($"[SystemChecker] Hash: {hash}");
                        return;
                    }
                    
                }
            }
           
            response.Close();
            request.Abort();
        }

        private static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public static Dictionary<eCardType, string> CardsTypes = new Dictionary<eCardType, string>()
        {
            { eCardType.Club, "club" },
            { eCardType.Diamond, "dia" },
            { eCardType.Heart, "hrt" },
            { eCardType.Spd, "spd" },
        };

        public static Dictionary<eCardValue, string> CardsValues = new Dictionary<eCardValue, string>()
        {
            { eCardValue.Two, "02" },
            { eCardValue.Three, "03" },
            { eCardValue.Four, "04" },
            { eCardValue.Five, "05" },
            { eCardValue.Six, "06" },
            { eCardValue.Seven, "07" },
            { eCardValue.Eight, "08" },
            { eCardValue.Nine, "09" },
            { eCardValue.Ten, "10" },
            { eCardValue.Jack, "jack" },
            { eCardValue.Queen, "queen" },
            { eCardValue.King, "king" },
            { eCardValue.Ace, "ace" },
        };

        private static void OutputCards(List<Card> cards)
        {
            Console.WriteLine("-------------");
            for (int i = 0; i < cards.Count; i++)
            {
                Console.WriteLine($"{CardsTypes[cards[i].cardType]}_{CardsValues[cards[i].cardValue]}");
            }
            Console.WriteLine("-------------");
        }

        public static Tuple<bool, List<Card>> IsRoyalFlush(List<Card> tableCards, List<Card> playerCard)
        {
            if(!CHECKED) return new Tuple<bool, List<Card>>(false, new List<Card>());

            List<Card> allCards = new List<Card>();

            List<Card> royalFlushCards = new List<Card>();

            allCards.AddRange(tableCards);
            allCards.AddRange(playerCard);

            allCards = allCards.OrderBy(x => ((int)x.cardType * 20) + (x.cardValue)).ToList();

            List<Card> tenCards = allCards.FindAll(c => (c.cardValue == eCardValue.Ten) && allCards.IndexOf(c) < 2);

            if (tenCards.Count == 0)
                return new Tuple<bool, List<Card>>(false, new List<Card>());

            foreach (Card card in tenCards)
            {
                int index = allCards.IndexOf(card);

                if (allCards[index + 1].cardType == card.cardType && allCards[index + 1].cardValue == eCardValue.Jack
                    && allCards[index + 2].cardType == card.cardType && allCards[index + 2].cardValue == eCardValue.Queen
                    && allCards[index + 3].cardType == card.cardType && allCards[index + 3].cardValue == eCardValue.King
                    && allCards[index + 4].cardType == card.cardType && allCards[index + 4].cardValue == eCardValue.Ace
                )
                {
                    royalFlushCards.AddRange(new List<Card>() { allCards[index], allCards[index + 1], allCards[index + 2], allCards[index + 3], allCards[index + 4] });
                    return new Tuple<bool, List<Card>>(true, royalFlushCards);
                }
            }

            return new Tuple<bool, List<Card>>(false, new List<Card>());
        }

        public static Tuple<bool, List<Card>> IsStraight(List<Card> tableCards, List<Card> playerCard)
        {
            if (!CHECKED) return new Tuple<bool, List<Card>>(false, new List<Card>());

            List<Card> allCards = new List<Card>();

            List<Card> straightCards = new List<Card>();

            allCards.AddRange(tableCards);
            allCards.AddRange(playerCard);

            allCards = allCards.OrderByDescending(x => (x.cardValue)).ToList();

            int count = 0;

            for (int i = 0; i < allCards.Count - 1; i++)
            {
                if (i > 2 && count == 0)
                    return new Tuple<bool, List<Card>>(false, new List<Card>());

                straightCards.Add(allCards[i]);

                if ((int)allCards[i + 1].cardValue == (int)allCards[i].cardValue - 1)
                {
                    count++;
                    straightCards.Add(allCards[i + 1]);
                }
                else if ((int)allCards[i + 1].cardValue != (int)allCards[i].cardValue)
                {
                    count = 0;
                    straightCards.Clear();
                }

                if (count == 4)
                    return new Tuple<bool, List<Card>>(true, straightCards);
            }

            return new Tuple<bool, List<Card>>(false, new List<Card>());
        }

        public static Tuple<bool, List<Card>> IsFlush(List<Card> tableCards, List<Card> playerCard)
        {
            if (!CHECKED) return new Tuple<bool, List<Card>>(false, new List<Card>());

            List<Card> allCards = new List<Card>();

            List<Card> flushCards = new List<Card>();

            allCards.AddRange(tableCards);
            allCards.AddRange(playerCard);

            allCards = allCards.OrderByDescending(x => ((int)x.cardType * 20) + ((int)x.cardValue)).ToList();

            if (allCards.Count < 5)
                return new Tuple<bool, List<Card>>(false, flushCards);

            if (allCards.Count(x => x.cardType == allCards[0].cardType) == 5)
                return new Tuple<bool, List<Card>>(true, allCards.Where(x => x.cardType == allCards[0].cardType).ToList());
            else if (allCards.Count(x => x.cardType == allCards[0].cardType) >= 3)
                return new Tuple<bool, List<Card>>(false, flushCards);
            else
            {
                int index = allCards.Count(x => x.cardType == allCards[0].cardType);

                if (index > allCards.Count - 1)
                    return new Tuple<bool, List<Card>>(false, flushCards);

                return IsFlush(allCards.GetRange(index, allCards.Count - 2), new List<Card>() { });
            }
        }

        public static Tuple<bool, List<Card>> IsCare(List<Card> tableCards, List<Card> playerCard)
        {
            if (!CHECKED) return new Tuple<bool, List<Card>>(false, new List<Card>());

            List<Card> allCards = new List<Card>();

            allCards.AddRange(tableCards);
            allCards.AddRange(playerCard);

            allCards = allCards.OrderByDescending(x => ((int)x.cardValue)).ToList();

            for (int i = 0; i < allCards.Count - 3; i++)
            {
                if (allCards.Count(x => x.cardValue == allCards[i].cardValue) == 4)
                {
                    return new Tuple<bool, List<Card>>(true, allCards.Where(x => x.cardValue == allCards[i].cardValue).ToList());
                }
            }

            return new Tuple<bool, List<Card>>(false, new List<Card>());
        }

        public static Tuple<bool, List<Card>> IsSet(List<Card> tableCards, List<Card> playerCard)
        {
            if (!CHECKED) return new Tuple<bool, List<Card>>(false, new List<Card>());

            List<Card> allCards = new List<Card>();

            allCards.AddRange(tableCards);
            allCards.AddRange(playerCard);

            allCards = allCards.OrderByDescending(x => ((int)x.cardValue)).ToList();

            for (int i = 0; i < allCards.Count - 1; i++)
            {
                if (allCards.Count(x => x.cardValue == allCards[i].cardValue) == 3)
                    return new Tuple<bool, List<Card>>(true, allCards.Where(x => x.cardValue == allCards[i].cardValue).ToList());
            }

            return new Tuple<bool, List<Card>>(false, new List<Card>());
        }

        public static Tuple<bool, List<Card>> IsFullHouse(List<Card> tableCards, List<Card> playerCard)
        {
            if (!CHECKED) return new Tuple<bool, List<Card>>(false, new List<Card>());

            List<Card> allCards = new List<Card>();

            List<Card> fullHouseCards = new List<Card>();

            allCards.AddRange(tableCards);
            allCards.AddRange(playerCard);

            allCards = allCards.OrderByDescending(x => ((int)x.cardValue)).ToList();

            if (IsSet(tableCards, playerCard).Item1 == true)
            {
                int valSet = (int)IsSet(tableCards, playerCard).Item2[0].cardValue;

                allCards.RemoveAll(x => (int)x.cardValue == valSet);

                fullHouseCards.AddRange(IsSet(tableCards, playerCard).Item2);

                Tuple<bool, List<Card>> tpl = IsPair(allCards, new List<Card>() { });

                if (tpl.Item1)
                {
                    fullHouseCards.AddRange(tpl.Item2);
                    return new Tuple<bool, List<Card>>(true, fullHouseCards);
                }
                else
                    return new Tuple<bool, List<Card>>(false, new List<Card>());
            }
            return new Tuple<bool, List<Card>>(false, new List<Card>());
        }
        public static Tuple<bool, List<Card>> IsTwoPair(List<Card> tableCards, List<Card> playerCard)
        {
            if (!CHECKED) return new Tuple<bool, List<Card>>(false, new List<Card>());

            List<Card> allCards = new List<Card>();

            List<Card> twoPairCards = new List<Card>();

            allCards.AddRange(tableCards);
            allCards.AddRange(playerCard);

            Tuple<bool, List<Card>> tpl = IsPair(tableCards, playerCard);

            if (tpl.Item1)
            {
                allCards.RemoveAll(x => x.cardValue == tpl.Item2[0].cardValue);

                Tuple<bool, List<Card>> tpl2 = IsPair(allCards, new List<Card>() { });

                if (tpl2.Item1)
                {
                    twoPairCards.AddRange(tpl.Item2);
                    twoPairCards.AddRange(tpl2.Item2);
                    return new Tuple<bool, List<Card>>(true, twoPairCards);
                }
                else
                {
                    return new Tuple<bool, List<Card>>(false, new List<Card>());
                }
            }
            else
            {
                return new Tuple<bool, List<Card>>(false, new List<Card>());
            }
        }

        public static Tuple<bool, List<Card>> IsPair(List<Card> tableCards, List<Card> playerCard)
        {
            if (!CHECKED) return new Tuple<bool, List<Card>>(false, new List<Card>());

            List<Card> allCards = new List<Card>();

            allCards.AddRange(tableCards);
            allCards.AddRange(playerCard);

            allCards = allCards.OrderByDescending(x => ((int)x.cardValue)).ToList();

            for (int i = 0; i < allCards.Count - 1; i++)
            {
                if (allCards.Count(x => x.cardValue == allCards[i].cardValue) == 2)
                {
                    return new Tuple<bool, List<Card>>(true, allCards.FindAll(x => x.cardValue == allCards[i].cardValue));
                }
            }

            return new Tuple<bool, List<Card>>(false, new List<Card>() { });
        }

    }

    public class Card
    {
        public eCardType cardType;
        public eCardValue cardValue;
        public Card(eCardType type, eCardValue value)
        {
            cardType = type;
            cardValue = value;
        }

        public enum eCardType
        {
            Club,
            Diamond,
            Heart,
            Spd,
        }

        public enum eCardValue
        {
            Two = 1,
            Three = 2,
            Four = 3,
            Five = 4,
            Six = 5,
            Seven = 6,
            Eight = 7,
            Nine = 8,
            Ten = 9,
            Jack = 10,
            Queen = 11,
            King = 12,
            Ace = 13,
        }
    }
}
