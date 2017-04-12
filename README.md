# Customer Loyalty Demo

# What is it?
This repository will provision an environment that may be used as a Lab to build an end to end scenario that does the following:

*	Customer submits a spoken feedback about their latest engagement with support
*	Bing Speech APIs are used to convert speech to text
*	Feedback is submitted to the system
*	Perform sentiment analysis on the feedback and determine if it is negative
*	Legacy API of customer information is queried to retrieve their email
*	Mail them the coupon

# Preparing for the solution

**IMPORTANT:** You need to enable Bing Speech API and Text Analytics API from Cognitive Services on your account first. Have a look here https://docs.microsoft.com/en-us/azure/cognitive-services/cognitive-services-apis-create-account
In short, you need to have created both those APIs from the portal at least once, then deleted them.

# How to install the solution
Hit the Deploy to Azure button to deploy the solution to the subscription you are logged into

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fsabbour%2Fcustomer-loyalty%2Fmaster%2FDeployment%2Fazuredeploy.json" target="_blank">
    <img src="http://azuredeploy.net/deploybutton.png"/>
</a>

This will take roughly 30 minutes as this will provision:

*	An Azure Web App hosting the feedback system
*	An Azure API App hosting the customer API system
*	An Azure Function App with 2 functions (GatherFeedback in C#, GenerateCoupon in nodeJs)
*	An Azure Logic App to be used to orchestrate the workflow
*	Cognitive Services for Text Analytics and Speech To Text
*	An API Management instance (Developer Tier)
*	Storage Account to for Queues and Blob Storage of the generated coupons

# How to run the solution
*	Have a look at the Customer API App that will be deployed at https://customerApi[uniqueId].azurewebsites.net/swagger
	![alt text](Documentation/customerapi.PNG)

*	Update the **customers.json** file with your email. You can edit it online here https://customerApi[uniqueId].scm.azurewebsites.net/dev/wwwroot/App_Data/customers.json
	![alt text](Documentation/customerjson.PNG)

*	Upload **FunctionApp/GenerateCoupon/coupon.jpg** to **coupons/coupon.jpg** in the Blob Storage account

*	Import Customer API App into API Management
	![alt text](Documentation/apim1.PNG)

	![alt text](Documentation/apim2.PNG)

	![alt text](Documentation/apim3.PNG)

	![alt text](Documentation/apim4.PNG)

	![alt text](Documentation/apim5.PNG)

*	Hit up the API Management Developer Portal, go to your Profile and make note of the Unlimited product Subscription Key
	![alt text](Documentation/apim6.PNG)
	
	![alt text](Documentation/apim7.PNG)

*	Check out the functions (GatherFeedback and GenerateCoupon) deployed in the Function App

	![alt text](Documentation/functionapp.PNG)

*	Edit CORS and delete anything other than the * from Allowed Origins

	![alt text](Documentation/cors.PNG)

*	Test the Feedback Web App that will be deployed at https://feedbackWeb[uniqueId].azurewebsites.net	

	![alt text](Documentation/feedbackapp1.PNG)

*	Edit the logic app by adding Queue trigger, then the required actions for Sentiment Analysis, Coupon generation and Email

	![alt text](Documentation/logic1.PNG)
	
	![alt text](Documentation/logic2.PNG)	
	@{Json(triggerBody()?['MessageText']).FeedbackText}
	
	![alt text](Documentation/logic3.PNG)

	![alt text](Documentation/logic4.PNG)

	![alt text](Documentation/logic5.PNG)

	![alt text](Documentation/logic6.PNG)
	@{encodeURIComponent(Json(triggerBody()?['MessageText']).PhoneNumber)}
	
	![alt text](Documentation/logic7.PNG)

	![alt text](Documentation/logic7.1.PNG)
	
	![alt text](Documentation/logic8.PNG)
	"body": {
        "Body": "Here is a discount coupon @{body('GenerateCoupon').CouponUrl}",
        "Subject": "We're sorry you are not satisfied",
        "To": "@{body('Customers_GetCustomerByPhoneNumber')?['Email']}"
    },
	![alt text](Documentation/logic9.PNG)

*	Save the Logic App and go back and add some feedback