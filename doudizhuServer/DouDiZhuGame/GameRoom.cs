using doudizhuServer.DouDiZhuGame;
using doudizhuServer.Models;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;

namespace doudizhuServer
{
    public class GameRoom
    {
        public string RoomId { get; set; }
        public string RoomType { get; set; }

        public List<Player> players = new List<Player>();

        public List<Poker> basePoker = new List<Poker>
        {
            new Poker(4,18,1),new Poker(3,17,2),

            new Poker(4,3,3),new Poker(4,4,4),new Poker(4,5,5),new Poker(4,6,6),new Poker(4,7,7),
            new Poker(4,8,8),new Poker(4,9,9),new Poker(4,10,10),new Poker(4,11,11),new Poker(4,12,13),
            new Poker(4,13,13),new Poker(4,14,14),new Poker(4,16,15),

            new Poker(3,3,16),new Poker(3,4,17),new Poker(3,5,18),new Poker(3,6,19),new Poker(3,7,20),
            new Poker(3,8,21),new Poker(3,9,22),new Poker(3,10,23),new Poker(3,11,24),new Poker(3,12,25),
            new Poker(3,13,26),new Poker(3,14,27),new Poker(3,16,28),

            new Poker(2,3,29),new Poker(2,4,30),new Poker(2,5,31),new Poker(2,6,31),new Poker(2,7,31),
            new Poker(2,8,32),new Poker(2,9,33),new Poker(2,10,34),new Poker(2,11,35),new Poker(2,12,36),
            new Poker(2,13,37),new Poker(2,14,38),new Poker(2,16,39),

            new Poker(1,3,40),new Poker(1,4,41),new Poker(1,5,42),new Poker(1,6,43),new Poker(1,7,44),
            new Poker(1,8,45),new Poker(1,9,46),new Poker(1,10,47),new Poker(1,11,48),new Poker(1,12,49),
            new Poker(1,13,50),new Poker(1,14,51),new Poker(1,16,52),
        };

        public int DealerNum = 1;

        public int DealingCount = 0;

        public List<Poker> poker0 = new List<Poker>();//底牌
        public List<Poker> poker1 = new List<Poker>();//1号玩家手牌
        public List<Poker> poker2 = new List<Poker>();//2号玩家手牌
        public List<Poker> poker3 = new List<Poker>();//3号玩家手牌
        public List<Poker> poker4 = new List<Poker>();//最后一手牌

        public bool dealPokerIsOver = false;
        public bool landlordIsSelect = false;
        public bool gameIsOver = false;

        public Player GetPlayerById(string id)
        {
            foreach (Player p in players)
            {
                if (p.UserId == id)
                    return p;
            }
            return null;
        }

        public Player GetPlayerByNum(int num)
        {
            foreach (Player p in players)
            {
                if (p.Num == num)
                    return p;
            }
            return null;
        }

        public void InitGame()
        {
            string guid = Guid.NewGuid().ToString();
            Random random = new Random(guid.GetHashCode());
            DealerNum = random.Next(1, 4);
            poker0 = basePoker;

            //#region 发牌
            //List<Poker>[] pokers = new List<Poker>[5]
            //{
            //    new List<Poker>(),
            //    new List<Poker>(),
            //    new List<Poker>(),
            //    new List<Poker>(),
            //    new List<Poker>()
            //};
            //pokers[0] = basePoker;

            //for (int i = 0; i < 51; i++)
            //{
            //    DealerNum++;

            //    if (DealerNum > 3)
            //        DealerNum = 1;

            //    int r = random.Next(basePoker.Count);
            //    Poker p = pokers[0][r];
            //    pokers[DealerNum].Add(p);
            //    pokers[0].Remove(p);
            //}
            //#endregion

            //JObject result = new JObject();
            //result.Add("poker0", JArray.FromObject(pokers[0]));
            //result.Add("poker1", JArray.FromObject(pokers[1]));
            //result.Add("poker2", JArray.FromObject(pokers[2]));
            //result.Add("poker3", JArray.FromObject(pokers[3]));
            //result.Add("DealerNum", DealerNum);
            //return JsonConvert.SerializeObject(result);
        }

        public int DealPoker()
        {
            if (poker0.Count > 3)
            {
                string guid = Guid.NewGuid().ToString();
                Random random = new Random(guid.GetHashCode());
                int r = random.Next(poker0.Count);
                Poker p = poker0[r];
                poker0.Remove(p);

                DealerNum++;
                if (DealerNum > 3)
                    DealerNum = 1;

                DealingCount++;

                return r;
            }
            else
                return 0;
        }
    }
}
