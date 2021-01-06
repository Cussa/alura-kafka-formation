namespace Ecommerce.Common
{
    public class Topics
    {
        public const string RegexAllTopics = "^ECOMMERCE.*";
        public const string NewOrder = "ECOMMERCE_NEW_ORDER";
        public const string SendEmail = "ECOMMERCE_SEND_EMAIL";
        public const string OrderReject = "ECOMMERCE_ORDER_REJECTED";
        public const string OrderApproved = "ECOMMERCE_ORDER_APPROVED";
        public const string UserGenerateReadingReport = "ECOMMERCE_USER_GENERATE_READING_REPORT";
        public const string SendMessageToAllUsers = "ECOMMERCE_SEND_MESSAGE_TO_ALL_USERS";
        public const string DeadLetter = "ECOMMERCE_DEADLETTER";
    }
}
