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

IMPORTANT: You need to enable Bing Speech API and Text Analytics API from Cognitive Services on your account first. Have a look here https://docs.microsoft.com/en-us/azure/cognitive-services/cognitive-services-apis-create-account
In short, you need to have created both those APIs from the portal at least once, then deleted them.

# How to install the solution
Hit the Deploy to Azure button to deploy the solution to the subscription you are logged into
<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fsabbour%2Fcustomer-loyalty%2Fmaster%2FDeployment%2Fazuredeploy.json" target="_blank">
    <img src="http://azuredeploy.net/deploybutton.png"/>
</a>
<a href="http://armviz.io/#/?load=https%3A%2F%2Fraw.githubusercontent.com%2Fsabbour%2Fcustomer-loyalty%2Fmaster%2FDeployment%2Fazuredeploy.json" target="_blank">
    <img src="http://armviz.io/visualizebutton.png"/>
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
*	The Feedback Web App will be deployed at http://feedbackWeb[uniqueId].azurewebsites.net
*	The Customer API App will be deployed at http://customerApi[uniqueId].azurewebsites.net/swagger

# Import the API into API Management
*	Import API App into API Management