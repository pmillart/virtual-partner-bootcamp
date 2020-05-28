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
  - [Customer Requirements](#customer-requirements)
      - [Provide Unified Identity Provisioning, Management, and Login](#provide-unified-identity-provisioning-management-and-login)
      - [Utilize Approrpriate Configuration and Access Control Using Identity Roles](#utilize-approrpriate-configuration-and-access-control-using-identity-roles)
      - [Ensure Data Encryption - AT ALL TIMES (at rest and in motion)](#ensure-data-encryption---at-all-times-at-rest-and-in-motion)
      - [Provide Robust Application Security](#provide-robust-application-security)
      - [Contoso Development Team Characteristics](#contoso-development-team-characteristics)
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
- Secure your secrets, passwords, and application configuration data from code repositories to production
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

The renewable energy industry is growing fast!   

Contoso Solar Inc (Contoso) is a family owned commercial and residential solar installation and wholesale supply firm headquartered in the Pacific NorthWest, Unitest States.    

Established 2004, Contoso have almost tripled in size, and have taken on a lot of new projects that even take them from a US Domestic only operation, to an international operation with the the closing of a lucrative project from neighboring British Columbia, Canada.   This has been a long and exciting road that involved a 36 month app-modernization effort coupled with a recent lift and shift to the cloud using Azure.  This was followed again by an 18 month re-design of their key applications to take advantage of serverless deployments to take advantage of swifter time to market to accomodate the massive grwoth enjoyed industry wide.  

Now the overall cost to install solar has dropped by almost 70% and charging and conversion efficiency has increased to boot making solar more accessible - in many cases, with components converging on commodity status.  

Contoso currently has three satellite offices, each supporting installation, fulfullment, and distribution teams:  Portland, OR, Seattle, WA, and Austin, TX.   And, as a result of their success in the industry as well as achieving GDPR/CCPA compliance including a SOC 3 certification, they will be opening a new office in Vancouver, BC to tackle the new commercial business acquired in BC which is the installation of solar for a number of provincial buildings including schools, and transit stations.   

Contoso now has most of their key line of business applications, data storage, and analysis operations on Azure and their enterprise supports direct integrations with first tier solar technology manufacturers and component suppliers. 

Contoso's main concern is providing **robust application security** for it's own suppliers and customers while also providing an easy online experience for them. Contoso currently manages authentication via Azure Active Directory but wants to reconfigure to support a more robust solution for 3 major user experiences:

* **B2B** - Solar Manufacturer/Providers, including professional technical contributors such as: architectural, engineering, and regulatory/legal advisors from state and local entities

* **B2C** - DYI Consumers purchasing components and services directly - either wholesale or retail, and/or opting in persistent account access enabling consumer access to extended offerings provided by Contoso Solar Consumer Services including pre- and post- sales technical support, blog access, and access to technical events sponsored by the industry

* **OPS** - Internal employee groups such as C-Suite, HR, IT, Fulfillment, Installation Teams, Partner and Consumer Services, Sales and Marketing

With these use cases and the addtion of new infrastructure, locations, and employees, Contoso needs a solution that will provide reliable authentication but in addition, they require robust account management functionality, as well as the ability to secure and audit application configuration, and integration events. 

In particular, Suppliers accessing Contoso's Supplier Workflow and APIs should only have access to resources granted to those accounts.   One Supplier should not be able to interact with allocated resources of another supplier.   

Customers purchasing from the retail website should not be able to access wholesale prices, but an OPS Contoso Sales Account Manager should be able to see both retail and wholesale information for both B2B and B2C scenarios. 

This requires a system for access and authentication that can be centrally controlled by the Contoso OPS IT Team.

In addition to Authentication and Access management (Identity), Contoso wants to ensure that their backend infrastructure use as much of the Azure backbone as possible to include direct integrations with their cloud-based CRM and ERP systems.   And of course, any APIs and Web Applications need to be secure as well, with no application secrets stored in any config or application settings so they can be managed by the security team in accordance with their InfoSec Policy. 

Contoso wants the security team to control all certificates, keys, and tokens for the applications and users at the company.

## Customer Requirements 

he following requirements are a result of over a year of hard work assessing Contoso's IT security requirements in order to maintain compliance with certain certifications they need to work with global providers and provide for their customers, as well as transacting with first-tier payment providers and certifcation organizations. 

Fulfiliing these requirements using robust, repeatable capabilities is essential to Contoso's continued growth and success.   Failure in any one of these areas can jeaopardize the firm's ability to maintain their current level of compliance    

#### Provide Unified Identity Provisioning, Management, and Login
As per use cases above, there are three valid authentication scenarios:  

- **B2B** - DIY Retail Consumers should be able to use the ecommerce purchase path to purchase solar components with an anonymous cart purchase path.   Or they can create a persistent account with Contoso, using an email as username or using a social login from a valid Identity Provider, and save payment methods and view order status and order history along with sharing other social details if they desire.   This method gives them access to pre- and post- sales technical support as well as event and blog content.  

- **B2C** - DIY Retail Consumers should be able to use the ecommerce purchase path to purchase solar components with an anonymous cart purchase path.   Or they can create a persistent account with Contoso, using an email as username or using a social login from a valid Identity Provider, and save payment methods and view order status and order history along with sharing other social details if they desire.   This method gives them access to pre- and post- sales technical support as well as event and blog content.  

- **OPS** - Existing and New internal employees should be able to login with thier assigned Active Directory Work Account at work and on their devices.  Optionally this authentication flow may require multi-factor authorization (MFA).   Once authenticated, the user will experience single sign-on (SSO) behavior across enterprise applications and endpoints until they fully log out.

#### Utilize Approrpriate Configuration and Access Control Using Identity Roles
As per Contoso's Infosec and Data Handling Policies, there is an active Segregation of Duties (SOD) Matrix that defines a list of account roles and security groups within the company structure.   Employees should have access only to the information they need to perform their duties.  This requirement implies use of Role-based Access Control (RBAC) within the enterprise to match the SOD matrix.   

OPS employees are allocated to roles and groups based on Employee Job Description and this will determine access to key resources, systems, and data.  

#### Ensure Data Encryption - AT ALL TIMES (at rest and in motion)
As Per Contoso's Infosec and Data Handling Policies, data will be encrypted throughout the application stack, regardless of location and status and this includes during application syncronization (raw data file upload/download, application data syncronization, and API transactions). 

**Contoso Data Classification**

Listed below is a simple data classification of Contoso secure data: 

- **Business Critical** - Client Architectural, Engineering, Requirements, or Legal and Regulatory documents uploaded by architectural firms or by state and local regulatory entities 
- **Business Critical** - ERP, Transactional Data related to Wholesale and Retail Transactions, and Service Process data classified as **Business Critical ** by Contoso
- **Business Critical** - CRM, Sales Pipeline/Biz-Dev, Supplier, and Consultative Contributor, and Persistent Retail Customer profile data
- **Mission Critical** - Post process Analysis, Poduct Definitions, Product Catalog
- **PII** - Employee HR data to include data that is considered PII by GDPR/CCPA standards

#### Provide Robust Application Security
Contoso is almost completely in the Azure Cloud, with only their 3 installation warehouses with minimal on-prem compute resources, mostly to drive installation kitting automation they have in house.  

Contoso operates mostly in North American West Coast. This means that for now, performance for multiple regions takes a back seat to the more emergent security configuration concerns.  The IT Group has implemented minimal cloud-based HADR, but since Contoso is growing, their IT Group will revisit these topics in the near future after the security upfit is complete.  

At this point, consider all front-end APIs and Web Apps are written using .Net/ASP.NET Core and are deployed to auto-scaled app service plans.   In the near future, the Application Architecture Team is considering converting to a CQRS architecture to address scale concerns for upcoming growth by using an event/messaging, or pub-sub design pattern. 

In terms of development requirements, a few things are clear:  
1. Developers should not have direct code-level access to production application secrets
2. Applications need to be configured to authenticate with Azure services such as Azure SQL Database, and Azure Key Vault
3. Client applications will need to authenticate discreet classes of users - Contoso needs a strategy for access tokens and authentication

**NOTE**

While application security is the primary concern right now, the client is growing, so consideration for scale and performance should be included during design.

#### Contoso Development Team Characteristics 
Contoso's Development Team have settled on Azure DevOps as their primary CI/CD platform and the run a tight ship with a clean backlog, using a Git-Flow   

Applications are deployed using Azure DevOps Pipelines with minimal 3rd party tool interaction with the exception of some common open source testing and verification/risk management libraries.  

Their typical sprint is 7 days with trunk releases (major, minor, and patch) in a blue-green configuration every Thursday.  

### Customer objections

1. Add customer objections here.

### Infographic for common scenarios

put the infographic here (show the current app set up - the new one will be greenfield)

## Step 2: Design a proof of concept solution

**Outcome**

Design a solution and prepare to present the solution to the target customer audience in a 15-minute chalk-talk format.

Time frame: 60 minutes

**Business needs**

Directions:  With all participants at your table, answer the following questions and list the answers on a flip chart.  

First, start defining the team approach by identifying the following:
1. Who is your target customer audience?   
2. Who should you present this solution to?  
3. Who are the decision makers in the scenario?
4. What customer business needs do you need to address with your solution?
5. Is there a priority or order in implementation that you think the cient should consider? 

**Design**

Directions: With all participants at your table, respond to the following requirements:

*Produce a High-level architecture*

- Without getting into the details (the following sections will address the particular details), diagram your initial vision for handling the top-level requirements for the mobile and web applications, data management, search, and extensibility.

*Provide data security from the primary data store forward to API/Application*

- List the requirements for securing data at rest and in motion across the client stack using features like Transparent Data Encryption and Always Encrypted
- How can the client use their current infrastructure to limit what data users have access to as determined by role? 
- What methods can be applied to protect sensitive customer information such as transaction and PII classified data? 

*Provide security for Sensitive Application Configuration Information*

- List the requirements for securing application secrets, connection strings, credentials, and related configuration information
- List requirements for ensuring application protocol security for distributed web applications 

*Provide security for bulk data upload and transfer between partner suppliers and client*

- Describe simple but effective ways in which the client can use Azure Services to enable Suppliers to integrate in terms of large file-based data bi-directional transfers? 
- What other Azure Tools or Services can be used to facilitate secure data transfers across corporate boundaries?

*Extensibility*

- Using Azure, how can the client ensure application scaling and performance as they grow and expand into new regions?
- What new Azure services or configurations might be required to ensure that the entire solution will scale appropriately under high demand?
- What can the Contoso Architecture Team consider to "future proof" application architecture to promote an easier extensibility path to match growth potential?

**Prepare**

Directions: With all participants at your table:

- Identify any customer needs that are not addressed with the proposed solution

- Identify the benefits of the proposed solution, and list potential caveats

- Determine how you will respond to the customer's objections

- Prepare a 15-minute, staged "chalk-talk" presentation to the customer

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
| Azure SQL Database TDE (BYOK)         | <https://docs.microsoft.com/en-us/azure/azure-sql/database/transparent-data-encryption-byok-configure> |
| Azure SQL Database Always Encrypted   | <https://docs.microsoft.com/en-us/azure/azure-sql/database/always-encrypted-certificate-store-configure> |
| Azure SQL Database AE Colum Master Key Overview |<https://docs.microsoft.com/en-us/sql/relational-databases/security/encryption/overview-of-key-management-for-always-encrypted>|
| Azure SQL Database AE Column Master Key Management| <https://docs.microsoft.com/en-us/sql/relational-databases/security/encryption/create-and-store-column-master-keys-always-encrypted> |
| Azure SQL Database Row Level Security | https://azure.microsoft.com/en-us/resources/videos/row-level-security-in-azure-sql-database/ |
| Azure Key Vault Developer's Guide     | <https://azure.microsoft.com/documentation/articles/key-vault-developers-guide/>|
| About Keys and Secrets                | <https://msdn.microsoft.com/library/dn903623.aspx> |
| Azure API Management Overview         | <https://docs.microsoft.com/azure/api-management/api-management-key-concepts> |
| Register an Application with MID V2   | <https://docs.microsoft.com/en-us/graph/auth-register-app-v2>|
| Microsoft Identity Platform and OpenID Connect |<https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-protocols-oidc>|
| Azure MSAL Authentication Flows       | <https://docs.microsoft.com/en-us/azure/active-directory/develop/msal-authentication-flows>|
| Working with Azure Functions Proxies  | <https://docs.microsoft.com/azure/azure-functions/functions-proxies> |
|Git Trunk-based Development|https://trunkbaseddevelopment.com/|
