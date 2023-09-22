namespace EventsConsumer.Models.Body;

public class SendEmailBody
{
    public SendEmailBody(string content, string subject)
    {
        Content = content;
        Subject = subject;
    }

    public string Content { get; set; }
    public string Subject { get; set; }

}