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

## Challenge 3: Deployment rings

Excellent! John is extremely excited about all your effort so far. After talking to the development team, they've set up GitFlow and a number of branches in their repository.
Specifically, they will only have the completely functional code running on the 'master' branch. In addition to that, they've decided they'd use a development branch for Continuous Delivery to dev/test environments. Developers will work in their own branches and/or feature branches and will open up pull requests whenever they complete a feature, to the development branch. 
Once all the tests are done and no further development is required, a new PR will be opened which will allow the application to get deployed to a pre-production slot. Closing the PR also means that the code in master will be the code running in production.

Since both your infrastructure and your application can be deployed fully automated, your *almost* in a prestine DevOps state.
## Success criteria

Prove how you have:
- created the branches and applied all the necessary policies to them
- use pull-requests to deploy ephemeral infrastructures in Azure
- deploy the code to a deployment slot before it is actually pushed to production

### Environment details
In the *resources* folder you will find a the proposed Azure Functions-based backend application.

## Progressing to the next challenge

After you have completed the challenge, click the **Mark complete** button to inform your coach you are ready for review.

## Resources
- TODO