//var Jimp = require("jimp");
//var path = require("path");
//var azure = require('azure-storage');

module.exports = function (context, req) {

    context.log('Function running');

    try {
        var customerName = req.body.CustomerName;

        // Log incoming request:
        context.log('Got a request to create coupon for customer: ' + customerName);


        // Determine where base image is:
        var baseImgPath = path.resolve(__dirname, 'coupon.jpg');
        context.log('Base image path: ' + baseImgPath);

        /*
        // Read image with Jimp
        Jimp.read(baseImgPath).then((image) => {
            context.log('Got base coupon to start from');

            Jimp.loadFont(Jimp.FONT_SANS_32_BLACK).then(function (font) {
                context.log('Loaded font for writing on image.');

                // Write discount on image:
                image.print(font, 50, 50, "25% Discount");

                Jimp.loadFont(Jimp.FONT_SANS_8_BLACK).then(function (font) {
                    // Write customerName on image:
                    image.print(font, 50, 150, customerName);

                    // Manipulate image
                    image.getBuffer(Jimp.MIME_JPEG, (error, stream) => {
                        context.log(`Successfully processed the image`);

                        var blobLocation = generateSasToken(context, 'coupons', AttemptId + '.jpg', 'r').uri;
                        context.log('Coupons to be stored here: ' + blobLocation);

                        // Return url to storage:
                        context.res = {
                            body: { couponUrl: blobLocation }
                        };

                        // Bind the stream to the output binding to create a new blob
                        context.done(null, { outputBlob: stream });
                    });
                });

                // Check for errors
                if (error) {
                    context.log(`There was an error processing the image.`);
                    context.done(error);
                }
            });
            
        });*/
        
    } catch (e) {
        context.log(e.message);
    }
};

function generateSasToken(context, container, blobName, permissions) {
    var connString = process.env.AzureWebJobsStorage;
    var blobService = azure.createBlobService(connString);

    // Create a SAS token that expires in an hour
    // Set start time to five minutes ago to avoid clock skew.
    var startDate = new Date();
    startDate.setMinutes(startDate.getMinutes() - 5);
    var expiryDate = new Date(startDate);
    expiryDate.setMinutes(startDate.getMinutes() + 60);

    permissions = permissions || azure.BlobUtilities.SharedAccessPermissions.READ;

    var sharedAccessPolicy = {
        AccessPolicy: {
            Permissions: permissions,
            Start: startDate,
            Expiry: expiryDate
        }
    };

    var sasToken = blobService.generateSharedAccessSignature(container, blobName, sharedAccessPolicy);

    return {
        token: sasToken,
        uri: blobService.getUrl(container, blobName, sasToken, true)
    };
}
