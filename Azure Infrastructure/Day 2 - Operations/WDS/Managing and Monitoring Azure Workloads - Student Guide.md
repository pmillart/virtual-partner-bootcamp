![Microsoft Cloud Workshops](https://github.com/Microsoft/MCW-Template-Cloud-Workshop/raw/master/Media/ms-cloud-workshop.png "Microsoft Cloud Workshops")

<div class="MCWHeader1">
Managing and Monitoring Azure Workloads
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

Microsoft and the trademarks listed at https://www.microsoft.com/en-us/legal/intellectualproperty/Trademarks/Usage/General.aspx are trademarks of the Microsoft group of companies. All other trademarks are property of their respective owners.

**Contents**

<!-- TOC -->

- [Managing and Monitoring Azure Workloads - Whiteboard Design Session Student Guide](#managing-and-monitoring-azure-workloads---whiteboard-design-session-student-guide)
  - [Abstract and learning objectives](#abstract-and-learning-objectives)
  - [Step 1: Review the customer case study](#step-1-review-the-customer-case-study)
    - [Customer background](#customer-background)
    - [Technical background](#technical-background)
    - [Current situation](#current-situation)
    - [Customer needs](#customer-needs)
    - [Customer objections](#customer-objections)
  - [Step 2: Design a proof of concept solution](#step-2-design-a-proof-of-concept-solution)
  - [Step 3: Present the solution](#step-3-present-the-solution)
  - [Wrap-up](#wrap-up)
  - [Additional references](#additional-references)

<!-- /TOC -->

# Managing and Monitoring Azure Workloads - Whiteboard Design Session Student Guide

## Abstract and learning objectives 

In this whiteboard design session, you will look at how to design monitoring and management solutions in Azure. Your focus will be on a partner Managed Service Provider scenario, where management must be provided at scale across a large number of customers.

At the end of the workshop, you will be better able to design and use Azure monitoring and management tools, including Azure Monitor Workbooks, Log Analytics, Azure Alerts, and Azure Automation. You will also understand how to deploy these technologies across a large portfolio of customers using a variety of automation technologies. You will also better understand the challenges involved in managing Azure solutions at scale.

## Step 1: Review the customer case study 

**Outcome** 

Analyze your customer’s needs.

Timeframe: 15 minutes 

Directions: With all participants in the session, the facilitator/SME presents an overview of the customer case study along with technical tips. 

1.  Meet your table participants and trainer. 

2.  Read all of the directions for steps 1–3 in the student guide. 

3.  As a table team, review the following customer case study.
 
### Customer background

Contoso Cloud Services (CCS) is a Microsoft Partner providing a broad portfolio of cloud-focused IT solutions to its customers.

CCS was founded in Atlanta in 1998, originally as Contoso IT Services. The original business successfully rode the dot-com bubble, creating and hosting web sites as customers established their on-line presence. The hosting side of the business grew into a substantial managed services business, with CCS building hosting facilities in Atlanta, New York, Denver and San Jose.

The CEO of CCS, Jack Walsh, was quick to recognize that the dawn of the cloud era would transform his industry. In 2015 he re-branded the company as Contoso Cloud Services and set in place a new business strategy offering cloud-based and hybrid cloud services. Given CCS’s long history as a Microsoft partner, Microsoft Azure was the natural choice as preferred public cloud provider.

CCS has built a broad base of around 600 managed services customers, mostly medium-sized and large businesses. More recently CCS has also acquired a handful of enterprise customers, reflecting the company’s ongoing growth and success.

### Technical background

The current CCS portfolio of managed customer applications comprises:
-   Legacy on-premises applications hosted in CCS’s four datacenters. The strategy is to migrate these solutions to cloud-based alternatives as hardware ages, and eventually to reduce the datacentre footprint from 4 sites to 2.
-   Hybrid solutions, using an 200Mbps ExpressRoute connection supplied by the existing CCS WAN provider
-   Cloud-only customer solutions. These comprise both IaaS solutions (mostly arising from lift-and-shift migrations) and a smaller but growing portfolio of modernized PaaS-based applications.

CCS is a direct CSP. CSP Azure subscriptions are used by most customers. However, some customers are using pay-as-you-go subscriptions which they acquired before engaging CCS, and their enterprise customers use EA subscriptions.

### Current situation

As head of Cloud Operations for Contoso Cloud Services, you are responsible for keeping all your in-house and customer managed services healthy, secure and available 100% of the time. As your cloud business grows, you need to scale your operations activities accordingly—but you can only use the resources your already have.

Currently, your team manages each customer and application individually--so as the number of customers and applications grows, so does your workload. Pretty soon, the team aren't going to be able to cope. The only solution is to improve your teams' efficiency.

Your challenge is to redesign your operations processes for horizontal scale. You need to create efficient processes that enable your team to deliver core operations scenarios—such as health alerts, dashboards, backup and patching—across your growing customer base.

### Customer needs 

1.	**Subscription access** Access to customer subscriptions uses a combination of CSP ‘Act On Behalf Of’ (AOBO) and AAD B2B Collaboration. Managing access as team members change is a challenge, and some enterprise customers are disgruntled at the overhead on their side of managing changes to B2B guest accounts. CCS need a solution that provides access to customer subscriptions without undue customer overhead, full transparency, and fine-grained access control.

2.	**Backup configuration and monitoring** A key part of CCS’s offerings is backup management. CCS uses Azure Backup for backup of both on-premises and cloud-based servers and databases. Backup monitoring and management is a growing time sink for the operations team. In a recent incident it was found that backup jobs had been failing on one customer application for the past 2 months. Alerts had failed due to a missing diagnostics logging configuration. No one knows how many other applications are similarly affected. CCS need a scalable and reliable backup monitoring solution.

3.	**Patch management** CCS are using System Center for patch management for both on-premises and cloud-based servers. Their goal is to deprecate System Center in favour of a cloud-based alternative. This must provide comprehensive coverage across their entire server estate (on-premises/cloud, Linux/Windows).

4.	**VM and server monitoring** CCS are managing a large number of VMs across their customer subscriptions. VM diagnostics and monitoring has been configured for each application over time, using a variety of approaches. These include Azure metrics, diagnostics storage accounts, and Log Analytics. CCS would like to settle on a unified approach to server monitoring across their entire estate, including on-premises machines.

5.  **Automation of manual tasks** Occasionally, CCS need to make configuration changes to on-premises servers or to Azure-based applications. A diverse and unpredictable range of activities is involved. Examples from the past 6 months include changing the endpoint for a file share, installing a new anti-malware product, rotating passwords and auditing database accounts. These tasks are currently carried out manually. CCS are seeking a general-purpose solution to enable these manual tasks to be automated.

6.  **PaaS monitoring** CCS are experienced at monitoring IaaS-based workloads. They are less experienced with PaaS workloads, and are unsure what level of monitoring they need to implement vs what is taken care of by the cloud provider. As the number of managed PaaS applications increases, they need to define best practice patterns for SQL Database and Web App monitoring and alerts.

7.  **VM and server compliance** An internal security audit recently identified several on-premises and Azure servers which did not comply with CCS standards. They need a way to prevent this happening in future.

8.  **Cost management** As Azure spending increases, there have been several incidents in which CCS customers have complained about unnecessary spend on their Azure bill or questioned the value that Azure is bringing to their organization. CCS need a way to manage, report and optimize Azure costs for their customers.

### Customer objections 

1.  Cost is a concern. As well as being cost-efficient, CCS prefer solutions that keep any costs associated with managing a customer solution within the customer subscription, rather than centralizing management costs into a CCS subscription and then having to apportion across customers.

2.  CCS are finding that their new enterprise customers are more demanding regarding security and compliance. How can CCS demonstrate their services meet enterprise standards for secure operations?

## Step 2: Design a proof of concept solution

**Outcome** 

Design a solution and prepare to present the solution to the target customer audience in a 15-minute chalk-talk format. 

Timeframe: 60 minutes

**Design**

Directions: With all participants in your group, respond to the following questions on a flip chart or (virtual) white board.

1.  **Subscription access** Design a solution to allow the CCS operations team with access to customer subscriptions.
    - The solution should support role-based access control, so the customer has visibility and control over which permissions are granted to CCS.
    - The solution should support user groups, so changes to the CCS operations team do not require customer involvement.
    - There solution should support all subscription types (PAYG, CSP, EA, etc).

    Are there any Azure features which are not compatible with your proposed solution?

2.  **Backup configuration and monitoring** How can CCS monitor Azure Backup health? Design a solution that enables:
    - Visibility into backup health across all customers
    - Identify VMs without backup enabled
    - Raise alerts upon backup failure, across all customers
    - Ensure backup monitoring is correctly consistently applied to all Recovery Services Vaults

    Does your solution scale to the number of customers and subscriptions managed by CCS?

3.  **Patch management** What alternatives to System Center does Azure offer for patch management? Design an approach that offers:
    - Support for on-premises and cloud-based servers
    - Support for both Windows Server and Linux
    - Automatic on-boarding for Azure virtual machines and VM scale sets

    How does your solution scale patch management across the CCS customer base?

4.  **VM and server monitoring** Design a solution for monitoring Azure VMs and on-premises servers. Your solution should support:
    - Streamlined onboarding of all Azure VMs and selected on-premises servers
    - Support for dashboards and alerts based on both metrics and logs. 
    - Customer billing according to usage
    - Compliance with Enterprise customer security requirements that all logs remain within their subscription boundary

    How can you ensure your solution is deployed consistently across all CCS customers, for data gathering, alert configuration, and reporting?

    Can your solution deliver flexible and highly configurable dashboards of key performance data?
    
    Can your dashboards show data aggregated across the CCS customer base? What limitations apply?

5.  **Automation of manual tasks** How can CCS automate their manual configuration tasks? Your solution must support:
    - Automation of Azure resource configuration tasks (e.g. VM resize, disk encryption, network changes)
    - Automation of operations for Azure VMs (e.g. rotating passwords, installing or uninstalling software, auditing permissions)
    - Automation of operations for on-premises servers

    How can you deploy automation at scale across the CCS customer base?

    How can you review the success or failure of each automation task, on each resource?

    Automating common tasks is a substantial investment in IP. How can you protect the confidentiality of this investment?

6.  **PaaS monitoring** Design a solution for monitoring PaaS services, in particular Azure SQL Database and Azure Web Apps. Your solution should support:
    - Gathering key metrics and log data for alerting and investigation of incidents
    - Configuration of alerts in the event of performance degradation (e.g. increased latency), errors, or critical events.

    What can you do to make your monitoring more representative of the end-user experience?

    A customer raises a support ticket claiming their application is 'slow'. How can your monitoring help investigate the cause?

7.  **VM and server compliance**  Propose a solution that enables CCS to audit the compliance of their entire server estate - Windows and Linux, on-premises and cloud.

    Your solution should support a wide range of common OS configuration issues, such as members of the local administrator group, password complexity, installed applications, and trusted certificates. The solution should also be extensible to support new issues as required by CCS.

    Explain how this solution scales to all CCS customers.

8.  **Cost management** Explain how CCS can meet their cost management requirements. These include:
    - Visibility into current and historical spend, with the ability to segment the data by a variety of categories (such as location, resource type, or tags)
    - Forecast of future spend
    - Cost saving recommendations
    - Configuration of spending alerts, including alerting at 1-day granularity in case of a spike in spending

    Cost reports should be available to customers, including CSP customers. However, any CSP discounts should be confidential to CCS.

**Prepare**

Directions: With all participants at your table: 

1.  Identify any customer needs that are not addressed with the proposed solution.

2.  Identify the benefits of your solution.

3.  Determine how you will respond to the customer’s objections.

## Step 3: Present the solution

**Outcome**
 
Present a solution to the target customer audience in a 15-minute chalk-talk format.

Timeframe: 30 minutes

**Presentation** 

Directions:
1.  Pair with another table.

2.  One table is the Microsoft team and the other table is the customer.

3.  The Microsoft team presents their proposed solution to the customer.

4.  The customer makes one of the objections from the list of objections.

5.  The Microsoft team responds to the objection.

6.  The customer team gives feedback to the Microsoft team. 

7.  Tables switch roles and repeat Steps 2–6.

##  Wrap-up 

Timeframe: 15 minutes

Directions: Tables reconvene with the larger group to hear the facilitator/SME share the preferred solution for the case study.

##  Additional references

|                                         |                                                                   |
|-----------------------------------------|:-----------------------------------------------------------------:|
| **Description**                         | **Links**                                                         |
| Microsoft Azure Reference Architectures | <https://docs.microsoft.com/azure/guidance/guidance-architecture> |
| Azure Lighthouse                        | <https://docs.microsoft.com/azure/lighthouse/>                    |
| Azure Backup Explorer                   | <https://docs.microsoft.com/azure/backup/monitor-azure-backup-with-backup-explorer>                        |
| Azure Arc for Servers                   | <https://docs.microsoft.com/azure/azure-arc/servers/overview>     |
| VM Guest Policy                         | <https://docs.microsoft.com/azure/governance/policy/concepts/guest-configuration>     |
| Azure Monitor for VMs                   | <https://docs.microsoft.com/azure/azure-monitor/insights/vminsights-overview>   |
| Azure Automation                        | <https://docs.microsoft.com/azure/automation/automation-intro>   |
| Azure Update Management                 | <https://docs.microsoft.com/azure/automation/automation-update-management>  |
| Web App monitoring                      | <https://docs.microsoft.com/azure/architecture/reference-architectures/app-service-web-app/app-monitoring> |
| SQL Database monitoring                 | <https://docs.microsoft.com/azure/sql-database/sql-database-monitor-tune-overview>    |
| Azure Cost Management                   | <https://docs.microsoft.com/en-us/azure/cost-management-billing/ > |
|                                         |                                                                   |
                                        