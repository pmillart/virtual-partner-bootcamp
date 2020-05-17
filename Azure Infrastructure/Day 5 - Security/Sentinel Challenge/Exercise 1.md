# Challenge Guide

## Azure Sentinel Hunting

## Overview

Contoso Finance has recently onboarded to Microsoft Azure and as a part of the security conscious culture, they have adopted Azure Sentinel to gain insights into their on-premises and cloud security. They have a provisioned a Sentinel workspace and begun to incorporate event data from their on-premises devices, including their Fortinet Fortigate next general firewalls. You have been brought on to make sure that the alerting infrastructure and underlying deployment are properly configured and will provide ongoing incident management for generated alerts.

![Contoso Finance logo](logo.png)

## Accessing Microsoft Azure

Launch Chrome from the virtual machine desktop and navigate to the URL below. Your Azure Credentials are available by clicking the Cloud Icon at the top of the Lab Player.

```sh
https://portal.azure.com
```

## Challenge 1: Understanding the environment

Before you can extend and further configure Contoso Finance's Sentinel deployment, you need to understand what they have done to date and help them plan for the future.

To do this, you must validate the sources of data for Sentinel and extrapolate the expected usage for Sentinel each day to help Contoso Finance understand costing for the service. While Contoso Finance has told you they are forwarding events from their on-premises devices such as their Fortinets, are there any other data sources present or have they missed any in their configuration?

## Success criteria

Explain to your proctor:

- How you validated the sources of data
- What the expected data volume over a day would be
- Are there any additional data connectors you would recommend for Contoso Finance?

## Progressing to the next challenge

After you have completed the challenge, click the **Mark complete** button to inform your proctor you are ready for review.

## Help Resources

- <a href="https://docs.microsoft.com/azure/sentinel/overview" target="_blank">What is Azure Sentinel?</a>
- <a href="https://docs.microsoft.com/azure/sentinel/connect-data-sources" target="_blank">Connect data sources</a>
- <a href="https://docs.microsoft.com/azure/sentinel/connect-common-event-format" target="_blank">Connect your external solution using Common Event Format</a>
- <a href="https://docs.microsoft.com/azure/sentinel/connect-fortinet" target="_blank">Connect Fortinet to Azure Sentinel</a>
- <a href="https://docs.fortinet.com/document/fortigate/6.0.9/fortios-log-message-reference/524940/introduction" target="_blank">FortiOS Log Message Reference</a>
- <a href="https://docs.microsoft.com/advanced-threat-analytics/cef-format-sa" target="_blank">ATA SIEM log reference</a>
- <a href="https://azure.microsoft.com/pricing/details/azure-sentinel/" target="_blank">Azure Sentinel pricing</a>
- <a href="https://docs.microsoft.com/azure/sentinel/resources" target="_blank">Useful resources for working with Azure Sentinel</a>
