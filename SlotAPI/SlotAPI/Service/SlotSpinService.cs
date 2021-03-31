using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SlotAPI.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using SlotAPI.Configuration;
using System.Collections;
using System.Security.Cryptography;



namespace SlotAPI.Service
{
    public class SlotSpinService
    {
        private MongoClient mongoClient;
        private IMongoDatabase database;
        private SlotMongoDbSettings settings;
        private UserService userService;
        

        public SlotSpinService(SlotMongoDbSettings settings,UserService userService)
        {
            this.settings = settings;
            this.userService = userService;
            mongoClient = new MongoClient(settings.ConnectionString);
            database = mongoClient.GetDatabase(settings.DatabaseName);

        }
        /// <summary>
        /// Handle the request of api
        /// </summary>
        /// <param name="slotSpinReq"></param>
        /// <returns></returns>
        public SlotSpinResponse HandleRequest(SlotSpinReq slotSpinReq)
        {
            SlotSpinResponse slotSpinResponse = new SlotSpinResponse();

            User user = userService.GetUser(slotSpinReq.Uid);
            int size = GetSlotQueueSize();
            slotSpinResponse.Results = new int[size];
            
            //create random result
            randomResult(slotSpinResponse);

            //calcute the result
            calcResult(slotSpinResponse);
            user.Balance -= slotSpinReq.Bet;
            user.Balance += slotSpinResponse.WonAmount;

            //save results
            userService.UpdateUser(slotSpinReq.Uid, user);

            return slotSpinResponse;
        }
        /// <summary>
        /// Get the slot spin size
        /// </summary>
        /// <returns></returns>
        public int GetSlotQueueSize()
        {
            SlotConfiguration size = database.GetCollection<SlotConfiguration>("SlotConfiguration").Find(c=>c.ItemName=="SlotSize").FirstOrDefault();
            if (size == null)
            {
                SetSlotQueueSize(settings.DefaultSlotSize);
                return settings.DefaultSlotSize;
            }
            else
            {
                return int.Parse(size.ItemValue);
            }
        }
        /// <summary>
        /// Setup the slot size
        /// </summary>
        /// <param name="size"></param>
        public void SetSlotQueueSize(int size)
        {
            SlotConfiguration slotConfiguration = new SlotConfiguration();
            slotConfiguration.ItemName = "SlotSize";
            slotConfiguration.ItemValue = size.ToString();
            database.GetCollection<SlotConfiguration>("SlotConfiguration").ReplaceOne(c => c.ItemName == "SlotSize", slotConfiguration);
        }

        private void randomResult (SlotSpinResponse slotSpinResponse)
        {
            //not really random.
            RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
            byte[] randomNumber = new byte[slotSpinResponse.Results.Length];
            randomNumberGenerator.GetBytes(randomNumber);
            slotSpinResponse.Results = randomNumber.Select(p => p % 10).ToArray<int>();

        }

        private void calcResult(SlotSpinResponse slotSpinResponse)
        {
            
            int maxValue = 0;
            for (int i = 0; i < slotSpinResponse.Results.Length; )
            {
                int j = i + 1;
                while (j < slotSpinResponse.Results.Length)
                {
                    if (slotSpinResponse.Results[i] == slotSpinResponse.Results[j])
                    {
                        maxValue = Math.Max(maxValue, (j - i + 1) * slotSpinResponse.Results[i]);
                        ++j;
                    }
                    else
                    {
                        i = j;
                        ++j;
                    }
                }
                
            }
        }

    }
}
