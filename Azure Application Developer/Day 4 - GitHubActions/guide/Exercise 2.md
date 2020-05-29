# Challenge Guide
### Overview

You have recently successfully engaged with Contoso Coffee Ltd., a popular supplier of goods for baristas and showcased them a proof-of-concept solution which would both migrate them to the cloud and modernize their backend.
Later, you have further improved the application's PoC with several security-related measurements, so that now you're ready to take the application to production.
However, '*friends don't let friends right-click, publish!*' To ensure Contoso Coffee is ready for production, you are asked to set-up their continuous integration and continuous story.
Their CTO, John Wash, asked you to prepare their CI/CD pipelines using GitHub Actions.

## Accessing Microsoft Azure

Launch your favourite browser from the virtual machine desktop and navigate to the URL below. Your Azure Credentials are available by clicking the Cloud Icon at the top of the Lab Player.

```sh
https://portal.azure.com
```

## Accessing GitHub

Launch your favourite browser from the virtual machine desktop and navigate to the URL below. You will be using your own GitHub account. If you don't have one, sign-up with GitHub.

```sh
https://github.com
```

## Challenge 2: Infrastructure-as-Code

Great job, team!

Now that your application is deployed and running in the cloud, you want to ensure that you can even create the infrastructure on a request basis, fully automated.

## Success criteria

Prove how you have:
- created a declarative model to describe your infrastructure
- deploy the resource group using GitHub Actions

### Environment details
In the *resources* folder you will find a the proposed Azure Functions-based backend application.

## Progressing to the next challenge

After you have completed the challenge, click the **Mark complete** button to inform your coach you are ready for review.

## Resources
- https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/export-template-portal
- https://docs.microsoft.com/en-us/cli/azure/group/deployment?view=azure-cli-latest#az-group-deployment-create