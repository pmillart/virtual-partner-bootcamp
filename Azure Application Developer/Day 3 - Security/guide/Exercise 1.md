# Challenge Guide
### Overview

You have recently successfully engaged with Contoso Coffee Ltd., a popular supplier of goods for baristas and showcased them a proof-of-concept solution which would both migrate them to the cloud and modernize their backend.
Nonetheless, before getting started it's of paramount importance to ensure proper security policies are in place to prevent malitious users from taking Contoso Coffee's application down.

## Accessing Microsoft Azure

Launch yout favourite browser from the virtual machine desktop and navigate to the URL below. Your Azure Credentials are available by clicking the Cloud Icon at the top of the Lab Player.

```sh
https://portal.azure.com
```

## Challenge 1: Authentication

The microservices architecture based application that you've prepared for Contoso Coffee is designed to handled a burst of activity and also ease ongoing software development effort.
However, until now not enough attention was paid for the security aspects of the application. Undeniably, one of the very first measures you find missing is the lack of an authentication and authorization middleware.

As of right now, all backend endpoints accessible via HTTP(S) are secured exclusively by some API keys which are not centrally-governed. Moreover, in case of bad actors, it is nearly impossible to limit the availability or accessibility of these users.
Therefore, it is your task to secure all your endpoints by leveraging an authentication and authorization mechanism.

In order to complete this task, you're asked to leverage the an Azure Active Directory OAuth 2.0 and Open ID Connect endpoints in order to authenticate clients calling the back-end Function app. 

## Success criteria

Explain to your coach:
- How you address authentication and authorization
- Which authentication flow you're using

### Environment details
In the *resources* folder you will find a the proposed Azure Functions-based backend application.

## Progressing to the next challenge

After you have completed the challenge, click the **Mark complete** button to inform your coach you are ready for review.

## Resources
- https://docs.microsoft.com/en-us/azure/app-service/configure-authentication-provider-aad?toc=/azure/azure-functions/toc.json
- https://docs.microsoft.com/en-us/azure/app-service/app-service-authentication-how-to?toc=/azure/azure-functions/toc.json