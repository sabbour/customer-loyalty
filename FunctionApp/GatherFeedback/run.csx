using System.Net;

public class Feedback
{
    public string FeedbackText { get; set; }
    public string PhoneNumber { get; set; }
}

public static HttpResponseMessage Run(HttpRequestMessage req, out Feedback outputQueueItem, TraceWriter log)
{
    // Get request body
    dynamic data = req.Content.ReadAsAsync<object>().Result;

    // Set name to query string or body data
    var feedbackText = data?.FeedbackText;
    var phoneNumber = data?.PhoneNumber;

    // Initialize the queue out object
    outputQueueItem = new Feedback { FeedbackText = feedbackText, PhoneNumber = phoneNumber };

    return req.CreateResponse(HttpStatusCode.OK, "Thanks for your feedback.");
}