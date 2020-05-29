# Azure Challange - App Modernization with Azure Functions

## Overview

Contoso Coffee Ltd., a popular supplier of goods for baristas has decided that it's time to move their 15 years old e-commerce application to the cloud. Nonetheless, their CTO, John Wash, wants to leverage their cloud migration as an opportunity to modernize their core business logic backend as well. Specifically, they've looked at how they could take advantage of Azure Functions to achieve a higher degree scalability whenever their application is under high load.

That said, whilst technically they could certainly implement the greatest and latest services to achieve the craziest of scalabilities, their current customer portfolio and market share doesn't validate the use of many specific and expensive services. Sure enough, they are looking to minimize their operational Azure costs doing the same.

## Accessing Microsoft Azure

Launch Chrome from the virtual machine desktop and navigate to the URL below. Your Azure Credentials are available by clicking the Cloud Icon at the top of the Lab Player.

```sh
https://portal.azure.com
```

## Challenge 1: Create Functions

Your customer's domain expert has made your life easy and exposed all the domain capabilities in a set of interfaces. Additionally, the customer's lead architect has also suggested that they are expecting to see these endpoints in their modernized application.
By the end of this challenge, you should have at least a single Function App project created in your favorite IDE which exposes the endpoints as per the architect's requirements:

- InventoryAddItemStockPut: [PUT] http://localhost:7071/api/store/inventory/{itemId}
- InventoryCreateItemPost: [POST] http://localhost:7071/api/store/inventory
- OrderCheckoutPost: [POST] http://localhost:7071/api/user/{userId}/order/checkout
- OrderGet: [GET] http://localhost:7071/api/user/{userId}/order/{orderId}
- ShoppingCartDelete: [DELETE] http://localhost:7071/api/user/{userId}/shoppingCart/{itemId}
- ShoppingCartGet: [GET] http://localhost:7071/api/user/{userId}/shoppingCart
- ShoppingCartPost: [POST] http://localhost:7071/api/user/{userId}/shoppingCart/{itemId}
- StoreGet: [GET] http://localhost:7071/api/store

At this time there are no particular requirements for the logic of these endpoints.

## Environment details

In the *resources* folder you will find all the interfaces as per the domain expert's specifications.

## Progressing to the next challenge

After you have completed the challenge, click the **Mark complete** button to inform your coach you are ready for review.

## Resources
- https://docs.microsoft.com/en-us/azure/azure-functions/functions-overview
- https://docs.microsoft.com/en-us/azure/azure-functions/functions-create-first-function-vs-code?pivots=programming-language-csharp