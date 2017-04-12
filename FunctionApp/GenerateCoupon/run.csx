#r "Microsoft.WindowsAzure.Storage"

using System.Net;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Microsoft.WindowsAzure.Storage.Blob;

public static HttpResponseMessage Run(HttpRequestMessage req, CloudBlockBlob inputCoupon, CloudBlockBlob outputBlob, TraceWriter log)
{
    // Get request body
    dynamic data = req.Content.ReadAsAsync<object>().Result;

    // Set name to body data
    string name = data?.CustomerName;
    
    log.Info("Received coupon request for: " + name);
    
    // Generate Coupon Id
    Random _rdm = new Random();
    var randomCode = _rdm.Next(1000,9999);
    
    // Get Coupon image and write new coupon
    using (Stream inputMemoryStream = new MemoryStream())
    using (Stream outputMemoryStream = new MemoryStream()){
    
        // Get the coupon image
        inputCoupon.DownloadToStream(inputMemoryStream);
        inputMemoryStream.Position = 0;
        
        // Write the text
        WriteWatermark("25% off voucher\n" + name +"\n" + randomCode, inputMemoryStream, outputMemoryStream,log);
        
        // Write to blob
        outputMemoryStream.Position = 0;
        outputBlob.UploadFromStream(outputMemoryStream);
        
        outputBlob.Properties.ContentType = "image/jpeg";
        outputBlob.SetProperties();
    }
    
    // Return
    return req.CreateResponse(HttpStatusCode.OK, new { CouponUrl = GetBlobSasUri(outputBlob) });
}

private static void WriteWatermark(string watermarkContent, Stream originalImage, Stream newImage, TraceWriter log)
{
    log.Info("Reading image");
    using (Image inputImage = Image.FromStream(originalImage, true))
    {
     log.Info("Creating Graphics");
        using (Graphics graphic = Graphics.FromImage(inputImage))
        {
            graphic.SmoothingMode = SmoothingMode.HighQuality;
            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
            
            log.Info("Drawing text");
            graphic.DrawString(watermarkContent, new Font("Tahoma", 18, FontStyle.Bold), Brushes.Black, 50, 150);
            graphic.Flush();
        
            log.Info("Writing to the output stream");
            inputImage.Save(newImage, ImageFormat.Jpeg);
        }
    }
}

private static string GetBlobSasUri(CloudBlockBlob blob)
{
    //Set the expiry time and permissions for the blob.
    //In this case the start time is specified as a few minutes in the past, to mitigate clock skew.
    //The shared access signature will be valid immediately.
    SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy();
    sasConstraints.SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-5);
    sasConstraints.SharedAccessExpiryTime = DateTime.UtcNow.AddHours(24);
    sasConstraints.Permissions = SharedAccessBlobPermissions.Read;

    //Generate the shared access signature on the blob, setting the constraints directly on the signature.
    string sasBlobToken = blob.GetSharedAccessSignature(sasConstraints);

    //Return the URI string for the container, including the SAS token.
    return blob.Uri + sasBlobToken;
}