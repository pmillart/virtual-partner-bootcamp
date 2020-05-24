# Challenge Guide
### Overview

You have recently successfully engaged with Contoso Coffee Ltd., a popular supplier of goods for baristas and showcased them a proof-of-concept solution which would both migrate them to the cloud and modernize their backend.
Nonetheless, before getting started it's of paramount importance to ensure proper security policies are in place to prevent malitious users from taking Contoso Coffee's application down.

## Accessing Microsoft Azure

Launch yout favourite browser from the virtual machine desktop and navigate to the URL below. Your Azure Credentials are available by clicking the Cloud Icon at the top of the Lab Player.

```sh
https://portal.azure.com
```

## Challenge 2: API Versioning

Even though versioning is not directly seen as a security-related problem, it is your job to ensure the smooth update and rollback of Contoso Coffee's backend and frontend applications. 
Therefore, you were made responsible with the proposal of a solution to maintain the APIs backwards compatibility by allowing several API versions to run concurently, whilst also ensuring a clean codebase. Specifically, whenever the team decides to no longer support an older API version, they want the ability of just pulling the old version without having to recompile or redeploy the entire code again. This also means that the dev team's 'master' branch should at all times only contain the latest version of deployed code. Should a patch be necessary on an older API version, the team will have to decide whether to stop supporting the old version or apply the patch and rebase the change to the newer versions too.
Additionally, since most API updates offer performance improvements, you're asked to never run on the same underlying infrastructure more than a single API version.
Last but not least, Contoso Coffee's Chief Business Officer is interested in creating a limited private preview offering for select partners so that Contoso Coffee's merchandise can reach markets outside the UK. In order to achieve that, she learnt from the offshore partners that they would want to consume your Contoso Coffee's product catalog and inventory by calling your APIs directly and thus have the ability of exposing stock data on their own website in near real-time.

## Success criteria

Explain to your coach:
- which solution you found and show the deployed solution which solves the versioning requirements

### Environment details
In the *resources* folder you will find a the proposed Azure Functions-based backend application.

## Progressing to the next challenge

After you have completed the challenge, click the **Mark complete** button to inform your coach you are ready for review.

## Resources
- https://docs.microsoft.com/en-us/azure/api-management/api-management-key-concepts
- https://docs.microsoft.com/en-us/azure/api-management/import-function-app-as-api