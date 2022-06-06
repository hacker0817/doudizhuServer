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
            new Poker(4,18),new Poker(3,17),

            new Poker(4,3),new Poker(4,4),new Poker(4,5),new Poker(4,6),new Poker(4,7),new Poker(4,8),new Poker(4,9),
            new Poker(4,10),new Poker(4,11),new Poker(4,12),new Poker(4,13),new Poker(4,14),new Poker(4,16),

            new Poker(3,3),new Poker(3,4),new Poker(3,5),new Poker(3,6),new Poker(3,7),new Poker(3,8),new Poker(3,9),
            new Poker(3,10),new Poker(3,11),new Poker(3,12),new Poker(3,13),new Poker(3,14),new Poker(3,16),

            new Poker(2,3),new Poker(2,4),new Poker(2,5),new Poker(2,6),new Poker(2,7),new Poker(2,8),new Poker(2,9),
            new Poker(2,10),new Poker(2,11),new Poker(2,12),new Poker(2,13),new Poker(2,14),new Poker(2,16),

            new Poker(1,3),new Poker(1,4),new Poker(1,5),new Poker(1,6),new Poker(1,7),new Poker(1,8),new Poker(1,9),
            new Poker(1,10),new Poker(1,11),new Poker(1,12),new Poker(1,13),new Poker(1,14),new Poker(1,16),
        };

        public int DealerNum = 1;

        public string GetPlayerByNum(int num)
        {
            foreach (Player p in players)
            {
                if (p.Num == num)
                    return p.UserId;
            }
            return "";
        }

        public string InitGame()
        {
            string guid = Guid.NewGuid().ToString();
            Random random = new Random(guid.GetHashCode());
            DealerNum = random.Next(1, 4);

            #region 发牌
            List<Poker>[] pokers = new List<Poker>[5]
            {
                new List<Poker>(),
                new List<Poker>(),
                new List<Poker>(),
                new List<Poker>(),
                new List<Poker>()
            };
            pokers[0] = basePoker;

            for (int i = 0; i < 51; i++)
            {
                if (DealerNum > 3)
                    DealerNum = 1;

                int r = random.Next(basePoker.Count);
                Poker p = pokers[0][r];
                pokers[DealerNum].Add(p);
                pokers[0].Remove(p);

                DealerNum++;
            }
            #endregion

            JObject result = new JObject();
            result.Add("poker0", JArray.FromObject(pokers[0]));
            result.Add("poker1", JArray.FromObject(pokers[1]));
            result.Add("poker2", JArray.FromObject(pokers[2]));
            result.Add("poker3", JArray.FromObject(pokers[3]));
            result.Add("DealerNum", DealerNum);
            return JsonConvert.SerializeObject(result);
        }
    }
}
