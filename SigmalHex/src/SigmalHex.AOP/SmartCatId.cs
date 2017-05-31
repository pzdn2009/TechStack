using System.Threading;

namespace SigmalHex.AOP
{
    /// <summary>
    /// 
    /// </summary>
    public class SmartCatId
    {
        private static object messageIDLockObject = new object();
        private ThreadLocal<string> local = new ThreadLocal<string>(() => { return string.Empty; });

        public const string CAT_ID = "CAT_ID";
        public const string WCF_NS = "http://tempuri.org";

        public SmartCatId()
        {
            CatKey = new CatKey();
        }

        public SmartCatId(string traceId)
        {
            TraceId = traceId;
            CatKey = new CatKey();
        }

        /// <summary>
        /// 跟蹤號
        /// </summary>
        public string TraceId { get; set; }

        /// <summary>
        /// 跟蹤鍵
        /// </summary>
        public CatKey CatKey { get; set; }

        /// <summary>
        /// 調用層次數
        /// </summary>
        public int InterceptorCount { get; set; }

        /// <summary>
        /// HttpContextHSID
        /// </summary>
        public int HttpContextHSID
        {
            get
            {
                //if (HttpContext.Current != null)
                //{
                //    return HttpContext.Current.GetHashCode();
                //}

                return -1;
            }
        }

        public static SmartCatId Init()
        {
            var catId = GetFromWebApiCall();
            //if (catId == null)
            //{
            //    catId = new SmartCatId(Guid.NewGuid().ToString());
            //}

            //HttpContext.Current.Items[CAT_ID] = catId;

            return catId;
        }

        /// <summary>
        /// 從當前線程獲取
        /// </summary>
        /// <returns></returns>
        public static SmartCatId GetFromCurrentCall()
        {
            SmartCatId messageID = null;

            //var contain = HttpContext.Current.Items.Contains(CAT_ID);
            //if (contain)
            //{
            //    messageID = HttpContext.Current.Items[CAT_ID] as SmartCatId;
            //}

            return messageID;
        }

        /// <summary>
        /// 從WebApi讀取
        /// </summary>
        /// <returns></returns>
        public static SmartCatId GetFromWebApiCall()
        {
            SmartCatId messageID = null;

            //var msgString = HttpContext.Current.Request.Headers[CAT_ID];
            //if (!string.IsNullOrEmpty(msgString))
            //{
            //    messageID = msgString.FromJson<SmartCatId>();
            //}

            return messageID;
        }

        private void Update()
        {
            //HttpContext.Current.Items[CAT_ID] = this;
        }

        public void IncreaseCall()
        {
            this.InterceptorCount++;
            lock (messageIDLockObject)
            {
                Update();
            }
        }
    }
}
