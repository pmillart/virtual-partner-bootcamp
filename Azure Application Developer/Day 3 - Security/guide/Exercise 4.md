# Challenge Guide
### Overview

You have recently successfully engaged with Contoso Coffee Ltd., a popular supplier of goods for baristas and showcased them a proof-of-concept solution which would both migrate them to the cloud and modernize their backend.
Nonetheless, before getting started it's of paramount importance to ensure proper security policies are in place to prevent malitious users from taking Contoso Coffee's application down.

## Accessing Microsoft Azure

Launch yout favourite browser from the virtual machine desktop and navigate to the URL below. Your Azure Credentials are available by clicking the Cloud Icon at the top of the Lab Player.

```sh
https://portal.azure.com
```

## Challenge 4: Maintaining secrets

Fantastic job so far!
As you've been working heavily on ensuring the privacy and safety of Contoso Coffee's customers and partners, Contoso's developers started leveraging a third party vendor's API. However, Contoso's Legal Advisor is worried by a phrase in the licensing agreements of the API which says "*callers must take all possible measures from having the API key breached, including but not limited to not storing the API key in source control, limiting the access to the key to high priviledges authorized operational personell only, preventing the pass of the key to Internet-bound middleware software and auditing all uses of the API key*". 
For this challenge, you're asked to propose a solution which would meet the licensing requirements of the third party API.

## Success criteria

Explain to your coach:
- where you are storing the API key
- how is the API key secured at rest
- how you prevent they key from being accessible to developers
- how you prevent they key from being requested or passed over Internet bound endpoints

### Environment details
In the *resources* folder you will find a the proposed Azure Functions-based backend application.

## Progressing to the next challenge

After you have completed the challenge, click the **Mark complete** button to inform your coach you are ready for review.

## Resources
- https://docs.microsoft.com/en-us/azure/key-vault/general/overview
- https://docs.microsoft.com/en-us/azure/private-link/private-link-overview
- https://docs.microsoft.com/en-us/azure/key-vault/general/private-link-service