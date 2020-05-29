# Azure Challange - App Modernization with Azure Functions

## Challenge 3: Orchestration and Transactions

Excellent. At this point you have part of your REST APIs authored and your entities running. It is now time to get to the real business: Checking customers' shopping carts out and earning Contoso some income.

Checking out a shopping cart isn't as easy as it sounds though. Whenever a shopping cart is checked out, the shopping cart should be deleted and the products sold should be removed from stock. Clearly, if either of the products are no longer on stock, the checkout operation should fail. Additionally, you might have realized by now that Contoso's domain expert has specified that both the shopping cart and order retrieval don't return the complete information of products. Therefore, in order to allow front-end developers create a usable user interface, you should expose the entire relevant product information via the shopping cart API.

### Environment details
In the *resources* folder you will find all the interfaces as per the domain expert's specifications.

## Progressing to the next challenge

After you have completed the challenge, click the **Mark complete** button to inform your coach you are ready for review.

## Resources
- https://docs.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-entities?tabs=csharp#entity-coordination
