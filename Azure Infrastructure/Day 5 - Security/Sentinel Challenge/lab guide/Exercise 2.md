# Challenge Guide

## Challenge 2: Sentinel alerting and analytics

Now that you understand the data sources and volume of data that Contoso Finance is contending with, you can begin operationalizing the Sentinel deployment.

To do this, the first task will be to create unique workbooks for any data sources presented through the **CommonSecurityLog** table in the Log Analytics workspace that is attached to Sentinel. For each unique device vendor found in the log, a workbook will need to be created.

Once you have an understanding of individual event volumes, you can begin to construct alerting logic around them. For each distinct vendor you found in the log, create a scheduled rule which can alert on events with a severity greater than or equal to five if the event is generated more than 10 times in the last 5 minutes.

## Success Criteria

- Workbooks have been created to visualize event counts for each unique device vendor found in the **CommonSecurityLog**.
    - For each event type in the data source, present a count of events per day in a standard stacked bar chart.
- Schedule alert rules have been created for events with a severity greater than or equal to five and alerts are firing for each distinct vendor found in the **CommonSecurityLog**.

## Progressing to the next challenge

After you have completed the challenge, click the **Mark complete** button to inform your proctor you are ready for review.

## Help Resources

- <a href="https://docs.microsoft.com/azure/azure-monitor/platform/workbooks-overview" target="_blank">Azure Monitor Workbooks</a>
- <a href="https://docs.microsoft.com/azure/data-explorer/kusto/query/sqlcheatsheet" target="_blank">SQL to Kusto query translation</a>
- <a href="https://docs.microsoft.com/azure/sentinel/tutorial-monitor-your-data" target="_blank">Visualize and monitor your data</a>
- <a href="https://docs.microsoft.com/azure/azure-monitor/reference/tables/commonsecuritylog" target="_blank">CommonSecurityLog</a>
- <a href="https://docs.microsoft.com/azure/sentinel/resources" target="_blank">Useful resources for working with Azure Sentinel</a>
