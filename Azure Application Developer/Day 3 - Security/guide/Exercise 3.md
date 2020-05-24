# Challenge Guide
### Overview

You have recently successfully engaged with Contoso Coffee Ltd., a popular supplier of goods for baristas and showcased them a proof-of-concept solution which would both migrate them to the cloud and modernize their backend.
Nonetheless, before getting started it's of paramount importance to ensure proper security policies are in place to prevent malitious users from taking Contoso Coffee's application down.

## Accessing Microsoft Azure

Launch yout favourite browser from the virtual machine desktop and navigate to the URL below. Your Azure Credentials are available by clicking the Cloud Icon at the top of the Lab Player.

```sh
https://portal.azure.com
```

## Challenge 3: Access policies

Congratulations, you've met all your customer's requirements! Great job!
Now that a centralized API facade was provisioned in the form of API Management, you need to ensure that neither external partners nor potential malitious users of your website can throttle legitimate users and thus prevent them from using Contoso Coffee's website.
Specifically, each client (Contoso Coffee's customer facing website, partner websites etc.) will have their own unique identification method by being subscribed using API Management's authentication functionalities.
After a thorough consideration with Contoso Coffee's Chief Security Office, you've come to the conclusion that the following polcies are required:
- limit all requests, irrelevant from where they're coming, to 100 requests per 10 seconds and 1000 requests per minute
- limit partners to call the product catalog API only (*/api/store*)

## Success criteria

Explain to your coach:
- how your proposed solution meets the requirements
- and show how you've implemented the solution in a deployable Azure resource

### Environment details
In the *resources* folder you will find a the proposed Azure Functions-based backend application.

## Progressing to the next challenge

After you have completed the challenge, click the **Mark complete** button to inform your coach you are ready for review.

## Resources
- https://docs.microsoft.com/en-us/azure/api-management/api-management-access-restriction-policies#LimitCallRate
- https://docs.microsoft.com/en-us/azure/api-management/api-management-policy-expressions