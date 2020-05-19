# Challenge Guide

## Operationalizing Azure Kubernetes Service

## Challenge 2: Isolating workloads with multiple node pools

With the Fruit Smoothies application deployed and stabilized, Fruit Smashers would like to onboard a new application that allows visitors to their corporate headquarters to leave messages about the favorite part of their visit. As with the Fruit Smoothies application, Fruit Smashers wants to maintain the isolation of their applications with namespaces. They also recognize that the compute needs of their new application will be far less than their popular ratings app and don't want to impact the existing configuration.

The developers of the guestbook messaging application have identified the `Standard_B1ms` as a good target size for a two-node pool. They feel it meets not only their baseline CPU and memory needs from their testing, but they also fee that the B-series VMs are ideal for their workload as it does not need the full performance of the CPU continuously.

Your challenge is to configure a new node pool with two nodes and deploy the Guestbook application into a dedicated namespace, ensuring that the Guestbook application can be deployed only its dedicated pool members. Keep in mind the existing Fruit Smoothies application is currently in production and should not be impacted by the deployment of this new application and its associated services.

## Success criteria

Explain to your coach:

- How you approached the creation of a new node pool.
- How you ensured that the existing ratings application was available the whole time you were deploying the new application.
- How you determined the size you would use for each VM in the new node pool.

## Progressing to the next challenge

After you have completed the challenge, click the **Mark complete** button to inform your coach you are ready for review.

## Help Resources

- <a href="https://docs.microsoft.com/azure/aks/operator-best-practices-multi-region" target="_blank">Best practices for business continuity and disaster recovery in Azure Kubernetes Service (AKS)</a>
- <a href="https://docs.microsoft.com/azure/aks/use-multiple-node-pools" target="_blank">Create and manage multiple node pools for a cluster in Azure Kubernetes Service (AKS)</a>
- <a href="https://kubernetes.io/docs/concepts/scheduling-eviction/taint-and-toleration/" target="_blank">Taints and Tolerations</a>
