# Challenge Guide
### Overview

Contoso Coffee Ltd., a popular supplier of goods for baristas has decided that it's time to move their 15 years old e-commerce application to the cloud. Nonetheless, their CTO, John Wash, wants to leverage their cloud migration as an opportunity to modernize their core business logic backend as well. Specifically, they've looked at how they could take advantage of Azure Functions to achieve a higher degree scalability whenever their application is under high load.
That said, whilst technically they could certainly implement the greatest and latest services to achieve the craziest of scalabilities, their current customer portofolio and market share dosn't validate the use of many specific and expensive services. Sure enough, they are looking to minize their operational Azure costs doing the same.

## Accessing Microsoft Azure

Launch Chrome from the virtual machine desktop and navigate to the URL below. Your Azure Credentials are available by clicking the Cloud Icon at the top of the Lab Player.

```sh
https://portal.azure.com
```

## Challenge 2: Stateful in the serverless world

Great, you're now ready to write the business logic for Contoso's e-commerce app.
Considering the key aspect of keeping the costs down, you realize that many of the domains are in nature stateful. Specifically, they need to persist data which is accessible at any time.
One undeniable option would be to rely on data services, but that would also require these services to handled load, be scalable, be further secured, enforce authentication and authorization of sorts... Moreover, it would not keep the cost down.
After a thorough analysis of the current traffic and storage of shopping cart, product catalog and order data, you agree with the customer on using Durable Entities for leveraging these functionalities.
By the end of the challenge you should:
- store the data for your product catalog, as well as the inventory of each product in a singleton Durable Entity
- persist the shopping cart data of each customer in a Durable Entity, contextually designed to maintain an instance of the entity for each customer. It's worth pointing out that a customer can only have a single shopping cart at a time.
- store all orders in a Durable Entity, also contextually designed to maintain an instance of the entity for each customer. Thus, an order entity instance would maintain all the customer's orders
All these stateful stores should be accessible via the REST APIs previously authored.

### Environment details
In the *resources* folder you will find all the interfaces as per the domain expert's specifications.

## Progressing to the next challenge

After you have completed the challenge, click the **Mark complete** button to inform your coach you are ready for review.

## Resources
- https://docs.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-entities?tabs=csharp#define-entities
- https://docs.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-dotnet-entities#accessing-entities-through-interfaces
- https://github.com/Azure/azure-functions-durable-extension/tree/dev/samples