# Keep In Touch App Challenge Guide

## Challenge 4: Configure App Permissions

In this challenge, you will configure Azure AD to allow your app to access to the Graph API. 

In an incognito or InPrivate browser session visit
<br>
```
https://portal.azure.com
```
<br>
In the Azure portal visit Azure Active Directory. Because it is necessary to read information from the Graph API we need to add permissions.

* Click on App Registrations and select the Application registered when creating the IceBreaker app. Then click on API Permissions in the left-hand navigations.
* Click on the Add a permission button.
* Click on the Microsoft Graph button
* Click on Application permissions
* Scroll down till the Users permission section is visible and check User.Read.All
* Finally click Add permissions
![](images/user-profile-permission.png)


## <span class="colour" style="color:rgb(36, 41, 46)">Succcess Criteria</span>

* <span class="colour" style="color:rgb(36, 41, 46)">You have allowed permissions to utilize the Graph API</span>

## Progressing to the next challenge


## Resources
```
https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-configure-app-access-web-apis
```
