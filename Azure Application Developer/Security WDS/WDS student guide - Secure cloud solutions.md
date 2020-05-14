![Microsoft Cloud Workshops](https://github.com/Microsoft/MCW-Template-Cloud-Workshop/raw/master/Media/ms-cloud-workshop.png "Microsoft Cloud Workshops")

<div class="MCWHeader1">
Secure cloud solution
</div>

<div class="MCWHeader2">
Whiteboard design session student guide
</div>

<div class="MCWHeader3">
May 2020
</div>

Information in this document, including URL and other Internet Web site references, is subject to change without notice. Unless otherwise noted, the example companies, organizations, products, domain names, e-mail addresses, logos, people, places, and events depicted herein are fictitious, and no association with any real company, organization, product, domain name, e-mail address, logo, person, place or event is intended or should be inferred. Complying with all applicable copyright laws is the responsibility of the user. Without limiting the rights under copyright, no part of this document may be reproduced, stored in or introduced into a retrieval system, or transmitted in any form or by any means (electronic, mechanical, photocopying, recording, or otherwise), or for any purpose, without the express written permission of Microsoft Corporation.

Microsoft may have patents, patent applications, trademarks, copyrights, or other intellectual property rights covering subject matter in this document. Except as expressly provided in any written license agreement from Microsoft, the furnishing of this document does not give you any license to these patents, trademarks, copyrights, or other intellectual property.

The names of manufacturers, products, or URLs are provided for informational purposes only and Microsoft makes no representations and warranties, either expressed, implied, or statutory, regarding these manufacturers or the use of the products with any Microsoft technologies. The inclusion of a manufacturer or product does not imply endorsement of Microsoft of the manufacturer or product. Links may be provided to third party sites. Such sites are not under the control of Microsoft and Microsoft is not responsible for the contents of any linked site or any link contained in a linked site, or any changes or updates to such sites. Microsoft is not responsible for webcasting or any other form of transmission received from any linked site. Microsoft is providing these links to you only as a convenience, and the inclusion of any link does not imply endorsement of Microsoft of the site or the products contained therein.

© 2020 Microsoft Corporation. All rights reserved.

Microsoft and the trademarks listed at <https://www.microsoft.com/legal/intellectualproperty/Trademarks/Usage/General.aspx> are trademarks of the Microsoft group of companies. All other trademarks are property of their respective owners.

**Contents**

<!-- TOC -->

- [Secure cloud solution whiteboard design session student guide](#secure-cloud-solution-whiteboard-design-session-student-guide)
  - [Abstract and learning objectives](#abstract-and-learning-objectives)
  - [Step 1: Review the customer case study](#step-1-review-the-customer-case-study)
    - [Customer situation](#customer-situation)
    - [Customer needs](#customer-needs)
    - [Customer objections](#customer-objections)
    - [Infographic for common scenarios](#infographic-for-common-scenarios)
  - [Step 2: Design a proof of concept solution](#step-2-design-a-proof-of-concept-solution)
  - [Step 3: Present the solution](#step-3-present-the-solution)
  - [Wrap-up](#wrap-up)
  - [Additional references UPDATE LINKS](#additional-references-update-links)

<!-- /TOC -->

# Secure cloud solution whiteboard design session student guide

## Abstract and learning objectives

In this whiteboard design session, you will learn how to implement different components of Azure resources for securing cloud solutions in Azure. Areas to address include:

- Identity: Authorization and Access for B2C and B2B scenarios
- Secure Azure PaaS resources such as App Services web apps, Azure SQL from public endpoints
- Secure your secrets,passwords, and application configuration data from code repositories to production
- Use Azure Policies and Role Based Access Control (RBAC).

At the end of this whiteboard design session, you will improve your knowledge of Authorization and access of developer resources. You will improve your knowledge of data encryption at rest and in transit. Learn how to keep solution secrets outside of source control. Learn about secret management and abstract key rotation away from applications and their developers. You will be able to better protect applications in the cloud from external threats by guarding public endpoints. Learn to give your applications access to secrets so development teams won't need to keep track or even know the actual secrets.

## Step 1: Review the customer case study

**Outcome**

Analyze your customer's needs.

Timeframe: 15 minutes

Directions: With all participants in the session, the facilitator/SME presents an overview of the customer case study along with technical tips.

1. Meet your table participants and trainer.

2. Read all of the directions for steps 1-3 in the student guide.

3. As a table team, review the following customer case study.

### Customer situation

Contoso Plumbing Supply (Contoso) is a family owned wholesale plumbing supply company founded in Minneapolis, Minnesota that has been around for over 40 years. In that time, the company has thrived by embracing new business technology and practices, a mindset passed on to the current CEO and granddaughter [NAME HERE] of the company founder.

In the 1990's, Contoso created a data center in the back of their main warehouse to handle customer orders and supply chain management. Over the years Contoso has modernized their data center to keep up with demand and it has worked well for them, allowing Contoso to acquire two plumbing supply companies that were not as fortunate, and a regional retail plumbing store.

Contoso now has warehouse locations in Pennsylvania, Seattle, and Nevada and retail locations in the surrounding states. In order to serve these locations, Contoso is adopting cloud technology to provide supply chain services and direct to consumer ordering. Additionally, Contoso wants to migrate all the functionality to the cloud and retire their small local data center.

Contoso's main concern is providing **secure connectivity** for it's own suppliers and customers while also providing an easy online experience for them. Contoso currently manages authentication with a private Identity server and wants to move to a more robust solution.

With the addition of a lot of new employees, Contoso needs a solution that will provide authentication to the system as well as handle permissions and access to specific data. In particular, customers that purchase from the retail website should not be able to access the wholesale prices, but a sales rep at the store location should be able to see that information in order to make on the spot deals. This requires a system for access and authentication that can be centrally controlled by the management teams.

In addition to Authentication and Access management (Identity), Contoso wants to see secure network architecture for the various locations. The web applications need to be secure as well, with no application secrets stored in any config or application settings so they can be managed by the security team only. Contoso wants the security team to control all the certificates, keys, and tokens for the applications and users at the company.

### Customer needs

1. Identity Login: Retail users (the public) should be able to create an account using a social login and save payment details and view past orders. New employees should be able to login with single sign-on and require multi-factor authorization.
2. Identity Roles: Employees should have access only to the information they need to do their job.
3. Data encryption: All data is encrypted at rest and in transit.
4. Network Security: Data needs to be consistent, which means information from one location and others needs to sync, and do so securely.
5. Developers should not have access to production application secrets.
6. The Application will need to authenticate with Azure services such as Key Vault.
7. Client applications will need to authenticate - Contoso needs a strategy for access tokens and authentication.

*This solution should be a combination of securing **SQL server** using **TDE** and **always encrypted** setting up network security on the **VNET** using **NSGs**. Additionally, the application secrets should be in **Azure Key Vault** and the connections should all be **SSL** and **tls**. The application should require log-in through AAD or social using Microsoft Identity v2. The data needs to be consistent so back up and replication syncing should be included. **Cosmos DB** is another possible solution either to replace or in addition to SQL server. Any storage accounts or other types of access can use **shared access signatures** with policies or some other form of **revokable token** for access. security.*

*The solution will be deployed in multiple regions and use Traffic Manager with WAF on the front-end. The solutions will use auto scale with app services for the web sites based on traffic and cpu. For the fast performance in ordering, users should use some form of CQRS and messaging (Table Queues or Azure Service Bus) and the order subscriber to the queue should query the queue length to autoscale.*

### Customer objections

1. Add customer objections here.

### Infographic for common scenarios

put the infographic here (show the current app set up - the new one will be greenfield)

## Step 2: Design a proof of concept solution

**Outcome**

Design a solution and prepare to present the solution to the target customer audience in a 15-minute chalk-talk format.

Time frame: 60 minutes

**Business needs**

Directions:  With all participants at your table, answer the following questions and list the answers on a flip chart:

1. Who should you present this solution to? Who is your target customer audience? Who are the decision makers?

2. What customer business needs do you need to address with your solution?

**Design**

Directions: With all participants at your table, respond to the following questions on a flip chart:

*High-level architecture*

1. Without getting into the details (the following sections will address the particular details), diagram your initial vision for handling the top-level requirements for the mobile and web applications, data management, search, and extensibility.

*Secure application secrets*

1. List the requirements for securing the different applications secrets

*Secure Data management*

1. List the requirements for TDE and Always Encrypted here.

*Networking*

1. List requirements for securing data in transit over the network

*Extensibility*

1. How will you ensure the application scales and can expand into new regions?

2. How will you ensure the solution will scale appropriately under heavy traffic?

**Prepare**

Directions: With all participants at your table:

1. Identify any customer needs that are not addressed with the proposed solution.

2. Identify the benefits of your solution.

3. Determine how you will respond to the customer's objections.

Prepare a 15-minute chalk-talk style presentation to the customer.

## Step 3: Present the solution

**Outcome**

Present a solution to the target customer audience in a 15-minute chalk-talk format.

Timeframe: 30 minutes

**Presentation**

Directions:

1. Pair with another table.

2. One table is the Microsoft team and the other table is the customer.

3. The Microsoft team presents their proposed solution to the customer.

4. The customer makes one of the objections from the list of objections.

5. The Microsoft team responds to the objection.

6. The customer team gives feedback to the Microsoft team.

7. Tables switch roles and repeat Steps 2-6.

## Wrap-up

Timeframe: 15 minutes

Directions: Tables reconvene with the larger group to hear the facilitator/SME share the preferred solution for the case study.

## Additional references UPDATE LINKS

|                 |           |
|-----------------|-----------|
| **Description** | **Links** |
| Hi-resolution version of blueprint | <https://msdn.microsoft.com/dn630664#fbid=rVymR_3WSRo> |
| Key Vault Developer's Guide | <https://azure.microsoft.com/documentation/articles/key-vault-developers-guide/> |
| About Keys and Secrets | <https://msdn.microsoft.com/library/dn903623.aspx> |
| Register an Application with AAD | <https://azure.microsoft.com/documentation/articles/key-vault-get-started/#register> |
| How to Use Azure Redis Cache | <https://azure.microsoft.com/documentation/articles/cache-dotnet-how-to-use-azure-redis-cache/> |
| Intro to Redis data types & abstractions | <http://redis.io/topics/data-types-intro> |
| Working with Azure Functions Proxies | <https://docs.microsoft.com/azure/azure-functions/functions-proxies> |
| Azure API Management Overview | <https://docs.microsoft.com/azure/api-management/api-management-key-concepts> |
