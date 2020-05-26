# Keep In Touch App Challenge Guide

## Challenge 5: Validating access

In this challenge, you will add some skills to the two users that you interacted with in the Icebreaker app. In an incognito or InPrivate browser session visit
<br>
```
https://developer.microsoft.com/en-us/graph/graph-explorer
```
<br>
<span class="colour" style="color:rgb(36, 41, 46)">Next open Graph Explorer and sign in with your Azure credentials to validate the permissions that were just setup. This will involve running two simple queries. The first just gets a simple list of all the users setup in Azure Active Directory. Copy the following value into Graph Explorer and click the Run Query button. You should get a 200 Success message with a list of users.</span>

[https://graph.microsoft.com/v1.0/users/](https://graph.microsoft.com/v1.0/users/)

![](images/user-profile.png)

Choose one of the users you setup with a profile and copy the id value (without the quotes. Open a new tab to Graph Explorer and run the following query to validate you can get the users profile information. Replace the {id} with the value you previously copied. Then click Run Query.
You should see one user with their respective responsibilities & skills listed.  
[https://graph.microsoft.com/v1.0/users/{id}?$select=userPrincipalName,displayName,skills,responsibilities](https://graph.microsoft.com/v1.0/users/{id}?$select=userPrincipalName,displayName,skills,responsibilities)  

![](images/testuser.png)


## Success Criteria

* <span class="colour" style="color: rgb(36, 41, 46);">You have written basic queries for Graph API and validated that you have allowed the appropriate access.</span>

## Progressing to the next challenge
