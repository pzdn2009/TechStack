namespace SigmalHex.Data
{
    /// <summary>
    /// Reponse data.
    /// </summary>
    public class ResponseData
    {
        /// <summary>
        /// When the code's value equals zero, that is success.
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// Description for the code.
        /// </summary>
        public string description { get; set; }
    }
}
