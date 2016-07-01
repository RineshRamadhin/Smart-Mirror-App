# Smart-Mirror-App
Repository for the Windows Application for the Smart Mirror - ICT-LAB (INFLAB01 &amp; INFLAB02) 

### Add Google services 
To add a new service of Google follow the next steps:
- Create a model according to the data you want from a specific Google Api in the folder "Models".
- Create a class in the folder Data/Api and inherit it with with class "DefaultGoogleService".
- Implement the missing methods like the other available Google Services
- Optional: If you need the data to be inserted to the DB. Create a table class in the folder Data/Databases and inherit the class "DefaultDatabaseTable"

### Add http services
To add a http service that gets data from HTTPRequests follow the next steps:
- Create a model according to the data you want from a specific API in the folder "Models".
- Create a class in the folder Data/Api and inherit it with with class "DefaultHttpClient".
- Implement the missing methods like the other available API services that inherits "DefaultHttpClient"
- Optional: If you need the data to be inserted to the DB. Create a table class in the folder Data/Databases and inherit the class "DefaultDatabaseTable"
