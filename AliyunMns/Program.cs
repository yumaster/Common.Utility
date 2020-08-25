using System;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Dybaseapi.Model.V20170525;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using QueryTokenForMnsQueue_MessageTokenDTO = Aliyun.Acs.Dybaseapi.Model.V20170525.QueryTokenForMnsQueueResponse.QueryTokenForMnsQueue_MessageTokenDTO;
using Aliyun.MNS;
using Aliyun.MNS.Model;

namespace AliyunMns
{
    class Program
    {
        static void Main(string[] args)
        {
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", "*********", "********"); // todo: 补充AK信息

            DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", "Dybaseapi", "dybaseapi.aliyuncs.com");

            DefaultAcsClient client = new DefaultAcsClient(profile);

            string queueName = "Alicom-Queue-***********-SmsReport"; // todo: 补充队列名称
            string messageType = "SmsReport"; // todo: 补充消息类型

            int maxThread = 2;

            for (int i = 0; i < maxThread; i++)
            {
                TestTask testTask = new TestTask("PullMessageTask-thread-" + i, messageType, queueName, client);
                Thread t = new Thread(new ThreadStart(testTask.Handle));
                //启动线程
                t.Start();
            }
        }
    }

    class TestTask
    {
        object o = new object();
        const int sleepTime = 50;
        const long bufferTime = 60 * 2; // 过期时间小于2分钟则重新获取，防止服务器时间误差
        const String mnsAccountEndpoint = "https://1943695596114318.mns.cn-hangzhou.aliyuncs.com/"; // 阿里通信消息的endpoint,固定

        public String name { get; private set; }
        public String messageType { get; private set; }
        public String QueueName { get; private set; }
        public int TaskID { get; private set; }
        public IAcsClient AcsClient { get; private set; }

        public TestTask(String name, String messageType, String queueName, IAcsClient acsClient)
        {
            this.name = name;
            this.messageType = messageType;
            this.QueueName = queueName;
            this.AcsClient = acsClient;
        }

        readonly Dictionary<string, QueryTokenForMnsQueue_MessageTokenDTO> tokenMap = new Dictionary<string, QueryTokenForMnsQueue_MessageTokenDTO>();
        readonly Dictionary<string, Queue> queueMap = new Dictionary<string, Queue>();

        public QueryTokenForMnsQueue_MessageTokenDTO GetTokenByMessageType(IAcsClient acsClient, String messageType)
        {
            QueryTokenForMnsQueueRequest request = new QueryTokenForMnsQueueRequest
            {
                MessageType = messageType
            };
            QueryTokenForMnsQueueResponse queryTokenForMnsQueueResponse = acsClient.GetAcsResponse(request);
            QueryTokenForMnsQueue_MessageTokenDTO token = queryTokenForMnsQueueResponse.MessageTokenDTO;
            return token;
        }

        /// 处理消息
        public void Handle()
        {
            while (true)
            {
                try
                {
                    QueryTokenForMnsQueue_MessageTokenDTO token = null;
                    Queue queue = null;
                    lock (o)
                    {
                        if (tokenMap.ContainsKey(messageType))
                        {
                            token = tokenMap[messageType];
                        }

                        if (queueMap.ContainsKey(QueueName))
                        {
                            queue = queueMap[QueueName];
                        }

                        TimeSpan ts = new TimeSpan(0);

                        if (token != null)
                        {
                            DateTime b = Convert.ToDateTime(token.ExpireTime);
                            DateTime c = Convert.ToDateTime(DateTime.Now);
                            ts = b - c;
                        }

                        if (token == null || ts.TotalSeconds < bufferTime || queue == null)
                        {
                            token = GetTokenByMessageType(AcsClient, messageType);
                            IMNS client = new MNSClient(token.AccessKeyId, token.AccessKeySecret, mnsAccountEndpoint, token.SecurityToken);
                            queue = client.GetNativeQueue(QueueName);
                            if (tokenMap.ContainsKey(messageType))
                            {
                                tokenMap.Remove(messageType);
                            }
                            if (queueMap.ContainsKey(QueueName))
                            {
                                queueMap.Remove(QueueName);
                            }
                            tokenMap.Add(messageType, token);
                            queueMap.Add(QueueName, queue);
                        }
                    }

                    BatchReceiveMessageResponse batchReceiveMessageResponse = queue.BatchReceiveMessage(16);
                    List<Message> messages = batchReceiveMessageResponse.Messages;

                    for (int i = 0; i <= messages.Count - 1; i++)
                    {
                        try
                        {
                            byte[] outputb = Convert.FromBase64String(messages[i].Body);
                            string orgStr = Encoding.UTF8.GetString(outputb);
                            Console.WriteLine(orgStr);
                            // TODO 具体消费逻辑,待客户自己实现.
                            // 消费成功的前提下删除消息
                            queue.DeleteMessage(messages[i].ReceiptHandle);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                    }
                }
                catch (Exception e)
                {
                    //Console.WriteLine(e.ToString());
                }
                Thread.Sleep(sleepTime);
            }
        }
    }
}
