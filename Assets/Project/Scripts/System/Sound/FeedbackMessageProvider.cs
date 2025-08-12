public static class FeedbackMessageProvider
{
    private static string lastMessage;

    public static void SetMessage(string message) => lastMessage = message;

    public static string GetLastMessage() => lastMessage;
}
