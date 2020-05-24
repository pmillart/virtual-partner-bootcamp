# Challenge Guide
### Overview

Contoso Coffee Ltd., a popular supplier of goods for baristas has decided that it's time to move their 15 years old e-commerce application to the cloud. Nonetheless, their CTO, John Wash, wants to leverage their cloud migration as an opportunity to modernize their core business logic backend as well. Specifically, they've looked at how they could take advantage of Azure Functions to achieve a higher degree scalability whenever their application is under high load.
That said, whilst technically they could certainly implement the greatest and latest services to achieve the craziest of scalabilities, their current customer portofolio and market share dosn't validate the use of many specific and expensive services. Sure enough, they are looking to minize their operational Azure costs doing the same.

## Accessing Microsoft Azure

Launch Chrome from the virtual machine desktop and navigate to the URL below. Your Azure Credentials are available by clicking the Cloud Icon at the top of the Lab Player.

```sh
https://portal.azure.com
```

## Challenge 3: Orchestration and transactions

Excellent. At this point you have part of your REST APIs authored and your entities running.
It is now time to get to the real business: checking customers' shopping carts out and earning Contoso some income.
Checking out a shopping cart isn't as easy as it sounds though. Whenever a shopping cart is checked out, the shopping cart should be deleted and the products sold should be removed from stock. Clearly, if either of the products are no longer on stock, the checkout operation should fail.

Additionally, you might have realized by now that Contoso's domain expert has specified that both the shopping cart and order retrieval don't return the complete information of products.
Therefore, in order to allow front-end developers create a usable user interface, you should expose the entire relevant product information via the shopping cart API.

### Environment details
In the *resources* folder you will find all the interfaces as per the domain expert's specifications.

## Progressing to the next challenge

After you have completed the challenge, click the **Mark complete** button to inform your coach you are ready for review.

## Resources
- https://docs.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-entities?tabs=csharp#entity-coordination